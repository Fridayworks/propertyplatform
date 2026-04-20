using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Infrastructure.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;

        public WalletService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetBalanceAsync(Guid tenantId)
        {
            var profile = await _context.AgentProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.TenantId == tenantId);
            return profile?.Credits ?? 0;
        }

        public async Task<bool> AddCreditsAsync(Guid tenantId, int amount, string type, string description)
        {
            if (amount <= 0) return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var profile = await _context.AgentProfiles
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.TenantId == tenantId);
                
                if (profile == null) return false;

                profile.Credits += amount;

                _context.CreditTransactions.Add(new CreditTransaction
                {
                    TenantId = tenantId,
                    Amount = amount,
                    Type = type,
                    Description = description
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeductCreditsAsync(Guid tenantId, int amount, string type, string description)
        {
            if (amount <= 0) return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var profile = await _context.AgentProfiles
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.TenantId == tenantId);

                if (profile == null || profile.Credits < amount) return false;

                profile.Credits -= amount;

                _context.CreditTransactions.Add(new CreditTransaction
                {
                    TenantId = tenantId,
                    Amount = -amount,
                    Type = type,
                    Description = description
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<CreditTransaction>> GetTransactionHistoryAsync(Guid tenantId, int count = 50)
        {
            return await _context.CreditTransactions
                .IgnoreQueryFilters()
                .Where(t => t.TenantId == tenantId)
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
