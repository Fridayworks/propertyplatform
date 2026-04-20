namespace PropertyPlatform.Core.Entities
{
    public class AgentReview
    {
        public Guid ReviewId { get; set; } = Guid.NewGuid();
        public Guid AgentTenantId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string ReviewerEmail { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; } = string.Empty;
        public bool IsVerified { get; set; } // Verified if linked to a lead
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Tenant? AgentTenant { get; set; }
    }
}
