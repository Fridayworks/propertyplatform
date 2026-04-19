namespace PropertyPlatform.Core.Entities
{
    public class UserEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }
        public Guid ListingId { get; set; }
        public string EventType { get; set; } = string.Empty; // VIEW, CLICK, CONTACT
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public PropertyListing? Listing { get; set; }
    }
}
