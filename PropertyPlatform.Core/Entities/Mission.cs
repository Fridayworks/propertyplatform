namespace PropertyPlatform.Core.Entities
{
    public class Mission
    {
        public Guid MissionId { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty; // e.g. "UPLOAD_5_LISTINGS"
        public int RequirementCount { get; set; } // e.g. 5
        public int CreditReward { get; set; }
        public int XPReward { get; set; }
    }
}
