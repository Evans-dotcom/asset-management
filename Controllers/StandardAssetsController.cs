using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
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
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? ward)
        {
            try
            {
                var query = _context.StandardAssets.AsQueryable();

                if (!string.IsNullOrWhiteSpace(category))
                    query = query.Where(a => a.AssetCondition == category);

                if (!string.IsNullOrWhiteSpace(ward))
                    query = query.Where(a => a.Location == ward);

                var assets = await query.ToListAsync();
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving assets: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.StandardAssets.FindAsync(id);
                if (asset == null)
                    return NotFound($"Asset with ID {id} not found.");

                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving asset: {ex.Message}");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(StandardAsset asset)
        {
            try
            {
                _context.StandardAssets.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create asset: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, StandardAsset updatedAsset)
        {
            if (id != updatedAsset.Id)
                return BadRequest("ID mismatch.");

            _context.Entry(updatedAsset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.StandardAssets.Any(a => a.Id == id))
                    return NotFound($"Asset with ID {id} does not exist.");
                else
                    return Conflict("A concurrency error occurred. Please try again.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update asset: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.StandardAssets.FindAsync(id);
                if (asset == null)
                    return NotFound($"Asset with ID {id} not found.");

                _context.StandardAssets.Remove(asset);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete asset: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
