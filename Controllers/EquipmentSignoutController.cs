using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.EquipmentSignout;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EquipmentSignoutController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public EquipmentSignoutController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EquipmentSignoutCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        var now = DateTimeOffset.UtcNow;

        var entity = new EquipmentSignout
        {
            EquipmentId = dto.EquipmentId,
            IssuedTo = dto.IssuedTo,
            DateIssued = dto.DateIssued ?? now,
            ExpectedReturnDate = dto.ExpectedReturnDate ?? now,
            ActualReturnDate = dto.ActualReturnDate ?? now,
            ConditionOnReturn = dto.ConditionOnReturn,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = now,
            ApprovedBy = null,
            ApprovalDate = null,
            ApprovalRemarks = null
        };

        _context.EquipmentSignouts.Add(entity);
        await _context.SaveChangesAsync();

        var summary = $"Equipment ID: {entity.EquipmentId} issued to {entity.IssuedTo}";
        await _emailService.NotifyAssetCreatedAsync("EquipmentSignout", entity.Id, summary, userEmail);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.EquipmentSignouts
            .Where(e => e.ApprovedBy == null)
            .ToListAsync();

        return Ok(pending);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var isAdmin = User.IsInRole("admin");

        if (isAdmin)
            return Ok(await _context.EquipmentSignouts.ToListAsync());

        // Normal users only see their own requests
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        var list = await _context.EquipmentSignouts
            .Where(e => e.RequestedBy == userEmail)
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.EquipmentSignouts.FindAsync(id);
        if (entity == null) return NotFound();

        var isAdmin = User.IsInRole("admin");
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        if (!isAdmin && !string.Equals(userEmail, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
            return Forbid();

        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] EquipmentSignoutApproveDto dto)
    {
        var entity = await _context.EquipmentSignouts.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var summary = $"Equipment ID: {entity.EquipmentId} issued to {entity.IssuedTo}";
        await _emailService.NotifyAssetApprovalAsync(
            "EquipmentSignout",
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
        var entity = await _context.EquipmentSignouts.FindAsync(id);
        if (entity == null) return NotFound();

        _context.EquipmentSignouts.Remove(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
