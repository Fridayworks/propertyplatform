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
    public class UsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<AgentProfile> Agents { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            Agents = await _context.AgentProfiles
                .AsNoTracking()
                .Include(a => a.Tenant)
                .OrderByDescending(a => a.AgentId)
                .ToListAsync();

            return Page();
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
