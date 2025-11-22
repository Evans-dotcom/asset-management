using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Asset_management.models.FurnitureFitting;

namespace Asset_management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FurnitureFittingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public FurnitureFittingController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FurnitureFitting.FurnitureFittingCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var entity = new FurnitureFitting
            {
                ItemDescription = dto.ItemDescription,
                SerialNumber = dto.SerialNumber,
                Quantity = dto.Quantity,
                Location = dto.Location,
                Department = dto.Department,
                DepartmentUnit = dto.DepartmentUnit,
                PurchaseDate = dto.PurchaseDate,
                PurchaseCost = dto.PurchaseCost,
                ResponsibleOfficer = dto.ResponsibleOfficer,
                Condition = dto.Condition,
                RequestedBy = userEmail,
                RequestedAt = DateTimeOffset.UtcNow,
                Status = AssetStatus.Pending
            };

            _context.FurnitureFittings.Add(entity);
            await _context.SaveChangesAsync();

            // Send notification
            var summary = $"{entity.ItemDescription} - {entity.Quantity}";
            await _emailService.NotifyAssetCreatedAsync("FurnitureFitting", entity.Id, summary, userEmail);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        // GET ALL (optionally include pending for admins)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includePending = false)
        {
            var isAdmin = User.IsInRole("admin");

            if (isAdmin && includePending)
            {
                return Ok(await _context.FurnitureFittings.ToListAsync());
            }

            var result = await _context.FurnitureFittings
                .Where(a => a.Status == AssetStatus.Approved)
                .ToListAsync();

            return Ok(result);
        }

        // GET PENDING (admin only)
        [Authorize(Roles = "admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _context.FurnitureFittings
                .Where(a => a.Status == AssetStatus.Pending)
                .ToListAsync();
            return Ok(pending);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.FurnitureFittings.FindAsync(id);
            if (entity == null) return NotFound();

            if (entity.Status == AssetStatus.Pending && !User.IsInRole("admin"))
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
                if (!string.Equals(userEmail, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                    return Forbid();
            }

            return Ok(entity);
        }

        // UPDATE (admin only)
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FurnitureFitting.FurnitureFittingCreateDto dto)
        {
            var entity = await _context.FurnitureFittings.FindAsync(id);
            if (entity == null) return NotFound();

            entity.ItemDescription = dto.ItemDescription;
            entity.SerialNumber = dto.SerialNumber;
            entity.Quantity = dto.Quantity;
            entity.Location = dto.Location;
            entity.Department = dto.Department;
            entity.DepartmentUnit = dto.DepartmentUnit;
            entity.PurchaseDate = dto.PurchaseDate;
            entity.PurchaseCost = dto.PurchaseCost;
            entity.ResponsibleOfficer = dto.ResponsibleOfficer;
            entity.Condition = dto.Condition;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE (admin only)
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.FurnitureFittings.FindAsync(id);
            if (entity == null) return NotFound();

            _context.FurnitureFittings.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // APPROVE / REJECT (admin only)
        [Authorize(Roles = "admin")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] FurnitureFitting.FurnitureFittingApproveDto dto)
        {
            var entity = await _context.FurnitureFittings.FindAsync(id);
            if (entity == null) return NotFound();

            var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            // Approval logic
            entity.ApprovedBy = adminEmail;
            entity.ApprovalDate = DateTimeOffset.UtcNow;
            entity.ApprovalRemarks = dto.Remarks;
            entity.Status = dto.Approve ? AssetStatus.Approved : AssetStatus.Rejected;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var summary = $"{entity.ItemDescription} - {entity.Quantity}";
            await _emailService.NotifyAssetApprovalAsync(
                "FurnitureFitting",
                entity.Id,
                summary,
                entity.RequestedBy,
                dto.Approve,
                dto.Remarks,
                adminEmail
            );

            return NoContent();
        }
    }
}
