using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Infrastructure.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ApplicationDbContext _context;

        public RecommendationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PropertyListing>> GetRecommendedPropertiesAsync(Guid? currentListingId = null, int count = 3)
        {
            var query = _context.PropertyListings
                .IgnoreQueryFilters()
                .Include(l => l.Media)
                .Where(l => l.Status == "Active");

            if (currentListingId.HasValue)
            {
                var current = await _context.PropertyListings
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(l => l.ListingId == currentListingId.Value);

                if (current != null)
                {
                    // Rule: Same location OR price within 20% range
                    var minPrice = current.Price * 0.8m;
                    var maxPrice = current.Price * 1.2m;

                    return await query
                        .Where(l => l.ListingId != currentListingId.Value)
                        .Where(l => l.Location == current.Location || (l.Price >= minPrice && l.Price <= maxPrice))
                        .OrderByDescending(l => l.CreatedAt)
                        .Take(count)
                        .ToListAsync();
                }
            }
            
            return await query
                .OrderByDescending(l => l.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
