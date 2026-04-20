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
        }

        [HttpPost]
        public async Task<IActionResult> CreateListing([FromBody] CreateListingRequest request)
        {
            var tenantId = GetCurrentTenantId();

            var listing = new PropertyListing
            {
                TenantId = tenantId,
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                Price = request.Price,
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
            listing.Status = request.Status;
            listing.UpdatedAt = DateTime.UtcNow;

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
    }
}