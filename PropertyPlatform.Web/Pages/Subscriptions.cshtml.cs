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
    public class SubscriptionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWalletService _walletService;

        public SubscriptionsModel(ApplicationDbContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
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

            await _context.SaveChangesAsync();

            // Bonus credits for upgrading via wallet service
            if (plan == "Pro") await _walletService.AddCreditsAsync(tenantId, 20, "SubscriptionBonus", "Bonus for Pro Plan upgrade");
            if (plan == "Premium") await _walletService.AddCreditsAsync(tenantId, 100, "SubscriptionBonus", "Bonus for Premium Plan upgrade");
            
            TempData["SuccessMessage"] = $"Successfully upgraded to {plan} plan!";
            return RedirectToPage();
        }
    }
}
