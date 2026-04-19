namespace PropertyPlatform.Core.Entities
{
    public class FloorPlan
    {
        public Guid FloorPlanId { get; set; } = Guid.NewGuid();
        public Guid ListingId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation
        public PropertyListing? Listing { get; set; }
    }
}
