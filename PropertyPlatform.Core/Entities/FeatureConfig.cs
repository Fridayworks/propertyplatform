namespace PropertyPlatform.Core.Entities
{
    public class FeatureConfig
    {
        public Guid FeatureConfigId { get; set; } = Guid.NewGuid();
        public string FeatureKey { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "ListingType";
        public bool IsEnabled { get; set; } = true;
        public int SortOrder { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
