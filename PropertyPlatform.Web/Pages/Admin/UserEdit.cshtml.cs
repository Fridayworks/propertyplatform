using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages.Admin
{
    public class UserEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserEditModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public AgentProfile Agent { get; set; } = new();

        public List<SelectListItem> AdminRoles { get; set; } = new();

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (!IsCurrentUserAdmin()) return RedirectToPage("/Login");

            if (id == null) return NotFound();

            var agent = await _context.AgentProfiles.FindAsync(id);
            if (agent == null) return NotFound();

            Agent = agent;

            await PopulateRolesDropdownAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            if (!ModelState.IsValid)
            {
                await PopulateRolesDropdownAsync();
                return Page();
            }

            var agentToUpdate = await _context.AgentProfiles.FindAsync(Agent.AgentId);
            if (agentToUpdate == null) return NotFound();

            // Only update editable fields
            agentToUpdate.Name = Agent.Name;
            agentToUpdate.Email = Agent.Email;
            agentToUpdate.Phone = Agent.Phone;
            agentToUpdate.REN_ID = Agent.REN_ID;
            agentToUpdate.Credits = Agent.Credits;
            agentToUpdate.Level = Agent.Level;
            agentToUpdate.AdminRoleId = Agent.AdminRoleId;
            agentToUpdate.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                SuccessMessage = "User profile updated successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to update user profile.";
            }

            await PopulateRolesDropdownAsync();
            return Page();
        }

        private async Task PopulateRolesDropdownAsync()
        {
            var roles = await _context.AdminRoles.OrderBy(r => r.Name).ToListAsync();
            AdminRoles = roles.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.Name
            }).ToList();
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
