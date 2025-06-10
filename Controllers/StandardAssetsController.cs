using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StandardAssetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StandardAssetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StandardAssets
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? ward)
        {
            var query = _context.StandardAssets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(a => a.AssetCondition == category);

            if (!string.IsNullOrWhiteSpace(ward))
                query = query.Where(a => a.Location == ward);

            var assets = await query.ToListAsync();
            return Ok(assets);
        }

        // GET: api/StandardAssets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.StandardAssets.FindAsync(id);
            if (asset == null)
                return NotFound();

            return Ok(asset);
        }

        // POST: api/StandardAssets
        [HttpPost]
        public async Task<IActionResult> Create(StandardAsset asset)
        {
            _context.StandardAssets.Add(asset);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        // PUT: api/StandardAssets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StandardAsset updatedAsset)
        {
            if (id != updatedAsset.Id)
                return BadRequest("ID mismatch");

            _context.Entry(updatedAsset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.StandardAssets.Any(a => a.Id == id))
                    return NotFound();

                throw;
            }
        }
        // DELETE: api/StandardAssets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.StandardAssets.FindAsync(id);
            if (asset == null)
                return NotFound();

            _context.StandardAssets.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
