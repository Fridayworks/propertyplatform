using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages
{
    [Authorize]
    public class WalletModel : PageModel
    {
        private readonly IWalletService _walletService;

        public WalletModel(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public int Balance { get; set; }
        public List<CreditTransaction> Transactions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(tenantIdString, out Guid tenantId))
                return RedirectToPage("/Login");

            Balance = await _walletService.GetBalanceAsync(tenantId);
            Transactions = await _walletService.GetTransactionHistoryAsync(tenantId);

            return Page();
        }
    }
}
