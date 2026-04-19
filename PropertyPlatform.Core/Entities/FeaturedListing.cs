namespace PropertyPlatform.Core.Entities
{
    public class FeaturedListing
    {
        public Guid FeatureId { get; set; } = Guid.NewGuid();
        public Guid ListingId { get; set; }
        public int BoostLevel { get; set; } = 1; // 1 = standard, 2 = premium, 3 = top
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsActive => DateTime.UtcNow <= EndDate;

        // Navigation
        public PropertyListing? Listing { get; set; }
    }
}
