using Asset_management.DTOs;
using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Asset_management.models.AccountsPayable;

namespace Asset_management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsPayableController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountsPayableController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountsPayableCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var now = DateTimeOffset.UtcNow;

            var entity = new AccountsPayable
            {
                CreditorName = dto.CreditorName,
                AmountDue = dto.AmountDue,
                DueDate = dto.DueDate,
                Reason = dto.Reason,
                Remarks = dto.Remarks,
                RequestedBy = userEmail,
                RequestedAt = now
            };

            _context.AccountsPayables.Add(entity);
            await _context.SaveChangesAsync();

            await _emailService.NotifyAssetCreatedAsync("AccountsPayable", entity.Id, $"{entity.CreditorName} - {entity.AmountDue}", userEmail);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _context.AccountsPayables
                .Where(a => a.ApprovedBy == null)
                .ToListAsync();
            return Ok(pending);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeUnapproved = false)
        {
            var isAdmin = User.IsInRole("admin");

            if (isAdmin && includeUnapproved)
            {
                var all = await _context.AccountsPayables.ToListAsync();
                return Ok(all);
            }

            var result = await _context.AccountsPayables
                .Where(a => a.ApprovedBy != null)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.AccountsPayables.FindAsync(id);
            if (entity == null) return NotFound();

            if (entity.ApprovedBy == null && !User.IsInRole("admin"))
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
                if (!string.Equals(userEmail, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                    return Forbid();
            }

            return Ok(entity);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] AccountsPayableApproveDto dto)
        {
            var entity = await _context.AccountsPayables.FindAsync(id);
            if (entity == null) return NotFound();

            var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            entity.ApprovedBy = adminEmail;
            entity.ApprovalDate = DateTimeOffset.UtcNow;
            entity.ApprovalRemarks = dto.Remarks;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _emailService.NotifyAssetApprovalAsync("AccountsPayable", entity.Id, $"{entity.CreditorName} - {entity.AmountDue}", entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.AccountsPayables.FindAsync(id);
            if (entity == null) return NotFound();

            _context.AccountsPayables.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
