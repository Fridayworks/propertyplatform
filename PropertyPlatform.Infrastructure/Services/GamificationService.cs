using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Infrastructure.Services
{
    public class GamificationService : IGamificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWalletService _walletService;

        public GamificationService(ApplicationDbContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        public async Task AwardXPAsync(Guid tenantId, int amount)
        {
            var profile = await _context.AgentProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.TenantId == tenantId);

            if (profile == null) return;

            profile.ExperiencePoints += amount;

            // Simple leveling formula: Level = sqrt(XP / 100) + 1
            // e.g. Level 1: 0 XP, Level 2: 100 XP, Level 3: 400 XP, Level 4: 900 XP
            int newLevel = (int)Math.Sqrt(profile.ExperiencePoints / 100.0) + 1;
            
            if (newLevel > profile.Level)
            {
                profile.Level = newLevel;
                // Optionally award bonus credits for leveling up
                await _walletService.AddCreditsAsync(tenantId, 10 * newLevel, "Reward", $"Level Up Bonus: Reached Level {newLevel}");
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> AwardBadgeAsync(Guid tenantId, string badgeCode)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(b => b.Code == badgeCode);
            if (badge == null) return false;

            var existing = await _context.AgentBadges.AnyAsync(ab => ab.TenantId == tenantId && ab.BadgeId == badge.BadgeId);
            if (existing) return false;

            _context.AgentBadges.Add(new AgentBadge
            {
                TenantId = tenantId,
                BadgeId = badge.BadgeId
            });

            await AwardXPAsync(tenantId, 50); // Badges give bonus XP
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task TrackActionAsync(Guid tenantId, string missionCode, int increment = 1)
        {
            var mission = await _context.Missions.FirstOrDefaultAsync(m => m.Code == missionCode);
            if (mission == null) return;

            var agentMission = await _context.AgentMissions
                .FirstOrDefaultAsync(am => am.TenantId == tenantId && am.MissionId == mission.MissionId);

            if (agentMission == null)
            {
                agentMission = new AgentMission
                {
                    TenantId = tenantId,
                    MissionId = mission.MissionId,
                    CurrentProgress = 0,
                    IsCompleted = false
                };
                _context.AgentMissions.Add(agentMission);
            }

            if (agentMission.IsCompleted) return;

            agentMission.CurrentProgress += increment;

            if (agentMission.CurrentProgress >= mission.RequirementCount)
            {
                agentMission.IsCompleted = true;
                agentMission.CompletedAt = DateTime.UtcNow;

                // Award rewards
                if (mission.CreditReward > 0)
                {
                    await _walletService.AddCreditsAsync(tenantId, mission.CreditReward, "Reward", $"Completed Mission: {mission.Title}");
                }
                if (mission.XPReward > 0)
                {
                    await AwardXPAsync(tenantId, mission.XPReward);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<GamificationStatusDto> GetStatusAsync(Guid tenantId)
        {
            var profile = await _context.AgentProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.TenantId == tenantId);

            if (profile == null) return new GamificationStatusDto();

            int nextLevel = profile.Level + 1;
            int currentLevelXP = (int)Math.Pow(profile.Level - 1, 2) * 100;
            int nextLevelXP = (int)Math.Pow(nextLevel - 1, 2) * 100;
            
            int xpInCurrentLevel = profile.ExperiencePoints - currentLevelXP;
            int xpRequiredForLevel = nextLevelXP - currentLevelXP;

            var recentBadges = await _context.AgentBadges
                .Where(ab => ab.TenantId == tenantId)
                .OrderByDescending(ab => ab.AwardedAt)
                .Take(5)
                .Select(ab => ab.Badge)
                .ToListAsync();

            var activeMissions = await _context.AgentMissions
                .Include(am => am.Mission)
                .Where(am => am.TenantId == tenantId && !am.IsCompleted)
                .Select(am => new AgentMissionDto
                {
                    Title = am.Mission!.Title,
                    Description = am.Mission!.Description,
                    CurrentProgress = am.CurrentProgress,
                    RequirementCount = am.Mission!.RequirementCount,
                    IsCompleted = am.IsCompleted
                })
                .ToListAsync();

            return new GamificationStatusDto
            {
                Level = profile.Level,
                ExperiencePoints = profile.ExperiencePoints,
                XPToNextLevel = nextLevelXP - profile.ExperiencePoints,
                LevelProgressPercentage = Math.Min(100, (double)xpInCurrentLevel / xpRequiredForLevel * 100),
                RecentBadges = recentBadges!,
                ActiveMissions = activeMissions
            };
        }
    }
}
