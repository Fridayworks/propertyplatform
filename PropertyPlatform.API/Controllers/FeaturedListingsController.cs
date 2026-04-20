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
    public class FeaturedListingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeaturedListingsController(ApplicationDbContext context)
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

        [HttpGet]
        public async Task<IActionResult> GetFeaturedListings()
        {
            var featured = await _context.FeaturedListings
                .Include(f => f.Listing)
                .Where(f => f.EndDate > DateTime.UtcNow)
                .OrderByDescending(f => f.BoostLevel)
                .ToListAsync();

            return Ok(featured);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFeaturedListing([FromBody] FeaturedListing featured)
        {
            var tenantId = GetCurrentTenantId();

            // Verify listing belongs to tenant
            var listing = await _context.PropertyListings
                .FirstOrDefaultAsync(l => l.ListingId == featured.ListingId && l.TenantId == tenantId);

            if (listing == null)
            {
                return NotFound("Listing not found");
            }

            _context.FeaturedListings.Add(featured);
            await _context.SaveChangesAsync();

            return Ok(featured);
        }
    }
}