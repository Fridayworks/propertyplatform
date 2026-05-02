using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class UnitType
    {
        public Guid UnitTypeId { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = "Apartment"; // Apartment, Condo, Townhouse, Duplex, etc.
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal Area { get; set; }
        public string AreaUnit { get; set; } = "sqft"; // sqft, sqm, acres
        public decimal Price { get; set; }
        public decimal PricePerSqft { get; set; }
        public string Status { get; set; } = "Active";
        public string Features { get; set; } = string.Empty;
        public string Amenities { get; set; } = string.Empty;
        public string FloorPlan { get; set; } = string.Empty;
        public string View { get; set; } = string.Empty;
        public string Orientation { get; set; } = string.Empty;
        public string Parking { get; set; } = string.Empty;
        public string Balcony { get; set; } = string.Empty;
        public string Terrace { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
        public string Security { get; set; } = string.Empty;
        public string Utilities { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Project? Project { get; set; }
        public ICollection<UnitTypeMedia> Media { get; set; } = new List<UnitTypeMedia>();
        public ICollection<PropertyListing> Listings { get; set; } = new List<PropertyListing>();
    }
}