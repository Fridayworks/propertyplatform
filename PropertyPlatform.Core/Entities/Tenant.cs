using System;
using System.Collections.Generic;

namespace PropertyPlatform.Core.Entities
{
    public class Tenant
    {
        public Guid TenantId { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public AgentProfile? AgentProfile { get; set; }
        public ICollection<PropertyListing> PropertyListings { get; set; } = new List<PropertyListing>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
