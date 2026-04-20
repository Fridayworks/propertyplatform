using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages
{
    public class ListingModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IRecommendationService _recommendationService;

        public ListingModel(ApplicationDbContext context, IRecommendationService recommendationService)
        {
            _context = context;
            _recommendationService = recommendationService;
        }

        public PropertyListing? Listing { get; set; }
        public List<PropertyListing> RecommendedListings { get; set; } = new();

        [BindProperty]
        public string BuyerName { get; set; } = string.Empty;
        [BindProperty]
        public string BuyerEmail { get; set; } = string.Empty;
        [BindProperty]
        public string BuyerPhone { get; set; } = string.Empty;
        [BindProperty]
        public string Message { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Listing = await _context.PropertyListings
                .Include(l => l!.Tenant)
                .ThenInclude(t => t!.AgentProfile)
                .Include(l => l!.Media)
                .Include(l => l!.Features)
                .Include(l => l!.FloorPlans)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (Listing == null)
            {
                return NotFound();
            }

            // Track View
            var analytic = await _context.ListingAnalytics.FirstOrDefaultAsync(a => a.ListingId == id);
            if (analytic == null)
            {
                analytic = new ListingAnalytic { ListingId = id, Views = 1 };
                _context.ListingAnalytics.Add(analytic);
            }
            else
            {
                analytic.Views++;
                analytic.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();

            RecommendedListings = await _recommendationService.GetRecommendedPropertiesAsync(id);

            ViewData["MetaDescription"] = $"{Listing.Title} in {Listing.Location} - RM {Listing.Price:N0}. Check out this {Listing.PropertyType} on PropertyPlatform.";
            if (Listing.Media.Any())
            {
                ViewData["MetaImage"] = Listing.Media.First().MediaUrl;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostContactAsync(Guid id)
        {
            var listing = await _context.PropertyListings.FindAsync(id);
            if (listing == null) return NotFound();

            // Calculate Lead Score
            int score = 10; // Base score
            if (Message.Length > 50) score += 20;
            if (Message.Contains("budget", StringComparison.OrdinalIgnoreCase) || Message.Contains("RM", StringComparison.OrdinalIgnoreCase)) score += 30;
            if (Message.Contains("view", StringComparison.OrdinalIgnoreCase) || Message.Contains("visit", StringComparison.OrdinalIgnoreCase)) score += 20;

            var lead = new Lead
            {
                ListingId = id,
                TenantId = listing.TenantId,
                BuyerName = BuyerName,
                BuyerEmail = BuyerEmail,
                BuyerPhone = BuyerPhone,
                Message = Message,
                Score = score,
                CreatedAt = DateTime.UtcNow
            };

            _context.Leads.Add(lead);

            // Update contact analytics
            var analytic = await _context.ListingAnalytics.FirstOrDefaultAsync(a => a.ListingId == id);
            if (analytic != null)
            {
                analytic.Contacts++;
                analytic.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your message has been sent to the agent!";
            return RedirectToPage(new { id = id });
        }
    }
}
