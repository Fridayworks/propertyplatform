using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public CreateListingModel(ApplicationDbContext context, IFileStorageService fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
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
        
        [BindProperty]
        public int Bedrooms { get; set; }
        
        [BindProperty]
        public int Bathrooms { get; set; }
        
        [BindProperty]
        public int Sqft { get; set; }

        [BindProperty]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

        [BindProperty]
        public List<IFormFile> FloorPlanImages { get; set; } = new List<IFormFile>();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            var newListing = new PropertyListing
            {
                TenantId = tenantId,
                Title = Title,
                Location = Location,
                Price = Price,
                PropertyType = PropertyType,
                Status = "Active",
                Description = string.IsNullOrEmpty(Description) ? "A great property." : Description
            };

            // Add basic features
            newListing.Features.Add(new PropertyFeature { Key = "Bedrooms", Value = Bedrooms.ToString() });
            newListing.Features.Add(new PropertyFeature { Key = "Bathrooms", Value = Bathrooms.ToString() });
            newListing.Features.Add(new PropertyFeature { Key = "Sqft", Value = Sqft.ToString() });

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

            return RedirectToPage("/Agents");
        }
    }
}
