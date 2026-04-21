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
    public class EditListingModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;

        public EditListingModel(ApplicationDbContext context, IFileStorageService fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        [BindProperty]
        public PropertyListing Listing { get; set; } = null!;

        [BindProperty]
        public List<IFormFile> NewImages { get; set; } = new List<IFormFile>();

        [BindProperty]
        public List<IFormFile> FloorPlanImages { get; set; } = new List<IFormFile>();

        [BindProperty]
        public int Bedrooms { get; set; }
        [BindProperty]
        public int Bathrooms { get; set; }
        [BindProperty]
        public int Sqft { get; set; }

        public List<FeatureConfig> ListingTypes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            await LoadListingTypesAsync();

            var listing = await _context.PropertyListings
                .Include(l => l!.Media)
                .Include(l => l!.Features)
                .Include(l => l!.FloorPlans)
                .FirstOrDefaultAsync(m => m.ListingId == id);

            if (listing == null)
            {
                return NotFound();
            }

            Listing = listing;

            if (Listing.TenantId != tenantId)
            {
                return Forbid();
            }

            // Load features into properties
            int.TryParse(Listing.Features.FirstOrDefault(f => f.Key == "Bedrooms")?.Value, out int beds);
            Bedrooms = beds;
            int.TryParse(Listing.Features.FirstOrDefault(f => f.Key == "Bathrooms")?.Value, out int baths);
            Bathrooms = baths;
            int.TryParse(Listing.Features.FirstOrDefault(f => f.Key == "Sqft")?.Value, out int s);
            Sqft = s;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            var listingToUpdate = await _context.PropertyListings
                .Include(l => l.Features)
                .FirstOrDefaultAsync(l => l.ListingId == Listing.ListingId);

            if (listingToUpdate == null) return NotFound();
            if (listingToUpdate.TenantId != tenantId) return Forbid();

            await LoadListingTypesAsync();
            Listing.ListingType = NormalizeListingType(Listing.ListingType);
            if (!ListingTypes.Any(t => t.FeatureKey == Listing.ListingType))
            {
                ModelState.AddModelError("Listing.ListingType", "This listing type is currently disabled.");
                Listing = listingToUpdate;
                return Page();
            }

            listingToUpdate.Title = Listing.Title;
            listingToUpdate.Location = Listing.Location;
            listingToUpdate.Price = Listing.Price;
            listingToUpdate.Description = Listing.Description;
            listingToUpdate.Status = Listing.Status;
            listingToUpdate.ListingType = Listing.ListingType;
            listingToUpdate.PropertyType = Listing.PropertyType;
            listingToUpdate.UpdatedAt = DateTime.UtcNow;

            // Update Features
            var bedsFeature = listingToUpdate.Features.FirstOrDefault(f => f.Key == "Bedrooms");
            if (bedsFeature != null) bedsFeature.Value = Bedrooms.ToString();
            else listingToUpdate.Features.Add(new PropertyFeature { Key = "Bedrooms", Value = Bedrooms.ToString() });

            var bathsFeature = listingToUpdate.Features.FirstOrDefault(f => f.Key == "Bathrooms");
            if (bathsFeature != null) bathsFeature.Value = Bathrooms.ToString();
            else listingToUpdate.Features.Add(new PropertyFeature { Key = "Bathrooms", Value = Bathrooms.ToString() });

            var sqftFeature = listingToUpdate.Features.FirstOrDefault(f => f.Key == "Sqft");
            if (sqftFeature != null) sqftFeature.Value = Sqft.ToString();
            else listingToUpdate.Features.Add(new PropertyFeature { Key = "Sqft", Value = Sqft.ToString() });

            // Handle New Images
            if (NewImages != null && NewImages.Count > 0)
            {
                foreach (var image in NewImages)
                {
                    using (var stream = image.OpenReadStream())
                    {
                        var url = await _fileStorage.SaveFileAsync(stream, image.FileName, "listings");
                        _context.PropertyMedia.Add(new PropertyMedia { ListingId = listingToUpdate.ListingId, MediaUrl = url, MediaType = "Image" });
                    }
                }
            }

            // Handle New Floor Plans
            if (FloorPlanImages != null && FloorPlanImages.Count > 0)
            {
                foreach (var fp in FloorPlanImages)
                {
                    using (var stream = fp.OpenReadStream())
                    {
                        var url = await _fileStorage.SaveFileAsync(stream, fp.FileName, "floorplans");
                        _context.FloorPlans.Add(new FloorPlan { ListingId = listingToUpdate.ListingId, ImageUrl = url, Description = "Standard Floor Plan" });
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListingExists(Listing.ListingId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Agents");
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId)) return RedirectToPage("/Login");

            var listing = await _context.PropertyListings.FirstOrDefaultAsync(l => l.ListingId == id);
            if (listing == null) return NotFound();
            if (listing.TenantId != tenantId) return Forbid();

            _context.PropertyListings.Remove(listing);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Listing deleted successfully.";
            return RedirectToPage("./Agents");
        }

        private bool ListingExists(Guid id)
        {
            return _context.PropertyListings.Any(e => e.ListingId == id);
        }

        private async Task LoadListingTypesAsync()
        {
            ListingTypes = await _context.FeatureConfigs
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
