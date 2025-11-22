using Asset_management.DTOs;
using Asset_management.models;
using Asset_management.NewFolder;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;

        public BankAccountController(ApplicationDbContext context, TokenService tokenService, IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BankAccountCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst(ClaimTypes.Email)?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized("User email not found in token");

                var now = DateTimeOffset.UtcNow;

                var entity = new BankAccount
                {
                    BankName = dto.BankName,
                    AccountNumber = dto.AccountNumber,
                    AccountType = dto.AccountType,
                    OpeningBalance = dto.OpeningBalance,
                    CurrentBalance = dto.OpeningBalance,
                    Remarks = dto.Remarks,
                    Department = dto.Department,
                    DepartmentUnit = dto.DepartmentUnit,
                    AccountName = dto.AccountName,
                    ContractDate = dto.ContractDate ?? now,
                    OfficerInCharge = dto.OfficerInCharge,
                    Signatories = dto.Signatories,
                    Status = BankAccountStatus.Open,
                    RequestedBy = userEmail,
                    RequestedAt = now
                };

                _context.BankAccounts.Add(entity);
                await _context.SaveChangesAsync();

                var assetSummary = $"{entity.BankName} - {entity.AccountNumber} - {entity.AccountType}";
                await _emailService.NotifyAssetCreatedAsync("BankAccount", entity.Id, assetSummary, userEmail);

                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating bank account: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var isInRole = User.IsInRole("Admin");
            var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            Console.WriteLine($"Email: {userEmail}");
            Console.WriteLine($"Role: {userRole}");
            Console.WriteLine($"IsInRole(Admin): {isInRole}");
            Console.WriteLine($"All Claims: {string.Join(", ", allClaims.Select(c => $"{c.Type}={c.Value}"))}");

            if (string.IsNullOrEmpty(userRole) || userRole.ToLower() != "admin")
            {
                return StatusCode(403, new
                {
                    message = "Access denied",
                    role = userRole,
                    claims = allClaims
                });
            }

            var pending = await _context.BankAccounts
                .Where(b => b.Status == BankAccountStatus.Open)
                .ToListAsync();

            return Ok(pending);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeUnapproved = false)
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var isAdmin = !string.IsNullOrEmpty(userRole) && userRole.ToLower() == "admin";

                if (isAdmin && includeUnapproved)
                {
                    var all = await _context.BankAccounts.ToListAsync();
                    return Ok(all);
                }

                var visibleStatuses = new[]
                {
                    BankAccountStatus.Approved,
                    BankAccountStatus.Rejected
                };

                var result = await _context.BankAccounts
                    .Where(b => visibleStatuses.Contains(b.Status))
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving accounts: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entity = await _context.BankAccounts.FindAsync(id);
                if (entity == null)
                    return NotFound();

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var isAdmin = !string.IsNullOrEmpty(userRole) && userRole.ToLower() == "admin";

                if (entity.Status != BankAccountStatus.Approved && !isAdmin)
                {
                    var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(ClaimTypes.Email)?.Value;

                    if (!string.Equals(userEmail, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                        return Forbid();
                }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving account: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] BankAccountApproveDto dto)
        {
            try
            {
                var entity = await _context.BankAccounts.FindAsync(id);
                if (entity == null)
                    return NotFound();

                var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst(ClaimTypes.Email)?.Value;

                entity.ApprovedBy = adminEmail;
                entity.ApprovalDate = DateTimeOffset.UtcNow;
                entity.ApprovalRemarks = dto.Remarks;
                entity.Status = dto.Approve ? BankAccountStatus.Approved : BankAccountStatus.Rejected;

                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var assetSummary = $"{entity.BankName} - {entity.AccountNumber} - {entity.AccountType}";
                await _emailService.NotifyAssetApprovalAsync(
                    "BankAccount",
                    entity.Id,
                    assetSummary,
                    entity.RequestedBy,
                    dto.Approve,
                    dto.Remarks,
                    adminEmail
                );

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing approval: {ex.Message}");
            }
        }
        [HttpGet("rejected")]
        public async Task<IActionResult> GetRejected()
        {
            try
            {
                var rejected = await _context.BankAccounts
                    .Where(b => b.Status == BankAccountStatus.Rejected)
                    .ToListAsync();

                return Ok(rejected);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving rejected accounts: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _context.BankAccounts.FindAsync(id);
                if (entity == null)
                    return NotFound();

                _context.BankAccounts.Remove(entity);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting account: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new
            {
                Email = userEmail,
                Role = userRole,
                AllClaims = allClaims
            });
        }
    }
}
