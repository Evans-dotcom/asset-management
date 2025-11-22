using System.Security.Claims;
using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LandRegisterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public LandRegisterController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LandRegister.LandRegisterCreateDto dto)
    {
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        var now = DateTimeOffset.UtcNow;

        var entity = new LandRegister
        {
            ParcelNumber = dto.ParcelNumber,
            Location = dto.Location,
            Acreage = dto.Acreage,
            TitleDeedNumber = dto.TitleDeedNumber,
            DateAcquired = dto.DateAcquired,
            OwnershipStatus = dto.OwnershipStatus,
            LandValue = dto.LandValue,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = now,
            Status = LandRegister.LandRegisterStatus.Pending
        };

        _context.LandRegisters.Add(entity);
        await _context.SaveChangesAsync();

        var summary = $"{entity.ParcelNumber} - {entity.Location}";
        await _emailService.NotifyAssetCreatedAsync("LandRegister", entity.Id, summary, userEmail);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.LandRegisters
            .Where(a => a.Status == LandRegister.LandRegisterStatus.Pending)
            .ToListAsync();
        return Ok(pending);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeUnapproved = false)
    {
        var isAdmin = User.IsInRole("admin");

        if (isAdmin && includeUnapproved)
        {
            var all = await _context.LandRegisters.ToListAsync();
            return Ok(all);
        }

        var allowedStatuses = new[]
        {
            LandRegister.LandRegisterStatus.Approved,
            LandRegister.LandRegisterStatus.Rejected
        };

        var list = await _context.LandRegisters
            .Where(a => allowedStatuses.Contains(a.Status))
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.LandRegisters.FindAsync(id);
        if (entity == null) return NotFound();

        if (entity.Status != LandRegister.LandRegisterStatus.Approved && !User.IsInRole("admin"))
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!string.Equals(email, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                return Forbid();
        }

        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] LandRegister.LandRegisterApproveDto dto)
    {
        var entity = await _context.LandRegisters.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? LandRegister.LandRegisterStatus.Approved : LandRegister.LandRegisterStatus.Rejected;

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var summary = $"{entity.ParcelNumber} - {entity.Location}";
        await _emailService.NotifyAssetApprovalAsync("LandRegister", entity.Id, summary, entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.LandRegisters.FindAsync(id);
        if (entity == null) return NotFound();

        _context.LandRegisters.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
