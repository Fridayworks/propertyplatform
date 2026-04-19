namespace PropertyPlatform.Core.Entities
{
    public class PropertyListing
    {
        public Guid ListingId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = "Active";
        public string PropertyType { get; set; } = "Condo"; // Condo, Terrace, Semi-D, Landed, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Tenant? Tenant { get; set; }
        public ICollection<PropertyMedia> Media { get; set; } = new List<PropertyMedia>();
        public ICollection<PropertyFeature> Features { get; set; } = new List<PropertyFeature>();
        public ICollection<FloorPlan> FloorPlans { get; set; } = new List<FloorPlan>();
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
        public ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();
        public ListingAnalytic? Analytics { get; set; }
        public FeaturedListing? FeaturedListing { get; set; }
    }
}
