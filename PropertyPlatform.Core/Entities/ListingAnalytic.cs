namespace PropertyPlatform.Core.Entities
{
    public class ListingAnalytic
    {
        public Guid AnalyticsId { get; set; } = Guid.NewGuid();
        public Guid ListingId { get; set; }
        public int Views { get; set; } = 0;
        public int Clicks { get; set; } = 0;
        public int Contacts { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public PropertyListing? Listing { get; set; }
    }
}
