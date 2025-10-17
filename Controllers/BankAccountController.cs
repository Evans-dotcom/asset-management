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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BankAccount asset)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
                var userRole = User.FindFirst("role")?.Value;

                _context.BankAccounts.Add(asset);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
           

            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        // [Authorize(Roles = "admin,user")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, [FromBody] BankAccount asset)
        {
            if (id != asset.Id)
                return BadRequest();

            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.BankAccounts.FindAsync(id);
            if (asset == null) return NotFound();

            _context.BankAccounts.Remove(asset);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _context.BankAccounts.FindAsync(id);
            if (asset == null) return NotFound();
            return Ok(asset);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bankAccounts = await _context.BankAccounts.ToListAsync();
            return Ok(bankAccounts);
        }

        [HttpGet("me")]
        [AllowAnonymous]
        public IActionResult GetMyUserInfo()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var role = User.FindFirst("role")?.Value;

            return Ok(new { Email = email, Role = role });
        }
    }
}
