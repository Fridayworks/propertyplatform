using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SubmitReviewAsync(Guid agentTenantId, string reviewerName, string reviewerEmail, int rating, string comment)
        {
            if (rating < 1 || rating > 5) return false;

            // Check if reviewer is a "verified" lead
            var isVerified = await _context.Leads
                .IgnoreQueryFilters()
                .AnyAsync(l => l.TenantId == agentTenantId && l.BuyerEmail.ToLower() == reviewerEmail.ToLower());

            var review = new AgentReview
            {
                AgentTenantId = agentTenantId,
                ReviewerName = reviewerName,
                ReviewerEmail = reviewerEmail,
                Rating = rating,
                Comment = comment,
                IsVerified = isVerified
            };

            _context.AgentReviews.Add(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AgentReview>> GetAgentReviewsAsync(Guid agentTenantId, int count = 20)
        {
            return await _context.AgentReviews
                .IgnoreQueryFilters()
                .Where(r => r.AgentTenantId == agentTenantId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(Guid agentTenantId)
        {
            var reviews = await _context.AgentReviews
                .IgnoreQueryFilters()
                .Where(r => r.AgentTenantId == agentTenantId)
                .ToListAsync();

            if (!reviews.Any()) return 0;

            return Math.Round(reviews.Average(r => r.Rating), 1);
        }

        public async Task<int> GetReviewCountAsync(Guid agentTenantId)
        {
            return await _context.AgentReviews
                .IgnoreQueryFilters()
                .Where(r => r.AgentTenantId == agentTenantId)
                .CountAsync();
        }
    }
}
