using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsPayableController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsPayableController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(AccountsPayable asset)
        {
            try
            {
                _context.AccountsPayables.Add(asset);
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
        public async Task<IActionResult> Update(int id, AccountsPayable asset)
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
                if (!_context.AccountsPayables.Any(a => a.Id == id))
                    return NotFound($"Record with ID {id} does not exist.");
                else
                    return Conflict("Concurrency error occurred. Please retry.");
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
                var asset = await _context.AccountsPayables.FindAsync(id);
                if (asset == null)
                    return NotFound($"Record with ID {id} not found.");

                _context.AccountsPayables.Remove(asset);
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
                var asset = await _context.AccountsPayables.FindAsync(id);
                if (asset == null)
                    return NotFound($"Record with ID {id} not found.");

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
                var list = await _context.AccountsPayables.ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving records: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
