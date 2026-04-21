using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("features")]
        public async Task<IActionResult> GetFeatures([FromQuery] string? category = null)
        {
            var query = _context.FeatureConfigs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(f => f.Category == category);
            }

            var features = await query
                .OrderBy(f => f.Category)
                .ThenBy(f => f.SortOrder)
                .ToListAsync();

            return Ok(features);
        }
    }
}
