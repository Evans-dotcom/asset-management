using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BiologicalAssetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BiologicalAssetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(BiologicalAsset asset)
        {
            if (asset == null)
                return BadRequest("Asset payload is required.");

            try
            {
                _context.BiologicalAssets.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving biological asset: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, BiologicalAsset asset)
        {
            if (id != asset.Id)
                return BadRequest("ID mismatch between route and payload.");

            var existing = await _context.BiologicalAssets.FindAsync(id);
            if (existing == null)
                return NotFound($"Biological asset with ID {id} not found.");

            try
            {
                _context.Entry(asset).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Concurrency error while updating the biological asset.");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.BiologicalAssets.FindAsync(id);
            if (asset == null)
                return NotFound($"Biological asset with ID {id} not found.");

            try
            {
                _context.BiologicalAssets.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting biological asset: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.BiologicalAssets.FindAsync(id);
            if (asset == null)
                return NotFound($"Biological asset with ID {id} not found.");

            return Ok(asset);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var assets = await _context.BiologicalAssets.ToListAsync();
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving biological assets: {ex.Message}");
            }
        }
    }
}
