using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Constants;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages
{
    [Authorize]
    public class CreateListingModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;
        private readonly IGamificationService _gamificationService;

        public CreateListingModel(ApplicationDbContext context, IFileStorageService fileStorage, IGamificationService gamificationService)
        {
            _context = context;
            _fileStorage = fileStorage;
            _gamificationService = gamificationService;
        }

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string Location { get; set; } = string.Empty;

        [BindProperty]
        public decimal Price { get; set; }

        [BindProperty]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        public string PropertyType { get; set; } = "Condo";

        [BindProperty(SupportsGet = true)]
        public string ListingType { get; set; } = ListingTypeKeys.Sale;

        public List<FeatureConfig> EnabledListingTypes { get; set; } = new();
        public string SelectedListingTypeName { get; set; } = "Sale";
        
        [BindProperty]
        public int Bedrooms { get; set; }
        
        [BindProperty]
        public int Bathrooms { get; set; }
        
        [BindProperty]
        public int Sqft { get; set; }

        // Rent Specific
        [BindProperty]
        public string RentalConfiguration { get; set; } = "Whole Unit";

        // New Project Specific
        [BindProperty]
        public string ProjectOverview { get; set; } = string.Empty;

        [BindProperty]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

        [BindProperty]
        public List<IFormFile> FloorPlanImages { get; set; } = new List<IFormFile>();

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadListingTypesAsync();

            ListingType = NormalizeListingType(ListingType);
            var selectedType = EnabledListingTypes.FirstOrDefault(t => t.FeatureKey == ListingType);
            if (selectedType == null)
            {
                return RedirectToPage("/SelectListingType");
            }

            SelectedListingTypeName = selectedType.DisplayName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            await LoadListingTypesAsync();
            ListingType = NormalizeListingType(ListingType);
            var selectedType = EnabledListingTypes.FirstOrDefault(t => t.FeatureKey == ListingType);
            if (selectedType == null)
            {
                ModelState.AddModelError(nameof(ListingType), "This listing type is currently disabled.");
                return Page();
            }
            SelectedListingTypeName = selectedType.DisplayName;

            var newListing = new PropertyListing
            {
                TenantId = tenantId,
                Title = Title,
                Location = Location,
                Price = Price,
                ListingType = ListingType,
                PropertyType = PropertyType,
                Status = "Active",
                Description = string.IsNullOrEmpty(Description) ? "A great property." : Description
            };

            // Add basic features
            newListing.Features.Add(new PropertyFeature { Key = "Bedrooms", Value = Bedrooms.ToString() });
            newListing.Features.Add(new PropertyFeature { Key = "Bathrooms", Value = Bathrooms.ToString() });
            newListing.Features.Add(new PropertyFeature { Key = "Sqft", Value = Sqft.ToString() });

            if (ListingType == PropertyPlatform.Core.Constants.ListingTypeKeys.Rent)
            {
                newListing.Features.Add(new PropertyFeature { Key = "RentalConfiguration", Value = RentalConfiguration });
            }
            
            if (ListingType == PropertyPlatform.Core.Constants.ListingTypeKeys.NewProject)
            {
                newListing.Description = string.IsNullOrEmpty(ProjectOverview) ? Description : ProjectOverview;
            }

            _context.PropertyListings.Add(newListing);

            // Handle Images
            if (Images != null && Images.Count > 0)
            {
                int order = 0;
                foreach (var image in Images)
                {
                    using (var stream = image.OpenReadStream())
                    {
                        var url = await _fileStorage.SaveFileAsync(stream, image.FileName, "listings");
                        newListing.Media.Add(new PropertyMedia
                        {
                            ListingId = newListing.ListingId,
                            MediaUrl = url,
                            MediaType = "Image",
                            SortOrder = order++
                        });
                    }
                }
            }

            // Handle Floor Plans
            if (FloorPlanImages != null && FloorPlanImages.Count > 0)
            {
                foreach (var fp in FloorPlanImages)
                {
                    using (var stream = fp.OpenReadStream())
                    {
                        var url = await _fileStorage.SaveFileAsync(stream, fp.FileName, "floorplans");
                        newListing.FloorPlans.Add(new FloorPlan
                        {
                            ListingId = newListing.ListingId,
                            ImageUrl = url,
                            Description = "Standard Floor Plan"
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            // Gamification: Track listing missions and award XP
            await _gamificationService.AwardXPAsync(tenantId, 50); // 50 XP per listing
            await _gamificationService.TrackActionAsync(tenantId, "FIRST_LISTING");
            await _gamificationService.TrackActionAsync(tenantId, "UPLOAD_5_LISTINGS");

            return RedirectToPage("/Agents");
        }

        private async Task LoadListingTypesAsync()
        {
            EnabledListingTypes = await _context.FeatureConfigs
                .AsNoTracking()
                .Where(f => f.Category == "ListingType" && f.IsEnabled)
                .OrderBy(f => f.SortOrder)
                .ToListAsync();
        }

        private static string NormalizeListingType(string? listingType)
        {
            var normalized = listingType?.Trim().ToLowerInvariant();
            return ListingTypeKeys.All.Contains(normalized) ? normalized! : ListingTypeKeys.Sale;
        }
    }
}
