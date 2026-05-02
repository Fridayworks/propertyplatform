using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class Developer
    {
        public Guid DeveloperId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string YearsOfOperation { get; set; } = string.Empty;
        public string Awards { get; set; } = string.Empty;
        public string ProjectsCompleted { get; set; } = string.Empty;
        public string Certifications { get; set; } = string.Empty;
        public string CompanyProfile { get; set; } = string.Empty;
        public string ProjectPortfolio { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Tenant? Tenant { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}