using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("listings")]
        public async Task<IActionResult> SearchListings(
            [FromQuery] string? keyword,
            [FromQuery] string? location,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? listingType,
            [FromQuery] string? propertyType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.PropertyListings.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(l => l.Title.Contains(keyword) || l.Description.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(l => l.Location.Contains(location));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(l => l.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(l => l.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(listingType))
            {
                query = query.Where(l => l.ListingType == listingType);
            }

            if (!string.IsNullOrEmpty(propertyType))
            {
                query = query.Where(l => l.PropertyType == propertyType);
            }

            var totalCount = await query.CountAsync();
            var listings = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                listings,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }
    }
}
