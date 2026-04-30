namespace PropertyPlatform.Core.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class PropertyFeature
    {
        [Key]
        public Guid FeatureId { get; set; } = Guid.NewGuid();
        public Guid ListingId { get; set; }
        public string Key { get; set; } = string.Empty;   // e.g. "Bedrooms", "Bathrooms", "Tenure"
        public string Value { get; set; } = string.Empty; // e.g. "3", "2", "Freehold"

        // Navigation
        public PropertyListing? Listing { get; set; }
    }
}
