namespace PropertyPlatform.Core.Entities
{
    public class AgentProfile
    {
        public Guid AgentId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string REN_ID { get; set; } = string.Empty;
        public string OfficeAddress { get; set; } = string.Empty;
        public string ProfilePhotoUrl { get; set; } = string.Empty;
        public string CompanyLogoUrl { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public int Credits { get; set; } = 0;
        public int ExperiencePoints { get; set; } = 0;
        public int Level { get; set; } = 1;

        // Navigation
        public Tenant? Tenant { get; set; }
    }
}
