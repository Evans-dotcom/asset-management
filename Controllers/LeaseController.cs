using System.Security.Claims;
using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaseController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public LeaseController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Lease.LeaseCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        User.FindFirst("sub")?.Value;

        var now = DateTimeOffset.UtcNow;

        var entity = new Lease
        {
            LeaseItem = dto.LeaseItem,
            Lessor = dto.Lessor,
            LeaseStart = dto.LeaseStart,
            LeaseEnd = dto.LeaseEnd,
            LeaseCost = dto.LeaseCost,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = dto.RequestedBy,
            RequestedAt = now,
            Status = Lease.LeaseAssetStatus.Pending
        };

        _context.Leases.Add(entity);
        await _context.SaveChangesAsync();

        var summary = $"{entity.LeaseItem} - {entity.Lessor}";
        await _emailService.NotifyAssetCreatedAsync("Lease", entity.Id, summary, userEmail);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.Leases
            .Where(l => l.Status == Lease.LeaseAssetStatus.Pending)
            .ToListAsync();

        return Ok(pending);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeUnapproved = false)
    {
        var isAdmin = User.IsInRole("admin");

        if (isAdmin && includeUnapproved)
        {
            var all = await _context.Leases.ToListAsync();
            return Ok(all);
        }

        var approved = new[]
        {
            Lease.LeaseAssetStatus.Approved,
            Lease.LeaseAssetStatus.Rejected
        };

        var result = await _context.Leases
            .Where(l => approved.Contains(l.Status))
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.Leases.FindAsync(id);
        if (entity == null) return NotFound();

        if (entity.Status != Lease.LeaseAssetStatus.Approved && !User.IsInRole("admin"))
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        User.FindFirst("sub")?.Value;

            if (!string.Equals(email, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                return Forbid();
        }

        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] Lease.LeaseApproveDto dto)
    {
        var entity = await _context.Leases.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve
            ? Lease.LeaseAssetStatus.Approved
            : Lease.LeaseAssetStatus.Rejected;

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var summary = $"{entity.LeaseItem} - {entity.Lessor}";
        await _emailService.NotifyAssetApprovalAsync("Lease", entity.Id, summary, entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Leases.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Leases.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
