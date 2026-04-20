using PropertyPlatform.Core.Entities;

namespace PropertyPlatform.Core.Interfaces
{
    public interface IGamificationService
    {
        Task AwardXPAsync(Guid tenantId, int amount);
        Task<bool> AwardBadgeAsync(Guid tenantId, string badgeCode);
        Task TrackActionAsync(Guid tenantId, string missionCode, int increment = 1);
        Task<GamificationStatusDto> GetStatusAsync(Guid tenantId);
    }

    public class GamificationStatusDto
    {
        public int Level { get; set; }
        public int ExperiencePoints { get; set; }
        public int XPToNextLevel { get; set; }
        public double LevelProgressPercentage { get; set; }
        public List<Badge> RecentBadges { get; set; } = new();
        public List<AgentMissionDto> ActiveMissions { get; set; } = new();
    }

    public class AgentMissionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CurrentProgress { get; set; }
        public int RequirementCount { get; set; }
        public bool IsCompleted { get; set; }
        public double ProgressPercentage => (double)CurrentProgress / RequirementCount * 100;
    }
}
