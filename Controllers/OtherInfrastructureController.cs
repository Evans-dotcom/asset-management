using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.OtherInfrastructure;
using AssetStatus = Asset_management.models.OtherInfrastructure.AssetStatus;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OtherInfrastructureController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public OtherInfrastructureController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OtherInfrastructureCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        var entity = new OtherInfrastructure
        {
            AssetName = dto.AssetName,
            Location = dto.Location,
            Description = dto.Description,
            Value = dto.Value,
            AcquisitionDate = dto.AcquisitionDate,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = DateTimeOffset.UtcNow,
            Status = AssetStatus.Pending
        };

        _context.OtherInfrastructures.Add(entity);
        await _context.SaveChangesAsync();

        await _emailService.NotifyAssetCreatedAsync("OtherInfrastructure", entity.Id, $"{entity.AssetName} - {entity.Value}", userEmail);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.OtherInfrastructures
            .Where(a => a.Status == AssetStatus.Pending)
            .ToListAsync();

        return Ok(pending);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includePending = false)
    {
        var isAdmin = User.IsInRole("admin");

        if (isAdmin && includePending)
            return Ok(await _context.OtherInfrastructures.ToListAsync());

        var result = await _context.OtherInfrastructures
            .Where(a => a.Status != AssetStatus.Pending)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.OtherInfrastructures.FindAsync(id);
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
    public async Task<IActionResult> Approve(int id, [FromBody] OtherInfrastructureApproveDto dto)
    {
        var entity = await _context.OtherInfrastructures.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

        await _context.SaveChangesAsync();

        await _emailService.NotifyAssetApprovalAsync("OtherInfrastructure", entity.Id, $"{entity.AssetName} - {entity.Value}", entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.OtherInfrastructures.FindAsync(id);
        if (entity == null) return NotFound();

        _context.OtherInfrastructures.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
