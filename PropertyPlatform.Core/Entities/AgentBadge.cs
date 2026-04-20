namespace PropertyPlatform.Core.Entities
{
    public class AgentBadge
    {
        public Guid AgentBadgeId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public Guid BadgeId { get; set; }
        public DateTime AwardedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Tenant? Tenant { get; set; }
        public Badge? Badge { get; set; }
    }
}
