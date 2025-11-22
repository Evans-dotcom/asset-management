using System.Security.Claims;
using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Asset_management.models.BuildingsRegister;

namespace Asset_management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsRegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public BuildingsRegisterController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BuildingsRegisterCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var now = DateTimeOffset.UtcNow;

            var entity = new BuildingsRegister
            {
                BuildingName = dto.BuildingName,
                Location = dto.Location,
                UsePurpose = dto.UsePurpose,
                DateConstructed = dto.DateConstructed ?? now,
                ConstructionCost = dto.ConstructionCost,
                Depreciation = dto.Depreciation,
                NetBookValue = dto.NetBookValue,
                Department = dto.Department,
                DepartmentUnit = dto.DepartmentUnit,
                RequestedBy = user,
                RequestedAt = now
            };

            _context.BuildingsRegisters.Add(entity);
            await _context.SaveChangesAsync();

            var summary = $"{entity.BuildingName} - {entity.Location}";
            await _emailService.NotifyAssetCreatedAsync("BuildingsRegister", entity.Id, summary, user);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _context.BuildingsRegisters
                .Where(b => b.ApprovedBy == null)
                .ToListAsync();

            return Ok(pending);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includePending = false)
        {
            var isAdmin = User.IsInRole("admin");

            if (isAdmin && includePending)
            {
                return Ok(await _context.BuildingsRegisters.ToListAsync());
            }

            var result = await _context.BuildingsRegisters
                .Where(b => b.ApprovedBy != null)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.BuildingsRegisters.FindAsync(id);
            if (entity == null) return NotFound();

            if (entity.ApprovedBy == null && !User.IsInRole("admin"))
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
                if (!string.Equals(user, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                    return Forbid();
            }

            return Ok(entity);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] BuildingsRegisterApproveDto dto)
        {
            var entity = await _context.BuildingsRegisters.FindAsync(id);
            if (entity == null) return NotFound();

            var admin = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            entity.ApprovedBy = admin;
            entity.ApprovalDate = DateTimeOffset.UtcNow;
            entity.ApprovalRemarks = dto.Remarks;

            if (!dto.Approve)
                entity.ApprovedBy = "REJECTED";

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var summary = $"{entity.BuildingName} - {entity.Location}";
            await _emailService.NotifyAssetApprovalAsync("BuildingsRegister", entity.Id, summary, entity.RequestedBy, dto.Approve, dto.Remarks, admin);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.BuildingsRegisters.FindAsync(id);
            if (entity == null) return NotFound();

            _context.BuildingsRegisters.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
