using System.Security.Claims;
using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asset_management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BiologicalAssetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public BiologicalAssetController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BiologicalAsset.BiologicalAssetCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var now = DateTimeOffset.UtcNow;

            var entity = new BiologicalAsset
            {
                AssetType = dto.AssetType,
                Quantity = dto.Quantity,
                AcquisitionDate = dto.AcquisitionDate ?? now,
                Location = dto.Location,
                Value = dto.Value,
                Notes = dto.Notes,
                Department = dto.Department,
                DepartmentUnit = dto.DepartmentUnit,
                ContractDate = dto.ContractDate ?? now,
                RequestedBy = userEmail,
                RequestedAt = now
            };

            _context.BiologicalAssets.Add(entity);
            await _context.SaveChangesAsync();

            var summary = $"{entity.AssetType} - {entity.Quantity}";
            await _emailService.NotifyAssetCreatedAsync("BiologicalAsset", entity.Id, summary, userEmail);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _context.BiologicalAssets
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
                var all = await _context.BiologicalAssets.ToListAsync();
                return Ok(all);
            }

            var result = await _context.BiologicalAssets
                .Where(a => a.ApprovedBy != null)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.BiologicalAssets.FindAsync(id);
            if (entity == null) return NotFound();

            if (entity.ApprovedBy == null && !User.IsInRole("admin"))
            {
                var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
                if (!string.Equals(email, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                    return Forbid();
            }

            return Ok(entity);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] BiologicalAsset.BiologicalAssetApproveDto dto)
        {
            var entity = await _context.BiologicalAssets.FindAsync(id);
            if (entity == null) return NotFound();

            var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            entity.ApprovedBy = adminEmail;
            entity.ApprovalDate = DateTimeOffset.UtcNow;
            entity.ApprovalRemarks = dto.Remarks;

            if (!dto.Approve)
                entity.ApprovedBy = "REJECTED";

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var summary = $"{entity.AssetType} - {entity.Quantity}";
            await _emailService.NotifyAssetApprovalAsync("BiologicalAsset", entity.Id, summary, entity.RequestedBy, dto.Approve, dto.Remarks, adminEmail);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.BiologicalAssets.FindAsync(id);
            if (entity == null) return NotFound();

            _context.BiologicalAssets.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
