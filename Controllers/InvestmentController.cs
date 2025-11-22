using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.Investments;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InvestmentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public InvestmentController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InvestmentsCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        var now = DateTimeOffset.UtcNow;

        var entity = new Investments
        {
            InvestmentType = dto.InvestmentType,
            Institution = dto.Institution,
            DateInvested = dto.DateInvested,
            Amount = dto.Amount,
            ExpectedReturn = dto.ExpectedReturn,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = now,
            Status = AssetStatus.Pending
        };

        _context.Investments.Add(entity);
        await _context.SaveChangesAsync();

        var summary = $"{entity.InvestmentType} - {entity.Institution}";
        await _emailService.NotifyAssetCreatedAsync("Investment", entity.Id, summary, userEmail);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.Investments
            .Where(a => a.Status == AssetStatus.Pending)
            .ToListAsync();
        return Ok(pending);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includePending = false)
    {
        var isAdmin = User.IsInRole("admin");

        if (isAdmin && includePending)
            return Ok(await _context.Investments.ToListAsync());

        var result = await _context.Investments
            .Where(a => a.Status != AssetStatus.Pending)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.Investments.FindAsync(id);
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
    public async Task<IActionResult> Approve(int id, [FromBody] InvestmentsApproveDto dto)
    {
        var entity = await _context.Investments.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

        await _context.SaveChangesAsync();

        var summary = $"{entity.InvestmentType} - {entity.Institution}";
        await _emailService.NotifyAssetApprovalAsync("Investment", entity.Id, summary, entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Investments.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Investments.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
