using Asset_management.DTOs;
using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.IntangibleAsset;
using AssetStatus = Asset_management.models.IntangibleAsset.AssetStatus;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IntangibleAssetController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public IntangibleAssetController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IntangibleAssetCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var entity = new IntangibleAsset
        {
            AssetType = dto.AssetType,
            Description = dto.Description,
            Value = dto.Value,
            UsefulLifeYears = dto.UsefulLifeYears,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = DateTimeOffset.UtcNow,
            Status = AssetStatus.Pending
        };

        _context.IntangibleAssets.Add(entity);
        await _context.SaveChangesAsync();

        await _emailService.NotifyAssetCreatedAsync(
            "IntangibleAsset", entity.Id, $"{entity.AssetType} - {entity.Value}", userEmail
        );

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        return Ok(await _context.IntangibleAssets
            .Where(a => a.Status == AssetStatus.Pending)
            .ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includePending = false)
    {
        if (User.IsInRole("admin") && includePending)
            return Ok(await _context.IntangibleAssets.ToListAsync());

        return Ok(await _context.IntangibleAssets
            .Where(a => a.Status != AssetStatus.Pending)
            .ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.IntangibleAssets.FindAsync(id);
        if (entity == null) return NotFound();

        if (entity.Status == AssetStatus.Pending && !User.IsInRole("admin"))
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.Equals(email, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                return Forbid();
        }

        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] IntangibleAssetApproveDto dto)
    {
        var entity = await _context.IntangibleAssets.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

        await _context.SaveChangesAsync();

        await _emailService.NotifyAssetApprovalAsync(
            "IntangibleAsset",
            id,
            $"{entity.AssetType} - {entity.Value}",
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
        var entity = await _context.IntangibleAssets.FindAsync(id);
        if (entity == null) return NotFound();

        _context.IntangibleAssets.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
