
using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ImprestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImprestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Imprest asset)
        {
            _context.Imprests.Add(asset);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Imprest asset)
        {
            if (id != asset.Id)
                return BadRequest();

            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.Imprests.FindAsync(id);
            if (asset == null) return NotFound();

            _context.Imprests.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.Imprests.FindAsync(id);
            if (asset == null) return NotFound();
            return Ok(asset);
        }
    }
}
