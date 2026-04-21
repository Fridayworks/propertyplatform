using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages
{
    [Authorize]
    public class SelectListingTypeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SelectListingTypeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<FeatureConfig> ListingTypes { get; set; } = new();

        public async Task OnGetAsync()
        {
            ListingTypes = await _context.FeatureConfigs
                .AsNoTracking()
                .Where(f => f.Category == "ListingType" && f.IsEnabled)
                .OrderBy(f => f.SortOrder)
                .ToListAsync();
        }
    }
}
