// Enterprise-grade rewritten ApplicationDbContext with explicit relationship mapping
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;

namespace PropertyPlatform.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<AgentProfile> AgentProfiles => Set<AgentProfile>();
        public DbSet<PropertyListing> PropertyListings => Set<PropertyListing>();
        public DbSet<PropertyMedia> PropertyMedia => Set<PropertyMedia>();
        public DbSet<PropertyFeature> PropertyFeatures => Set<PropertyFeature>();
        public DbSet<FloorPlan> FloorPlans => Set<FloorPlan>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<FeaturedListing> FeaturedListings => Set<FeaturedListing>();
        public DbSet<ListingAnalytic> ListingAnalytics => Set<ListingAnalytic>();
        public DbSet<UserEvent> UserEvents => Set<UserEvent>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<CreditTransaction> CreditTransactions => Set<CreditTransaction>();
        public DbSet<Referral> Referrals => Set<Referral>();
        public DbSet<Badge> Badges => Set<Badge>();
        public DbSet<AgentBadge> AgentBadges => Set<AgentBadge>();
        public DbSet<Mission> Missions => Set<Mission>();
        public DbSet<AgentMission> AgentMissions => Set<AgentMission>();
        public DbSet<FeatureConfig> FeatureConfigs => Set<FeatureConfig>();
        public DbSet<AgentReview> AgentReviews => Set<AgentReview>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTenantModule(modelBuilder);
            ConfigureListingModule(modelBuilder);
            ConfigureGamificationModule(modelBuilder);
            ConfigureSeedData(modelBuilder);
        }

        private void ConfigureTenantModule(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(x => x.TenantId);
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.UpdatedAt).IsRequired();
            });

            modelBuilder.Entity<AgentProfile>(entity =>
            {
                entity.HasKey(x => x.AgentId);
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Email).IsRequired();
                entity.Property(x => x.PasswordHash).IsRequired();
                entity.Property(x => x.Phone).IsRequired();
                entity.Property(x => x.REN_ID).IsRequired();
                entity.Property(x => x.OfficeAddress).IsRequired();
                entity.Property(x => x.ProfilePhotoUrl).IsRequired();
                entity.Property(x => x.CompanyLogoUrl).IsRequired();
                entity.Property(x => x.Slug).IsRequired();
                entity.Property(x => x.Bio).IsRequired();
                entity.HasIndex(x => x.Slug).IsUnique();

                entity.HasOne(x => x.Tenant)
                      .WithOne(x => x.AgentProfile)
                      .HasForeignKey<AgentProfile>(x => x.TenantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(x => x.SubscriptionId);

                entity.HasOne(x => x.Tenant)
                      .WithMany(x => x.Subscriptions)
                      .HasForeignKey(x => x.TenantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.TokenId);

                entity.HasOne(x => x.Tenant)
                      .WithMany(x => x.RefreshTokens)
                      .HasForeignKey(x => x.TenantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CreditTransaction>(entity =>
            {
                entity.HasKey(x => x.TransactionId);
                entity.Property(x => x.Description).IsRequired();
                entity.Property(x => x.Type).IsRequired();

                entity.HasOne(x => x.Tenant)
                      .WithMany(x => x.CreditTransactions)
                      .HasForeignKey(x => x.TenantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Referral>(entity =>
            {
                entity.HasKey(x => x.ReferralId);

                entity.HasOne(x => x.Referrer)
                      .WithMany(x => x.ReferralsMade)
                      .HasForeignKey(x => x.ReferrerTenantId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.NewTenant)
                      .WithMany(x => x.ReferralsReceived)
                      .HasForeignKey(x => x.NewTenantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureListingModule(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyListing>(entity =>
            {
                entity.HasKey(x => x.ListingId);
                entity.Property(x => x.Title).IsRequired();
                entity.Property(x => x.Description).IsRequired();
                entity.Property(x => x.Location).IsRequired();
                entity.Property(x => x.PropertyType).IsRequired();
                entity.Property(x => x.ListingType).IsRequired();
                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.Price).HasColumnType("numeric");

                entity.HasIndex(x => x.Location);
                entity.HasIndex(x => x.ListingType);

                entity.HasOne(x => x.Tenant)
                      .WithMany(x => x.PropertyListings)
                      .HasForeignKey(x => x.TenantId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PropertyMedia>()
                .HasOne(x => x.Listing)
                .WithMany(x => x.Media)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropertyFeature>()
                .HasOne(x => x.Listing)
                .WithMany(x => x.Features)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FloorPlan>()
                .HasOne(x => x.Listing)
                .WithMany(x => x.FloorPlans)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lead>()
                .HasOne(x => x.Listing)
                .WithMany(x => x.Leads)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FeaturedListing>()
                .HasOne(x => x.Listing)
                .WithOne(x => x.FeaturedListing)
                .HasForeignKey<FeaturedListing>(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ListingAnalytic>()
                .HasOne(x => x.Listing)
                .WithOne(x => x.Analytics)
                .HasForeignKey<ListingAnalytic>(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEvent>()
                .HasOne(x => x.Listing)
                .WithMany(x => x.UserEvents)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureGamificationModule(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Badge>().HasKey(x => x.BadgeId);
            modelBuilder.Entity<Mission>().HasKey(x => x.MissionId);
            modelBuilder.Entity<FeatureConfig>().HasKey(x => x.FeatureConfigId);

            modelBuilder.Entity<AgentBadge>()
                .HasOne(x => x.Badge)
                .WithMany()
                .HasForeignKey(x => x.BadgeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AgentBadge>()
                .HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AgentMission>()
                .HasOne(x => x.Mission)
                .WithMany()
                .HasForeignKey(x => x.MissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AgentMission>()
                .HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureSeedData(ModelBuilder modelBuilder)
        {
            var adminTenantId = new Guid("00000000-0000-0000-0000-000000000100");
            var adminAgentId = new Guid("00000000-0000-0000-0000-000000000101");

            modelBuilder.Entity<Tenant>().HasData(
                new Tenant
                {
                    TenantId = adminTenantId,
                    Name = "Platform Administration",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<AgentProfile>().HasData(
                new AgentProfile
                {
                    AgentId = adminAgentId,
                    TenantId = adminTenantId,
                    Name = "Platform Admin",
                    Email = "admin",
                    PasswordHash = "$2a$11$pxNvtTsemAwY0LSFV3JE6.k2WEG1OzVBkbMHIOs3rsQmUDrTV2.r.", // 123456$$
                    Phone = "000-000-0000",
                    REN_ID = "ADMIN-001",
                    OfficeAddress = "System HQ",
                    ProfilePhotoUrl = "https://via.placeholder.com/150",
                    CompanyLogoUrl = "https://via.placeholder.com/150",
                    Slug = "admin",
                    Bio = "System Administrator"
                }
            );

            modelBuilder.Entity<Badge>().HasData(
                new Badge { BadgeId = new Guid("00000000-0000-0000-0000-000000000001"), Code = "RISING_STAR", Name = "Rising Star", Description = "Joined the platform and completed initial setup.", IconUrl = "✨" },
                new Badge { BadgeId = new Guid("00000000-0000-0000-0000-000000000002"), Code = "ELITE_LISTER", Name = "Elite Lister", Description = "Uploaded 10 high-quality listings.", IconUrl = "🏆" }
            );

            modelBuilder.Entity<FeatureConfig>().HasData(
                new FeatureConfig
                {
                    FeatureConfigId = new Guid("00000000-0000-0000-0000-000000000201"),
                    FeatureKey = "sale",
                    DisplayName = "Property Sale",
                    Description = "Allow agents to create listings for property sales.",
                    Category = "ListingType",
                    IsEnabled = true,
                    SortOrder = 1,
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new FeatureConfig
                {
                    FeatureConfigId = new Guid("00000000-0000-0000-0000-000000000202"),
                    FeatureKey = "rent",
                    DisplayName = "Property Rental",
                    Description = "Allow agents to create listings for residential or commercial rentals.",
                    Category = "ListingType",
                    IsEnabled = true,
                    SortOrder = 2,
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new FeatureConfig
                {
                    FeatureConfigId = new Guid("00000000-0000-0000-0000-000000000203"),
                    FeatureKey = "new-project",
                    DisplayName = "New Projects",
                    Description = "Allow agents to create listings for new developer projects.",
                    Category = "ListingType",
                    IsEnabled = true,
                    SortOrder = 3,
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}