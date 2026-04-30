using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages.Admin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public int TotalAgents { get; set; }
        public int TotalListings { get; set; }
        public int TotalLeads { get; set; }
        public decimal TotalRevenue { get; set; } // Placeholder for wallet/subscription integration

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            TotalAgents = await _context.AgentProfiles.CountAsync();
            TotalListings = await _context.PropertyListings.CountAsync();
            TotalLeads = await _context.Leads.CountAsync();
            
            // For now, let's just count active subscriptions or something similar
            // TotalRevenue = await _context.Subscriptions.SumAsync(s => s.Price); // If price exists
            TotalRevenue = 12450.00m; // Mock for now

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
