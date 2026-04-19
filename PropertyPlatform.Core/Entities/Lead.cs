namespace PropertyPlatform.Core.Entities
{
    public class Lead
    {
        public Guid LeadId { get; set; } = Guid.NewGuid();
        public Guid ListingId { get; set; }
        public Guid TenantId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string BuyerEmail { get; set; } = string.Empty;
        public string BuyerPhone { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ContactType { get; set; } = "Form"; // Form, WhatsApp
        public int Score { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public PropertyListing? Listing { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
