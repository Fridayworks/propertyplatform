namespace PropertyPlatform.Core.Entities
{
    public class Badge
    {
        public Guid BadgeId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty; // e.g. "WELCOME", "ELITE_LISTER"
    }
}
