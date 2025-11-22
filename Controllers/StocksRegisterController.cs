using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.StocksRegister;
using AssetStatus = Asset_management.models.StocksRegister.AssetStatus;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StocksRegisterController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StocksRegisterController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var assets = await _context.StocksRegisters.ToListAsync();
        return Ok(assets);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var asset = await _context.StocksRegisters.FindAsync(id);
        if (asset == null) return NotFound();
        return Ok(asset);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StocksRegisterCreateDto dto)
    {
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        var asset = new StocksRegister
        {
            ItemName = dto.ItemName,
            Unit = dto.Unit,
            Quantity = dto.Quantity,
            UnitCost = dto.UnitCost,
            TotalValue = dto.TotalValue,
            Location = dto.Location,
            LastRestocked = dto.LastRestocked,
            Remarks = dto.Remarks,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = DateTimeOffset.UtcNow,
            Status = AssetStatus.Pending
        };

        _context.StocksRegisters.Add(asset);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] StocksRegisterApproveDto dto)
    {
        var asset = await _context.StocksRegisters.FindAsync(id);
        if (asset == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        asset.ApprovedBy = adminEmail;
        asset.ApprovalDate = DateTimeOffset.UtcNow;
        asset.ApprovalRemarks = dto.Remarks;
        asset.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, StocksRegister updatedAsset)
    {
        if (id != updatedAsset.Id) return BadRequest("ID mismatch");

        _context.Entry(updatedAsset).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var asset = await _context.StocksRegisters.FindAsync(id);
        if (asset == null) return NotFound();

        _context.StocksRegisters.Remove(asset);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
