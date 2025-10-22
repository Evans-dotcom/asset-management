//using Asset_management.models;
//using Asset_management.services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace AssetManagementSystem.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class BankAccountController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly TokenService _tokenService;

//        public BankAccountController(ApplicationDbContext context, TokenService tokenService)
//        {
//            _context = context;
//            _tokenService = tokenService;
//        }

//        [AllowAnonymous]
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] BankAccount asset)
//        {
//            try
//            {
//                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
//                var userRole = User.FindFirst("role")?.Value;

//                _context.BankAccounts.Add(asset);
//                await _context.SaveChangesAsync();

//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.InnerException.Message);
//            }


//            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
//        }

//        [HttpPut("{id}")]
//        // [Authorize(Roles = "admin,user")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Update(int id, [FromBody] BankAccount asset)
//        {
//            if (id != asset.Id)
//                return BadRequest();

//            _context.Entry(asset).State = EntityState.Modified;
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        [Authorize(Roles = "admin")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var asset = await _context.BankAccounts.FindAsync(id);
//            if (asset == null) return NotFound();

//            _context.BankAccounts.Remove(asset);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpGet("{id}")]
//        [AllowAnonymous]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var asset = await _context.BankAccounts.FindAsync(id);
//            if (asset == null) return NotFound();
//            return Ok(asset);
//        }

//        [AllowAnonymous]
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var bankAccounts = await _context.BankAccounts.ToListAsync();
//            return Ok(bankAccounts);
//        }

//        [HttpGet("me")]
//        [AllowAnonymous]
//        public IActionResult GetMyUserInfo()
//        {
//            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
//            var role = User.FindFirst("role")?.Value;

//            return Ok(new { Email = email, Role = role });
//        }
//    }
//}
using System.Security.Claims;
using Asset_management.DTOs;
using Asset_management.models;
using Asset_management.NewFolder;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BankAccountCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
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
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pending = await _context.BankAccounts
            .Where(b => b.Status == BankAccountStatus.Open)
            .ToListAsync();
        return Ok(pending);
    }

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeUnapproved = false)
    {
        var isAdmin = User.IsInRole("admin");

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


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _context.BankAccounts.FindAsync(id);
        if (entity == null) return NotFound();

        if (entity.Status != BankAccountStatus.Approved && !User.IsInRole("admin"))
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!string.Equals(userEmail, entity.RequestedBy, StringComparison.OrdinalIgnoreCase))
                return Forbid();
        }

        return Ok(entity);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] BankAccountApproveDto dto)
    {
        var entity = await _context.BankAccounts.FindAsync(id);
        if (entity == null) return NotFound();

        var adminEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        entity.ApprovedBy = adminEmail;
        entity.ApprovalDate = DateTimeOffset.UtcNow;
        entity.ApprovalRemarks = dto.Remarks;
        entity.Status = dto.Approve ? BankAccountStatus.Approved : BankAccountStatus.Rejected;

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.BankAccounts.FindAsync(id);
        if (entity == null) return NotFound();

        _context.BankAccounts.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
