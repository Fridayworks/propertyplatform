using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class Project
    {
        public Guid ProjectId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public Guid? DeveloperId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; } = "Active";
        public string ProjectType { get; set; } = "Residential"; // Residential, Commercial, Mixed-Use
        public string DeveloperName { get; set; } = string.Empty;
        public string DeveloperContact { get; set; } = string.Empty;
        public string DeveloperEmail { get; set; } = string.Empty;
        public string DeveloperPhone { get; set; } = string.Empty;
        public string DeveloperWebsite { get; set; } = string.Empty;
        public string DeveloperLogoUrl { get; set; } = string.Empty;
        public string BrochureUrl { get; set; } = string.Empty;
        public string ProjectWebsite { get; set; } = string.Empty;
        public string ProjectVideoUrl { get; set; } = string.Empty;
        public string ProjectHighlights { get; set; } = string.Empty;
        public string ProjectFeatures { get; set; } = string.Empty;
        public string ProjectAmenities { get; set; } = string.Empty;
        public string ProjectSpecifications { get; set; } = string.Empty;
        public string ProjectDocuments { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Tenant? Tenant { get; set; }
        public Developer? Developer { get; set; }
        public ICollection<ProjectMedia> Media { get; set; } = new List<ProjectMedia>();
        public ICollection<UnitType> UnitTypes { get; set; } = new List<UnitType>();
        public ICollection<PropertyListing> Listings { get; set; } = new List<PropertyListing>();
    }
}