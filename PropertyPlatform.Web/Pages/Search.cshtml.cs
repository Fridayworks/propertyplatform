using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PropertyListing> Listings { get; set; } = new List<PropertyListing>();
        
        public string? SearchQuery { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? PropertyType { get; set; }
        public string? ListingType { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(string? query, decimal? minPrice, decimal? maxPrice, string? propertyType, string? listingType, int p = 1)
        {
            SearchQuery = query;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            PropertyType = propertyType;
            ListingType = listingType;
            PageNumber = p <= 0 ? 1 : p;
            int pageSize = 12;

            var q = _context.PropertyListings
                .Include(l => l.Media)
                .Include(l => l.Features)
                .Include(l => l.FloorPlans)
                .Include(l => l.FeaturedListing)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                q = q.Where(l => l.Title.Contains(query) || l.Location.Contains(query) || l.Description.Contains(query));
            }

            if (minPrice.HasValue)
            {
                q = q.Where(l => l.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                q = q.Where(l => l.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                q = q.Where(l => l.PropertyType == propertyType);
            }

            if (!string.IsNullOrEmpty(listingType))
            {
                q = q.Where(l => l.ListingType == listingType);
            }

            int totalCount = await q.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Listings = await q
                .Include(l => l.Analytics)
                .OrderByDescending(l => l.FeaturedListing != null && l.FeaturedListing.EndDate > DateTime.UtcNow)
                .ThenByDescending(l => (l.Analytics != null ? l.Analytics.Views : 0))
                .ThenByDescending(l => l.CreatedAt)
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
