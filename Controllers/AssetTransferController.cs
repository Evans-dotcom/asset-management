using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssetTransferController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssetTransferController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssetTransfer asset)
        {
            if (asset == null)
                return BadRequest("Asset payload is required.");

            try
            {
                _context.AssetTransfers.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving asset transfer: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AssetTransfer asset)
        {
            if (id != asset.Id)
                return BadRequest("ID in route does not match asset ID.");

            var existing = await _context.AssetTransfers.FindAsync(id);
            if (existing == null)
                return NotFound($"Asset transfer with ID {id} not found.");

            try
            {
                _context.Entry(asset).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Concurrency error while updating the asset transfer.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.AssetTransfers.FindAsync(id);
            if (asset == null)
                return NotFound($"Asset transfer with ID {id} not found.");

            try
            {
                _context.AssetTransfers.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting asset transfer: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.AssetTransfers.FindAsync(id);
            if (asset == null)
                return NotFound($"Asset transfer with ID {id} not found.");

            return Ok(asset);
        }
    }
}
