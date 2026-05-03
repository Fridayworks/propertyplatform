using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SmartListingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAISmartListingService _aiService;

        public SmartListingController(ApplicationDbContext context, IAISmartListingService aiService)
        {
            _context = context;
            _aiService = aiService;
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

        [HttpPost("generate-selling-points")]
        public async Task<IActionResult> GenerateSellingPoints([FromBody] GenerateSmartContentRequest request)
        {
            try
            {
                var listing = await _context.PropertyListings
                    .FirstOrDefaultAsync(l => l.ListingId == request.ListingId && l.TenantId == GetCurrentTenantId());

                if (listing == null)
                {
                    return NotFound("Listing not found");
                }

                var sellingPoints = await _aiService.GenerateSellingPointsAsync(listing);
                return Ok(new { SellingPoints = sellingPoints });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("generate-seo-title")]
        public async Task<IActionResult> GenerateSeoTitle([FromBody] GenerateSmartContentRequest request)
        {
            try
            {
                var listing = await _context.PropertyListings
                    .FirstOrDefaultAsync(l => l.ListingId == request.ListingId && l.TenantId == GetCurrentTenantId());

                if (listing == null)
                {
                    return NotFound("Listing not found");
                }

                var seoTitle = await _aiService.GenerateSeoTitleAsync(listing);
                return Ok(new { SeoTitle = seoTitle });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] GenerateSmartContentRequest request)
        {
            try
            {
                var listing = await _context.PropertyListings
                    .FirstOrDefaultAsync(l => l.ListingId == request.ListingId && l.TenantId == GetCurrentTenantId());

                if (listing == null)
                {
                    return NotFound("Listing not found");
                }

                var description = await _aiService.GenerateDescriptionAsync(listing);
                return Ok(new { Description = description });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("generate-faq")]
        public async Task<IActionResult> GenerateFaq([FromBody] GenerateSmartContentRequest request)
        {
            try
            {
                var listing = await _context.PropertyListings
                    .FirstOrDefaultAsync(l => l.ListingId == request.ListingId && l.TenantId == GetCurrentTenantId());

                if (listing == null)
                {
                    return NotFound("Listing not found");
                }

                var faq = await _aiService.GenerateFaqAsync(listing);
                return Ok(new { Faq = faq });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("generate-nearby-amenities")]
        public async Task<IActionResult> GenerateNearbyAmenities([FromBody] GenerateSmartContentRequest request)
        {
            try
            {
                var listing = await _context.PropertyListings
                    .FirstOrDefaultAsync(l => l.ListingId == request.ListingId && l.TenantId == GetCurrentTenantId());

                if (listing == null)
                {
                    return NotFound("Listing not found");
                }

                var amenities = await _aiService.GenerateNearbyAmenitiesNarrativeAsync(listing);
                return Ok(new { NearbyAmenities = amenities });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("generate-draft")]
        public async Task<IActionResult> GenerateEditableDraft([FromBody] GenerateSmartContentRequest request)
        {
            try
            {
                var listing = await _context.PropertyListings
                    .FirstOrDefaultAsync(l => l.ListingId == request.ListingId && l.TenantId == GetCurrentTenantId());

                if (listing == null)
                {
                    return NotFound("Listing not found");
                }

                var draft = await _aiService.GenerateEditableDraftAsync(listing);
                return Ok(new { Draft = draft });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        public class GenerateSmartContentRequest
        {
            public Guid ListingId { get; set; }
        }
    }
}