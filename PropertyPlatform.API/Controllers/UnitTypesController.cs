using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UnitTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UnitTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Guid GetCurrentTenantId()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(tenantIdString, out Guid tenantId))
            {
                return tenantId;
            }
            throw new UnauthorizedAccessException("Invalid tenant");
        }

        public class CreateUnitTypeRequest
        {
            public Guid ProjectId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Type { get; set; } = "Apartment";
            public int Bedrooms { get; set; }
            public int Bathrooms { get; set; }
            public decimal Area { get; set; }
            public string AreaUnit { get; set; } = "sqft";
            public decimal Price { get; set; }
            public decimal PricePerSqft { get; set; }
            public string Status { get; set; } = "Active";
            public string Features { get; set; } = string.Empty;
            public string Amenities { get; set; } = string.Empty;
            public string FloorPlan { get; set; } = string.Empty;
            public string View { get; set; } = string.Empty;
            public string Orientation { get; set; } = string.Empty;
            public string Parking { get; set; } = string.Empty;
            public string Balcony { get; set; } = string.Empty;
            public string Terrace { get; set; } = string.Empty;
            public string Storage { get; set; } = string.Empty;
            public string Security { get; set; } = string.Empty;
            public string Utilities { get; set; } = string.Empty;
            public string Notes { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUnitType([FromBody] CreateUnitTypeRequest request)
        {
            var tenantId = GetCurrentTenantId();

            // Verify project belongs to tenant
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId && p.TenantId == tenantId);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            var unitType = new UnitType
            {
                ProjectId = request.ProjectId,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                Bedrooms = request.Bedrooms,
                Bathrooms = request.Bathrooms,
                Area = request.Area,
                AreaUnit = request.AreaUnit,
                Price = request.Price,
                PricePerSqft = request.PricePerSqft,
                Status = request.Status,
                Features = request.Features,
                Amenities = request.Amenities,
                FloorPlan = request.FloorPlan,
                View = request.View,
                Orientation = request.Orientation,
                Parking = request.Parking,
                Balcony = request.Balcony,
                Terrace = request.Terrace,
                Storage = request.Storage,
                Security = request.Security,
                Utilities = request.Utilities,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.UnitTypes.Add(unitType);
            await _context.SaveChangesAsync();

            return Ok(unitType);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnitTypes()
        {
            var tenantId = GetCurrentTenantId();

            var unitTypes = await _context.UnitTypes
                .Include(u => u.Project)
                .Where(u => u.Project.TenantId == tenantId)
                .ToListAsync();

            return Ok(unitTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitType(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var unitType = await _context.UnitTypes
                .Include(u => u.Project)
                .FirstOrDefaultAsync(u => u.UnitTypeId == id && u.Project.TenantId == tenantId);

            if (unitType == null)
            {
                return NotFound();
            }

            return Ok(unitType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnitType(Guid id, [FromBody] CreateUnitTypeRequest request)
        {
            var tenantId = GetCurrentTenantId();

            var unitType = await _context.UnitTypes
                .Include(u => u.Project)
                .FirstOrDefaultAsync(u => u.UnitTypeId == id && u.Project.TenantId == tenantId);

            if (unitType == null)
            {
                return NotFound();
            }

            unitType.Name = request.Name;
            unitType.Description = request.Description;
            unitType.Type = request.Type;
            unitType.Bedrooms = request.Bedrooms;
            unitType.Bathrooms = request.Bathrooms;
            unitType.Area = request.Area;
            unitType.AreaUnit = request.AreaUnit;
            unitType.Price = request.Price;
            unitType.PricePerSqft = request.PricePerSqft;
            unitType.Status = request.Status;
            unitType.Features = request.Features;
            unitType.Amenities = request.Amenities;
            unitType.FloorPlan = request.FloorPlan;
            unitType.View = request.View;
            unitType.Orientation = request.Orientation;
            unitType.Parking = request.Parking;
            unitType.Balcony = request.Balcony;
            unitType.Terrace = request.Terrace;
            unitType.Storage = request.Storage;
            unitType.Security = request.Security;
            unitType.Utilities = request.Utilities;
            unitType.Notes = request.Notes;
            unitType.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(unitType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitType(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var unitType = await _context.UnitTypes
                .Include(u => u.Project)
                .FirstOrDefaultAsync(u => u.UnitTypeId == id && u.Project.TenantId == tenantId);

            if (unitType == null)
            {
                return NotFound();
            }

            _context.UnitTypes.Remove(unitType);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}