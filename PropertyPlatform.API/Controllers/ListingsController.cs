using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Constants;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ListingsController(ApplicationDbContext context)
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

        public class CreateListingRequest
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Status { get; set; } = "Draft";
            public string ListingType { get; set; } = ListingTypeKeys.Sale;
            public string PropertyType { get; set; } = "Condo";
        }

        [HttpPost]
        public async Task<IActionResult> CreateListing([FromBody] CreateListingRequest request)
        {
            var tenantId = GetCurrentTenantId();
            var listingType = NormalizeListingType(request.ListingType);

            if (!await IsListingTypeEnabledAsync(listingType))
            {
                return BadRequest($"Listing type '{request.ListingType}' is disabled.");
            }

            var listing = new PropertyListing
            {
                TenantId = tenantId,
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                Price = request.Price,
                ListingType = listingType,
                PropertyType = request.PropertyType,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PropertyListings.Add(listing);
            await _context.SaveChangesAsync();

            return Ok(listing);
        }

        [HttpGet]
        public async Task<IActionResult> GetListings()
        {
            var tenantId = GetCurrentTenantId();

            var listings = await _context.PropertyListings
                .Where(l => l.TenantId == tenantId)
                .ToListAsync();

            return Ok(listings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetListing(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var listing = await _context.PropertyListings
                .FirstOrDefaultAsync(l => l.ListingId == id && l.TenantId == tenantId);

            if (listing == null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateListing(Guid id, [FromBody] CreateListingRequest request)
        {
            var tenantId = GetCurrentTenantId();

            var listing = await _context.PropertyListings
                .FirstOrDefaultAsync(l => l.ListingId == id && l.TenantId == tenantId);

            if (listing == null)
            {
                return NotFound();
            }

            listing.Title = request.Title;
            listing.Description = request.Description;
            listing.Location = request.Location;
            listing.Price = request.Price;
            listing.ListingType = NormalizeListingType(request.ListingType);
            listing.PropertyType = request.PropertyType;
            listing.Status = request.Status;
            listing.UpdatedAt = DateTime.UtcNow;

            if (!await IsListingTypeEnabledAsync(listing.ListingType))
            {
                return BadRequest($"Listing type '{request.ListingType}' is disabled.");
            }

            await _context.SaveChangesAsync();

            return Ok(listing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListing(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var listing = await _context.PropertyListings
                .FirstOrDefaultAsync(l => l.ListingId == id && l.TenantId == tenantId);

            if (listing == null)
            {
                return NotFound();
            }

            _context.PropertyListings.Remove(listing);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private static string NormalizeListingType(string? listingType)
        {
            var normalized = listingType?.Trim().ToLowerInvariant();
            return ListingTypeKeys.All.Contains(normalized) ? normalized! : ListingTypeKeys.Sale;
        }

        private async Task<bool> IsListingTypeEnabledAsync(string listingType)
        {
            return await _context.FeatureConfigs
                .AnyAsync(f => f.Category == "ListingType" && f.FeatureKey == listingType && f.IsEnabled);
        }
    }
}
