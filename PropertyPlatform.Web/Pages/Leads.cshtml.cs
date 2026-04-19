using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages
{
    [Authorize]
    public class LeadsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LeadsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Lead> Leads { get; set; } = new List<Lead>();

        public async Task OnGetAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(tenantIdString, out Guid tenantId))
            {
                Leads = await _context.Leads
                    .Include(l => l.Listing)
                    .OrderByDescending(l => l.Score)
                    .ThenByDescending(l => l.CreatedAt)
                    .ToListAsync();
            }
        }
    }
}
