namespace PropertyPlatform.Core.Entities
{
    public class AgentMission
    {
        public Guid AgentMissionId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public Guid MissionId { get; set; }
        public int CurrentProgress { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Navigation
        public Tenant? Tenant { get; set; }
        public Mission? Mission { get; set; }
    }
}
