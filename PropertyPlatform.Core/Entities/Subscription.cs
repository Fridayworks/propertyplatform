namespace PropertyPlatform.Core.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Subscription
    {
        [Key]
        public Guid SubscriptionId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public string Plan { get; set; } = "Basic"; // Basic, Pro, Premium
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddMonths(1);
        public bool IsActive => DateTime.UtcNow <= EndDate;

        // Navigation
        public Tenant? Tenant { get; set; }
    }
}
