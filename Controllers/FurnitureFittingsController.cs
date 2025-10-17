using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asset_management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FurnitureFittingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FurnitureFittingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(FurnitureFitting asset)
        {
            try
            {
                _context.FurnitureFittings.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating furniture fitting: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FurnitureFitting asset)
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
                if (!_context.FurnitureFittings.Any(a => a.Id == id))
                    return NotFound($"Furniture fitting with ID {id} not found.");
                else
                    return Conflict("A concurrency error occurred while updating.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Update failed: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.FurnitureFittings.FindAsync(id);
                if (asset == null) return NotFound($"Furniture fitting with ID {id} not found.");

                _context.FurnitureFittings.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Deletion failed: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.FurnitureFittings.FindAsync(id);
                if (asset == null)
                    return NotFound($"Furniture fitting with ID {id} not found.");
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // Optional: Get all furniture fittings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _context.FurnitureFittings.ToListAsync();
            return Ok(assets);
        }
    }
}
