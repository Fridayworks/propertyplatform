using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages
{
    [Authorize]
    public class AgentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AgentsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<PropertyListing> MyListings { get; set; } = new List<PropertyListing>();
        public int TotalLeads { get; set; }
        public int TotalViews { get; set; }
        public int AgentCredits { get; set; }
        public string ReferralLink { get; set; } = string.Empty;
        public int TotalReferrals { get; set; }

        public async Task OnGetAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(tenantIdString, out Guid tenantId))
            {
                ReferralLink = $"{Request.Scheme}://{Request.Host}/Signup?referrer={tenantId}";
                TotalReferrals = await _context.Referrals.CountAsync(r => r.ReferrerTenantId == tenantId);

                MyListings = await _context.PropertyListings
                    .Include(l => l.Analytics)
                    .Include(l => l.FeaturedListing)
                    .OrderByDescending(l => l.CreatedAt)
                    .ToListAsync();

                TotalLeads = await _context.Leads.CountAsync();
                TotalViews = await _context.ListingAnalytics.SumAsync(a => a.Views);

                var profile = await _context.AgentProfiles.FirstOrDefaultAsync(p => p.TenantId == tenantId);
                AgentCredits = profile?.Credits ?? 0;
            }
        }

        public async Task<IActionResult> OnPostBoostAsync(Guid id)
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId)) return RedirectToPage("/Login");

            var profile = await _context.AgentProfiles.FirstOrDefaultAsync(p => p.TenantId == tenantId);
            if (profile == null || profile.Credits < 10) // 10 credits to boost
            {
                TempData["ErrorMessage"] = "Insufficient credits to boost this listing.";
                return RedirectToPage();
            }

            var listing = await _context.PropertyListings
                .Include(l => l.FeaturedListing)
                .FirstOrDefaultAsync(l => l.ListingId == id && l.TenantId == tenantId);

            if (listing == null) return NotFound();

            if (listing.FeaturedListing != null && listing.FeaturedListing.EndDate > DateTime.UtcNow)
            {
                TempData["ErrorMessage"] = "Listing is already featured.";
                return RedirectToPage();
            }

            // Deduct credits and feature
            profile.Credits -= 10;
            
            if (listing.FeaturedListing == null)
            {
                _context.FeaturedListings.Add(new FeaturedListing
                {
                    ListingId = listing.ListingId,
                    EndDate = DateTime.UtcNow.AddDays(7)
                });
            }
            else
            {
                listing.FeaturedListing.EndDate = DateTime.UtcNow.AddDays(7);
                listing.FeaturedListing.StartDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Listing boosted successfully for 7 days!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            var listing = await _context.PropertyListings.FindAsync(id);
            if (listing != null && listing.TenantId == tenantId)
            {
                _context.PropertyListings.Remove(listing);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
