using PropertyPlatform.Core.Entities;

namespace PropertyPlatform.Core.Interfaces
{
    public interface IWalletService
    {
        Task<int> GetBalanceAsync(Guid tenantId);
        Task<bool> AddCreditsAsync(Guid tenantId, int amount, string type, string description);
        Task<bool> DeductCreditsAsync(Guid tenantId, int amount, string type, string description);
        Task<List<CreditTransaction>> GetTransactionHistoryAsync(Guid tenantId, int count = 50);
    }
}
