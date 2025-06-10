using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public BankAccountController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: api/BankAccount
        [HttpPost]
        [Authorize(Roles = "admin,user")] 
        public async Task<IActionResult> Create([FromBody] BankAccount asset)
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var userRole = User.FindFirst("role")?.Value;

            // Optionally attach created-by info, etc.
            _context.BankAccounts.Add(asset);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        // PUT: api/BankAccount/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> Update(int id, [FromBody] BankAccount asset)
        {
            if (id != asset.Id)
                return BadRequest();

            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/BankAccount/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.BankAccounts.FindAsync(id);
            if (asset == null) return NotFound();

            _context.BankAccounts.Remove(asset);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/BankAccount/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.BankAccounts.FindAsync(id);
            if (asset == null) return NotFound();
            return Ok(asset);
        }

        // GET: api/BankAccount
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bankAccounts = await _context.BankAccounts.ToListAsync();
            return Ok(bankAccounts);
        }

        // GET: api/BankAccount/me
        [HttpGet("me")]
        public IActionResult GetMyUserInfo()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var role = User.FindFirst("role")?.Value;

            return Ok(new { Email = email, Role = role });
        }
    }
}
