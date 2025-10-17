using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsRegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BuildingsRegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(BuildingsRegister asset)
        {
            if (asset == null)
                return BadRequest("Asset payload is required.");

            try
            {
                _context.BuildingsRegisters.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving asset: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, BuildingsRegister asset)
        {
            if (id != asset.Id)
                return BadRequest("ID mismatch between route and payload.");

            var existing = await _context.BuildingsRegisters.FindAsync(id);
            if (existing == null)
                return NotFound($"Building asset with ID {id} not found.");

            try
            {
                _context.Entry(asset).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Concurrency error while updating the asset.");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.BuildingsRegisters.FindAsync(id);
            if (asset == null)
                return NotFound($"Building asset with ID {id} not found.");

            try
            {
                _context.BuildingsRegisters.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting asset: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.BuildingsRegisters.FindAsync(id);
            if (asset == null)
                return NotFound($"Building asset with ID {id} not found.");

            return Ok(asset);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var assets = await _context.BuildingsRegisters.ToListAsync();
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving building assets: {ex.Message}");
            }
        }
    }
}
