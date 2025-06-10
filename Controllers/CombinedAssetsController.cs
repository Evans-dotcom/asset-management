using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asset_management.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CombinedAssetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CombinedAssetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{assetType}")]
        public async Task<IActionResult> GetAll(string assetType)
        {
            switch (assetType.ToLower())
            {
                case "standard":
                    return Ok(await _context.StandardAssets.ToListAsync());
                case "furniture":
                    return Ok(await _context.FurnitureFittings.ToListAsync());
                case "plant":
                    return Ok(await _context.PlantMachineries.ToListAsync());
                case "portable":
                    return Ok(await _context.PortableItems.ToListAsync());
                case "vehicle":
                    return Ok(await _context.MotorVehicles.ToListAsync());
                case "bank":
                    return Ok(await _context.BankAccounts.ToListAsync());
                case "land":
                    return Ok(await _context.LandRegisters.ToListAsync());
                case "building":
                    return Ok(await _context.BuildingsRegisters.ToListAsync());
                case "intangible":
                    return Ok(await _context.IntangibleAssets.ToListAsync());
                case "stock":
                    return Ok(await _context.StocksRegisters.ToListAsync());
                case "road":
                    return Ok(await _context.RoadsInfrastructures.ToListAsync());
                case "infrastructure":
                    return Ok(await _context.OtherInfrastructures.ToListAsync());
                case "bio":
                    return Ok(await _context.BiologicalAssets.ToListAsync());
                case "subsoil":
                    return Ok(await _context.SubsoilAssets.ToListAsync());
                default:
                    return BadRequest("Invalid asset type.");
            }
        }

        [HttpGet("{assetType}/{id}")]
        public async Task<IActionResult> GetById(string assetType, int id)
        {
            object? asset = assetType.ToLower() switch
            {
                "standard" => await _context.StandardAssets.FindAsync(id),
                "furniture" => await _context.FurnitureFittings.FindAsync(id),
                "plant" => await _context.PlantMachineries.FindAsync(id),
                "portable" => await _context.PortableItems.FindAsync(id),
                "vehicle" => await _context.MotorVehicles.FindAsync(id),
                "land" => await _context.LandRegisters.FindAsync(id),
                "building" => await _context.BuildingsRegisters.FindAsync(id),
                "intangible" => await _context.IntangibleAssets.FindAsync(id),
                "stock" => await _context.StocksRegisters.FindAsync(id),
                "road" => await _context.RoadsInfrastructures.FindAsync(id),
                "infrastructure" => await _context.OtherInfrastructures.FindAsync(id),
                "bio" => await _context.BiologicalAssets.FindAsync(id),
                "subsoil" => await _context.SubsoilAssets.FindAsync(id),
                _ => null
            };

            if (asset == null)
                return NotFound();

            return Ok(asset);
        }

    }
}
