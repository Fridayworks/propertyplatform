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
    public class SubscriptionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Subscription? CurrentSubscription { get; set; }
        public string? SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(tenantIdString, out Guid tenantId))
            {
                CurrentSubscription = await _context.Subscriptions
                    .FirstOrDefaultAsync(s => s.TenantId == tenantId);
            }
        }

        public async Task<IActionResult> OnPostUpgradeAsync(string plan)
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId)) return Unauthorized();

            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.TenantId == tenantId);

            if (subscription == null)
            {
                subscription = new Subscription
                {
                    TenantId = tenantId,
                    Plan = plan,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1)
                };
                _context.Subscriptions.Add(subscription);
            }
            else
            {
                subscription.Plan = plan;
                subscription.EndDate = DateTime.UtcNow.AddMonths(1);
            }

            // Bonus credits for upgrading
            var profile = await _context.AgentProfiles.FirstOrDefaultAsync(p => p.TenantId == tenantId);
            if (profile != null)
            {
                if (plan == "Pro") profile.Credits += 20;
                if (plan == "Premium") profile.Credits += 100;
            }

            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Successfully upgraded to {plan} plan!";
            return RedirectToPage();
        }
    }
}
