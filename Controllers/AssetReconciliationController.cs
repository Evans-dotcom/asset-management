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
using static Asset_management.models.AssetReconciliation;

namespace Asset_management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssetReconciliationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AssetReconciliationController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssetReconciliationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var now = DateTimeOffset.UtcNow;

            var entity = new AssetReconciliation
            {
                AssetId = dto.AssetId,
                DateReconciled = dto.DateReconciled ?? now,
                PhysicalCount = dto.PhysicalCount,
                SystemCount = dto.SystemCount,
                ReconciledBy = dto.ReconciledBy,
                Discrepancy = dto.Discrepancy,
                Remarks = dto.Remarks,
                Department = dto.Department,
                DepartmentUnit = dto.DepartmentUnit,
                ContractDate = dto.ContractDate ?? now,
                RequestedBy = userEmail,
                RequestedAt = now
            };

            _context.AssetReconciliations.Add(entity);
            await _context.SaveChangesAsync();

            await _emailService.NotifyAssetCreatedAsync("AssetReconciliation", entity.Id, $"Asset {entity.AssetId} reconciled by {entity.ReconciledBy}", userEmail);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _context.AssetReconciliations
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
                var all = await _context.AssetReconciliations.ToListAsync();
                return Ok(all);
            }

            var result = await _context.AssetReconciliations
                .Where(a => a.ApprovedBy != null)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.AssetReconciliations.FindAsync(id);
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
        public async Task<IActionResult> Approve(int id, [FromBody] AssetReconciliationApproveDto dto)
        {
            var entity = await _context.AssetReconciliations.FindAsync(id);
            if (entity == null) return NotFound();

            var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            entity.ApprovedBy = adminEmail;
            entity.ApprovalDate = DateTimeOffset.UtcNow;
            entity.ApprovalRemarks = dto.Remarks;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _emailService.NotifyAssetApprovalAsync("AssetReconciliation", entity.Id, $"Asset {entity.AssetId} reconciled by {entity.ReconciledBy}", entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.AssetReconciliations.FindAsync(id);
            if (entity == null) return NotFound();

            _context.AssetReconciliations.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
