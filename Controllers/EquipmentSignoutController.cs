using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentSignoutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentSignoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(EquipmentSignout asset)
        {
            try
            {
                _context.EquipmentSignouts.Add(asset);
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
        public async Task<IActionResult> Update(int id, EquipmentSignout asset)
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
                if (!_context.EquipmentSignouts.Any(a => a.Id == id))
                    return NotFound($"Equipment signout with ID {id} not found.");
                else
                    throw;
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.EquipmentSignouts.FindAsync(id);
            if (asset == null) return NotFound($"Equipment signout with ID {id} not found.");

            _context.EquipmentSignouts.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.EquipmentSignouts.FindAsync(id);
            if (asset == null) return NotFound($"Equipment signout with ID {id} not found.");
            return Ok(asset);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _context.EquipmentSignouts.ToListAsync();
            return Ok(assets);
        }
    }
}
