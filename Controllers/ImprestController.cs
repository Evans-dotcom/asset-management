using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.Imprest;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ImprestController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public ImprestController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Imprest.ImprestCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst("sub")?.Value;

        var now = DateTimeOffset.UtcNow;

        var entity = new Imprest
        {
            Officer = dto.Officer,
            Amount = dto.Amount,
            DateIssued = dto.DateIssued,
            Purpose = dto.Purpose,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = now
        };

        _context.Imprests.Add(entity);
        await _context.SaveChangesAsync();

        var summary = $"{entity.Officer} - {entity.Amount}";
        await _emailService.NotifyAssetCreatedAsync("Imprest", entity.Id, summary, userEmail);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.Imprests
            .Where(i => i.Status == AssetStatus.Pending)
            .ToListAsync();

        return Ok(pending);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includePending = false)
    {
        var isAdmin = User.IsInRole("admin");

        if (isAdmin && includePending)
            return Ok(await _context.Imprests.ToListAsync());

        var list = await _context.Imprests
            .Where(i => i.Status != AssetStatus.Pending)
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.Imprests.FindAsync(id);
        if (entity == null) return NotFound();

        var isAdmin = User.IsInRole("admin");
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst("sub")?.Value;

        if (!isAdmin && !string.Equals(entity.RequestedBy, userEmail, StringComparison.OrdinalIgnoreCase))
            return Forbid();

        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] Imprest.ImprestApproveDto dto)
    {
        var entity = await _context.Imprests.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var summary = $"{entity.Officer} - {entity.Amount}";
        await _emailService.NotifyAssetApprovalAsync(
            "Imprest",
            entity.Id,
            summary,
            entity.RequestedBy,
            dto.Approve,
            dto.Remarks,
            adminEmail
        );

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Imprests.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Imprests.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
