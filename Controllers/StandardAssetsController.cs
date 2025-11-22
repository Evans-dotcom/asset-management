using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.StandardAsset;
using AssetStatus = Asset_management.models.StandardAsset.AssetStatus;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StandardAssetsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StandardAssetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? location)
    {
        var query = _context.StandardAssets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(a => a.AssetCondition == category);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(a => a.Location == location);

        var assets = await query.ToListAsync();
        return Ok(assets);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var asset = await _context.StandardAssets.FindAsync(id);
        if (asset == null) return NotFound();
        return Ok(asset);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StandardAssetCreateDto dto)
    {
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        var asset = new StandardAsset
        {
            AssetDescription = dto.AssetDescription,
            SerialNumber = dto.SerialNumber,
            MakeModel = dto.MakeModel,
            TagNumber = dto.TagNumber,
            DeliveryDate = dto.DeliveryDate,
            ContractDate = dto.ContractDate,
            PvNumber = dto.PvNumber,
            PurchaseAmount = dto.PurchaseAmount,
            DepreciationRate = dto.DepreciationRate,
            AnnualDepreciation = dto.AnnualDepreciation,
            AccumulatedDepreciation = dto.AccumulatedDepreciation,
            NetBookValue = dto.NetBookValue,
            ResponsibleOfficer = dto.ResponsibleOfficer,
            Location = dto.Location,
            AssetCondition = dto.AssetCondition,
            Notes = dto.Notes,
            Department = dto.Department,
            DepartmentUnit = dto.DepartmentUnit,
            RequestedBy = userEmail,
            RequestedAt = DateTimeOffset.UtcNow,
            Status = AssetStatus.Pending
        };

        _context.StandardAssets.Add(asset);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] StandardAssetApproveDto dto)
    {
        var asset = await _context.StandardAssets.FindAsync(id);
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
    public async Task<IActionResult> Update(int id, StandardAsset updatedAsset)
    {
        if (id != updatedAsset.Id) return BadRequest("ID mismatch");

        _context.Entry(updatedAsset).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.StandardAssets.Any(a => a.Id == id)) return NotFound();
            throw;
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var asset = await _context.StandardAssets.FindAsync(id);
        if (asset == null) return NotFound();

        _context.StandardAssets.Remove(asset);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
