using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages
{
    public class CompareModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CompareModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PropertyListing> Properties { get; set; } = new List<PropertyListing>();
        public List<string> FeatureKeys { get; set; } = new();

        public async Task OnGetAsync(string? ids)
        {
            if (string.IsNullOrEmpty(ids)) return;

            var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(s => Guid.TryParse(s, out var g) ? g : Guid.Empty)
                           .Where(g => g != Guid.Empty)
                           .ToList();

            if (!idList.Any()) return;

            Properties = await _context.PropertyListings
                .Include(l => l.Media)
                .Include(l => l.Features)
                .Include(l => l.Tenant)
                .Where(l => idList.Contains(l.ListingId))
                .ToListAsync();

            // Collect all unique feature keys for comparison rows
            FeatureKeys = Properties.SelectMany(p => p.Features)
                                    .Select(f => f.Key)
                                    .Distinct()
                                    .OrderBy(k => k)
                                    .ToList();
        }
    }
}
