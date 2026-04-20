using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;
        private readonly IGamificationService _gamificationService;

        public ProfileModel(ApplicationDbContext context, IFileStorageService fileStorage, IGamificationService gamificationService)
        {
            _context = context;
            _fileStorage = fileStorage;
            _gamificationService = gamificationService;
        }

        [BindProperty]
        public AgentProfile Profile { get; set; } = null!;

        [BindProperty]
        public IFormFile? Photo { get; set; }

        [BindProperty]
        public IFormFile? CompanyLogo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            Profile = await _context.AgentProfiles.FirstOrDefaultAsync(p => p.TenantId == tenantId);

            if (Profile == null)
            {
                Profile = new AgentProfile { TenantId = tenantId };
                _context.AgentProfiles.Add(Profile);
                await _context.SaveChangesAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            var profileToUpdate = await _context.AgentProfiles.FirstOrDefaultAsync(p => p.TenantId == tenantId);

            if (profileToUpdate == null) return NotFound();

            profileToUpdate.Name = Profile.Name;
            profileToUpdate.Phone = Profile.Phone;
            profileToUpdate.REN_ID = Profile.REN_ID;
            profileToUpdate.OfficeAddress = Profile.OfficeAddress;
            profileToUpdate.Bio = Profile.Bio;

            // Handle Slug
            if (!string.IsNullOrEmpty(Profile.Slug))
            {
                var slug = Profile.Slug.ToLower().Trim().Replace(" ", "-");
                var existing = await _context.AgentProfiles
                    .AnyAsync(p => p.Slug == slug && p.TenantId != tenantId);
                
                if (existing)
                {
                    ModelState.AddModelError("Profile.Slug", "This slug is already taken.");
                    Profile = profileToUpdate;
                    return Page();
                }
                profileToUpdate.Slug = slug;
            }

            if (Photo != null)
            {
                using (var stream = Photo.OpenReadStream())
                {
                    var url = await _fileStorage.SaveFileAsync(stream, Photo.FileName, "profiles");
                    profileToUpdate.ProfilePhotoUrl = url;
                }
            }

            if (CompanyLogo != null)
            {
                using (var stream = CompanyLogo.OpenReadStream())
                {
                    var url = await _fileStorage.SaveFileAsync(stream, CompanyLogo.FileName, "logos");
                    profileToUpdate.CompanyLogoUrl = url;
                }
            }

            await _context.SaveChangesAsync();

            // Gamification: Award Rising Star badge if profile is robust
            if (!string.IsNullOrEmpty(profileToUpdate.Bio) && !string.IsNullOrEmpty(profileToUpdate.ProfilePhotoUrl))
            {
                await _gamificationService.AwardBadgeAsync(tenantId, "RISING_STAR");
            }

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage();
        }
    }
}
