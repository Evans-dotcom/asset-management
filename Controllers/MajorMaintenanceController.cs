using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MajorMaintenanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MajorMaintenanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MajorMaintenance asset)
        {
            try
            {
                _context.MajorMaintenances.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MajorMaintenance asset)
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
                if (!_context.MajorMaintenances.Any(a => a.Id == id))
                    return NotFound($"Maintenance record with ID {id} does not exist.");
                else
                    return Conflict("A concurrency error occurred. Please retry.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.MajorMaintenances.FindAsync(id);
                if (asset == null)
                    return NotFound($"Maintenance record with ID {id} not found.");

                _context.MajorMaintenances.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.MajorMaintenances.FindAsync(id);
                if (asset == null)
                    return NotFound($"Maintenance record with ID {id} not found.");

                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
