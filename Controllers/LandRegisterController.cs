using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LandRegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LandRegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] LandRegister asset)
        {
            try
            {
                _context.LandRegisters.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create land register: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, [FromBody] LandRegister asset)
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
                if (!_context.LandRegisters.Any(a => a.Id == id))
                    return NotFound($"Land register with ID {id} not found.");
                else
                    return Conflict("A concurrency conflict occurred. Please retry.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update land register: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.LandRegisters.FindAsync(id);
                if (asset == null)
                    return NotFound($"Land register with ID {id} not found.");

                _context.LandRegisters.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete land register: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _context.LandRegisters.ToListAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve records: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.LandRegisters.FindAsync(id);
                if (asset == null)
                    return NotFound($"Land register with ID {id} not found.");

                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving land register: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
