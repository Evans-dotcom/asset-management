using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LossesRegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LossesRegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(LossesRegister asset)
        {
            try
            {
                _context.LossesRegisters.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LossesRegister asset)
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
                if (!_context.LossesRegisters.Any(a => a.Id == id))
                    return NotFound($"Loss record with ID {id} does not exist.");
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
                var asset = await _context.LossesRegisters.FindAsync(id);
                if (asset == null)
                    return NotFound($"Loss record with ID {id} not found.");

                _context.LossesRegisters.Remove(asset);
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
                var asset = await _context.LossesRegisters.FindAsync(id);
                if (asset == null)
                    return NotFound($"Loss record with ID {id} not found.");

                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
