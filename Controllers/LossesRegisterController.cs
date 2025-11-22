using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.LossesRegister;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LossesRegisterController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LossesRegisterController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LossesRegisterCreateDto dto)
    {
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        var entity = new LossesRegister
        {
            AssetId = dto.AssetId,
            LossDate = dto.LossDate,
            Cause = dto.Cause,
            ReportedBy = dto.ReportedBy,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = DateTimeOffset.UtcNow,
            Status = AssetStatus.Pending
        };

        _context.LossesRegisters.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.LossesRegisters.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.LossesRegisters.FindAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] LossesRegisterApproveDto dto)
    {
        var entity = await _context.LossesRegisters.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LossesRegister entity)
    {
        if (id != entity.Id) return BadRequest("ID mismatch");

        _context.Entry(entity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.LossesRegisters.Any(a => a.Id == id)) return NotFound();
            else throw;
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.LossesRegisters.FindAsync(id);
        if (entity == null) return NotFound();

        _context.LossesRegisters.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
