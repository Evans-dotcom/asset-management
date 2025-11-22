using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.MajorMaintenance;
using AssetStatus = Asset_management.models.MajorMaintenance.AssetStatus;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MajorMaintenanceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public MajorMaintenanceController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MajorMaintenanceCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        var entity = new MajorMaintenance
        {
            AssetId = dto.AssetId,
            MaintenanceType = dto.MaintenanceType,
            DateStarted = dto.DateStarted,
            DateCompleted = dto.DateCompleted,
            Cost = dto.Cost,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = DateTimeOffset.UtcNow,
            Status = AssetStatus.Pending
        };

        _context.MajorMaintenances.Add(entity);
        await _context.SaveChangesAsync();

        await _emailService.NotifyAssetCreatedAsync("MajorMaintenance", entity.Id, $"{entity.MaintenanceType} - {entity.Cost}", userEmail);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.MajorMaintenances
            .Where(a => a.Status == AssetStatus.Pending)
            .ToListAsync();

        return Ok(pending);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includePending = false)
    {
        var isAdmin = User.IsInRole("admin");

        if (isAdmin && includePending)
            return Ok(await _context.MajorMaintenances.ToListAsync());

        var result = await _context.MajorMaintenances
            .Where(a => a.Status != AssetStatus.Pending)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.MajorMaintenances.FindAsync(id);
        if (entity == null) return NotFound();

        if (entity.Status == AssetStatus.Pending && !User.IsInRole("admin"))
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!string.Equals(userEmail, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                return Forbid();
        }

        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] MajorMaintenanceApproveDto dto)
    {
        var entity = await _context.MajorMaintenances.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

        await _context.SaveChangesAsync();

        await _emailService.NotifyAssetApprovalAsync("MajorMaintenance", entity.Id, $"{entity.MaintenanceType} - {entity.Cost}", entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.MajorMaintenances.FindAsync(id);
        if (entity == null) return NotFound();

        _context.MajorMaintenances.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
