using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class Tenant
    {
        public Guid TenantId { get; set; } = Guid.NewGuid();
        
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string? LogoUrl { get; set; }
        
        public string? ContactEmail { get; set; }
        
        public string? ContactPhone { get; set; }
        
        public string? Address { get; set; }
        
        public string? City { get; set; }
        
        public string? State { get; set; }
        
        public string? Country { get; set; }
        
        public string? PostalCode { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public AgentProfile? AgentProfile { get; set; }
        public ICollection<PropertyListing> PropertyListings { get; set; } = new List<PropertyListing>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<CreditTransaction> CreditTransactions { get; set; } = new List<CreditTransaction>();
        public ICollection<Referral> ReferralsMade { get; set; } = new List<Referral>();
	public ICollection<Referral> ReferralsReceived { get; set; } = new List<Referral>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Developer> Developers { get; set; } = new List<Developer>();
    }
}