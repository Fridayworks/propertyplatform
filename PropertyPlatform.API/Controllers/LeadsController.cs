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
    public class LeadsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeadsController(ApplicationDbContext context)
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

        public class CreateLeadRequest
        {
            public Guid ListingId { get; set; }
            public string BuyerName { get; set; } = string.Empty;
            public string BuyerEmail { get; set; } = string.Empty;
            public string BuyerPhone { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string ContactType { get; set; } = "Email"; // Email, Phone, WhatsApp
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequest request)
        {
            var tenantId = GetCurrentTenantId();

            // Verify the listing belongs to the tenant
            var listing = await _context.PropertyListings
                .FirstOrDefaultAsync(l => l.ListingId == request.ListingId && l.TenantId == tenantId);

            if (listing == null)
            {
                return NotFound("Listing not found");
            }

            var lead = new Lead
            {
                ListingId = request.ListingId,
                TenantId = tenantId,
                BuyerName = request.BuyerName,
                BuyerEmail = request.BuyerEmail,
                BuyerPhone = request.BuyerPhone,
                Message = request.Message,
                ContactType = request.ContactType,
                Score = CalculateLeadScore(request), // Simple scoring
                CreatedAt = DateTime.UtcNow
            };

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            return Ok(lead);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLeads()
        {
            var tenantId = GetCurrentTenantId();

            var leads = await _context.Leads
                .Include(l => l.Listing)
                .Where(l => l.TenantId == tenantId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return Ok(leads);
        }

        private int CalculateLeadScore(CreateLeadRequest request)
        {
            int score = 0;
            if (!string.IsNullOrEmpty(request.BuyerPhone)) score += 20;
            if (!string.IsNullOrEmpty(request.Message) && request.Message.Length > 10) score += 30;
            if (request.ContactType == "Phone") score += 50;
            return score;
        }
    }
}