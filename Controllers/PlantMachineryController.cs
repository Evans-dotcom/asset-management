using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlantMachineryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlantMachineryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(PlantMachinery asset)
        {
            try
            {
                _context.PlantMachineries.Add(asset);
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
        public async Task<IActionResult> Update(int id, PlantMachinery asset)
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
                if (!_context.PlantMachineries.Any(a => a.Id == id))
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
                var asset = await _context.PlantMachineries.FindAsync(id);
                if (asset == null)
                    return NotFound($"Asset with ID {id} not found.");

                _context.PlantMachineries.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete asset: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.PlantMachineries.FindAsync(id);
                if (asset == null)
                    return NotFound($"Asset with ID {id} not found.");
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving asset: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var assets = await _context.PlantMachineries.ToListAsync();
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving assets: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
