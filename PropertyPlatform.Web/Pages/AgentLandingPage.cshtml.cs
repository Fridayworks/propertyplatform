using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages
{
    public class AgentLandingPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IReviewService _reviewService;

        public AgentLandingPageModel(ApplicationDbContext context, IReviewService reviewService)
        {
            _context = context;
            _reviewService = reviewService;
        }

        public AgentProfile AgentProfile { get; set; } = null!;
        public IList<PropertyListing> Listings { get; set; } = new List<PropertyListing>();
        public IList<AgentReview> Reviews { get; set; } = new List<AgentReview>();
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            // Ignore global query filters for public agent discovery
            var agentProfile = await _context.AgentProfiles
                .IgnoreQueryFilters()
                .Include(a => a!.Tenant)
                .FirstOrDefaultAsync(a => a.Slug == slug);

            if (agentProfile == null) return NotFound();

            AgentProfile = agentProfile;

            Listings = await _context.PropertyListings
                .IgnoreQueryFilters()
                .Include(l => l.Media)
                .Where(l => l.TenantId == AgentProfile.TenantId && l.Status == "Active")
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            Reviews = await _reviewService.GetAgentReviewsAsync(AgentProfile.TenantId);
            AverageRating = await _reviewService.GetAverageRatingAsync(AgentProfile.TenantId);
            TotalReviews = await _reviewService.GetReviewCountAsync(AgentProfile.TenantId);

            return Page();
        }
    }
}
