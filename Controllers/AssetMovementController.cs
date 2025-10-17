using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssetMovementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssetMovementController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssetMovement asset)
        {
            try
            {
                _context.AssetMovements.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create asset movement: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AssetMovement asset)
        {
            if (id != asset.Id)
                return BadRequest("ID mismatch.");

            _context.Entry(asset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.AssetMovements.Any(a => a.Id == id))
                    return NotFound($"Asset movement with ID {id} not found.");
                else
                    return Conflict("A concurrency error occurred. Please try again.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update asset movement: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.AssetMovements.FindAsync(id);
                if (asset == null)
                    return NotFound($"Asset movement with ID {id} not found.");

                _context.AssetMovements.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete asset movement: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.AssetMovements.FindAsync(id);
                if (asset == null)
                    return NotFound($"Asset movement with ID {id} not found.");

                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving asset movement: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.AssetMovements.ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving asset movements: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
