using Asset_management.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string assetType)
        {
            assetType = assetType.ToLower();
            return assetType switch
            {
                "standard" => Ok(await _context.StandardAssets.ToListAsync()),
                "furniture" => Ok(await _context.FurnitureFittings.ToListAsync()),
                "plant" => Ok(await _context.PlantMachineries.ToListAsync()),
                "portable" => Ok(await _context.PortableItems.ToListAsync()),
                "vehicle" => Ok(await _context.MotorVehicles.ToListAsync()),
                "bank" => Ok(await _context.BankAccounts.ToListAsync()),
                "land" => Ok(await _context.LandRegisters.ToListAsync()),
                "building" => Ok(await _context.BuildingsRegisters.ToListAsync()),
                "intangible" => Ok(await _context.IntangibleAssets.ToListAsync()),
                "stock" => Ok(await _context.StocksRegisters.ToListAsync()),
                "road" => Ok(await _context.RoadsInfrastructures.ToListAsync()),
                "infrastructure" => Ok(await _context.OtherInfrastructures.ToListAsync()),
                "bio" => Ok(await _context.BiologicalAssets.ToListAsync()),
                "subsoil" => Ok(await _context.SubsoilAssets.ToListAsync()),
                _ => BadRequest($"Invalid asset type: '{assetType}'.")
            };
        }

        [HttpGet("{assetType}/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string assetType, int id)
        {
            assetType = assetType.ToLower();

            object? asset = assetType switch
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
                return NotFound($"Asset of type '{assetType}' with ID '{id}' not found.");

            return Ok(asset);
        }
    }
}
