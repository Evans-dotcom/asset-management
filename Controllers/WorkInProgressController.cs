using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkInProgressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkInProgressController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(WorkInProgress asset)
        {
            try
            {
                _context.WorkInProgresses.Add(asset);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, WorkInProgress asset)
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
                if (!_context.WorkInProgresses.Any(a => a.Id == id))
                    return NotFound($"WorkInProgress with ID {id} not found.");
                else
                    return Conflict("A concurrency conflict occurred. Please try again.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.WorkInProgresses.FindAsync(id);
                if (asset == null)
                    return NotFound($"WorkInProgress with ID {id} not found.");

                _context.WorkInProgresses.Remove(asset);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var asset = await _context.WorkInProgresses.FindAsync(id);
                if (asset == null)
                    return NotFound($"WorkInProgress with ID {id} not found.");

                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving record: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _context.WorkInProgresses.ToListAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving records: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
