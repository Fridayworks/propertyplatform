using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages.Admin
{
    [Authorize]
    public class FeatureConfigModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public FeatureConfigModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<FeatureConfig> ListingTypes { get; set; } = new();

        [BindProperty]
        public Dictionary<string, bool> EnabledFeatures { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            await LoadListingTypesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            var configs = await _context.FeatureConfigs
                .Where(f => f.Category == "ListingType")
                .ToListAsync();

            foreach (var config in configs)
            {
                config.IsEnabled = EnabledFeatures.TryGetValue(config.FeatureKey, out var enabled) && enabled;
                config.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Feature configuration updated.";

            return RedirectToPage();
        }

        private async Task LoadListingTypesAsync()
        {
            ListingTypes = await _context.FeatureConfigs
                .AsNoTracking()
                .Where(f => f.Category == "ListingType")
                .OrderBy(f => f.SortOrder)
                .ToListAsync();

            EnabledFeatures = ListingTypes.ToDictionary(f => f.FeatureKey, f => f.IsEnabled);
        }

        private bool IsCurrentUserAdmin()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(email)) return false;

            var adminEmails = _configuration.GetSection("Admin:Emails").Get<string[]>() ?? [];
            return adminEmails.Any(admin => string.Equals(admin, email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
