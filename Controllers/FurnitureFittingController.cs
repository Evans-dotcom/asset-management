using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FurnitureFittingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FurnitureFittingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
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
                return BadRequest($"Error creating asset: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
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
                    throw;
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.FurnitureFittings.FindAsync(id);
            if (asset == null) return NotFound($"Furniture fitting with ID {id} not found.");

            _context.FurnitureFittings.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.FurnitureFittings.FindAsync(id);
            if (asset == null) return NotFound($"Furniture fitting with ID {id} not found.");
            return Ok(asset);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _context.FurnitureFittings.ToListAsync();
            return Ok(assets);
        }
    }
}
