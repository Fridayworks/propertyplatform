using PropertyPlatform.Core.Entities;

namespace PropertyPlatform.Core.Interfaces
{
    public interface IReviewService
    {
        Task<bool> SubmitReviewAsync(Guid agentTenantId, string reviewerName, string reviewerEmail, int rating, string comment);
        Task<List<AgentReview>> GetAgentReviewsAsync(Guid agentTenantId, int count = 20);
        Task<double> GetAverageRatingAsync(Guid agentTenantId);
        Task<int> GetReviewCountAsync(Guid agentTenantId);
    }
}
