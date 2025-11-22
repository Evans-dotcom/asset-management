using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.RoadsInfrastructure;
using AssetStatus = Asset_management.models.RoadsInfrastructure.RDAssetStatus;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoadsInfrastructureController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoadsInfrastructureController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoadsInfrastructureCreateDto dto)
    {
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        var entity = new RoadsInfrastructure
        {
            RoadName = dto.RoadName,
            Location = dto.Location,
            LengthKm = dto.LengthKm,
            ConstructionCost = dto.ConstructionCost,
            YearConstructed = dto.YearConstructed,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = DateTimeOffset.UtcNow,
            Status = AssetStatus.Pending
        };

        _context.RoadsInfrastructures.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.RoadsInfrastructures.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.RoadsInfrastructures.FindAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] RoadsInfrastructureApproveDto dto)
    {
        var entity = await _context.RoadsInfrastructures.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? RDAssetStatus.Approved : RDAssetStatus.Rejected;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RoadsInfrastructure entity)
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
            if (!_context.RoadsInfrastructures.Any(a => a.Id == id)) return NotFound();
            else throw;
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.RoadsInfrastructures.FindAsync(id);
        if (entity == null) return NotFound();

        _context.RoadsInfrastructures.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
