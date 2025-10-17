using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvestmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _context.Investments.ToListAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve investments: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.Investments.FindAsync(id);
                if (asset == null)
                    return NotFound($"Investment with ID {id} not found.");
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving investment: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Investments asset)
        {
            try
            {
                _context.Investments.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create investment: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, Investments asset)
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
                if (!_context.Investments.Any(a => a.Id == id))
                    return NotFound($"Investment with ID {id} not found.");
                else
                    return Conflict("A concurrency error occurred. Please retry.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update investment: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.Investments.FindAsync(id);
                if (asset == null)
                    return NotFound($"Investment with ID {id} not found.");

                _context.Investments.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete investment: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
