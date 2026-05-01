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
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<AdminRole> AdminRoles => Set<AdminRole>();
        public DbSet<DynamicMenu> DynamicMenus => Set<DynamicMenu>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTenantModule(modelBuilder);
            ConfigureListingModule(modelBuilder);
            ConfigureGamificationModule(modelBuilder);
            ConfigureCMSModule(modelBuilder);
            ConfigureGovernanceModule(modelBuilder);
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

        private void ConfigureCMSModule(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(x => x.ArticleId);
                entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Slug).IsRequired().HasMaxLength(250);
                entity.Property(x => x.Content).IsRequired();
                entity.Property(x => x.Status).IsRequired().HasMaxLength(20);

                entity.HasIndex(x => x.Slug).IsUnique();
                entity.HasIndex(x => x.Status);
                entity.HasIndex(x => x.Category);
            });
        }

        private void ConfigureGovernanceModule(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRole>(entity =>
            {
                entity.HasKey(x => x.RoleId);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<DynamicMenu>(entity =>
            {
                entity.HasKey(x => x.MenuId);
                entity.Property(x => x.Title).IsRequired().HasMaxLength(100);
                entity.Property(x => x.Url).IsRequired().HasMaxLength(255);
                entity.Property(x => x.Location).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<AgentProfile>()
                .HasOne(x => x.AdminRole)
                .WithMany()
                .HasForeignKey(x => x.AdminRoleId)
                .OnDelete(DeleteBehavior.SetNull);
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
                    Bio = "System Administrator",
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed Admin Roles
            var superAdminRoleId = new Guid("00000000-0000-0000-0000-000000000401");
            var contentEditorRoleId = new Guid("00000000-0000-0000-0000-000000000402");

            modelBuilder.Entity<AdminRole>().HasData(
                new AdminRole
                {
                    RoleId = superAdminRoleId,
                    Name = "Super Admin",
                    Description = "Full access to all modules and configurations.",
                    Permissions = "CMS.Manage,Users.Manage,Roles.Manage,Menus.Manage,Config.Manage",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new AdminRole
                {
                    RoleId = contentEditorRoleId,
                    Name = "Content Editor",
                    Description = "Access to manage CMS articles and news only.",
                    Permissions = "CMS.Manage",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Assign Super Admin role to the seeded admin user (if we wanted to hardlink, but for now we rely on config or we can just set it here)
            // Note: The agent profile is already seeded, so we'd need to modify the seeded AgentProfile. 
            // We'll just leave AdminRoleId null for the seeded user to avoid primary key conflicts in HasData. 
            // Actually, we can update the seeded Admin AgentProfile:

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

            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    ArticleId = new Guid("00000000-0000-0000-0000-000000000301"),
                    Title = "Welcome to the New Property Platform",
                    Slug = "welcome-to-new-platform",
                    Excerpt = "We are excited to announce the launch of our next-generation property ecosystem.",
                    Content = "<p>Welcome to the <strong>Annie Rustic Property Platform</strong>. We have completely rewritten our core engine to provide a seamless, AI-powered experience for agents and buyers alike.</p><p>Explore our new features including AI-driven listing automation, smart lead management, and premium agent microsites.</p>",
                    Author = "System Admin",
                    Category = "Company News",
                    Status = "Published",
                    PublishedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ThumbnailUrl = "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?auto=format&fit=crop&q=80&w=800"
                },
                new Article
                {
                    ArticleId = new Guid("00000000-0000-0000-0000-000000000302"),
                    Title = "Market Trends: Q1 2026 Residential Outlook",
                    Slug = "market-trends-q1-2026",
                    Excerpt = "Analyzing the shift in urban living preferences and price stability in major metropolitan areas.",
                    Content = "<p>The residential market in early 2026 shows a strong preference for mixed-use developments and smart home integrations.</p><p>Our data indicates a 15% increase in searches for properties with dedicated home office spaces and high-speed fiber connectivity.</p>",
                    Author = "Market Analytics Team",
                    Category = "Market Trends",
                    Status = "Published",
                    PublishedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    ThumbnailUrl = "https://images.unsplash.com/photo-1460472178825-e52506b3f90a?auto=format&fit=crop&q=80&w=800"
                }
            );

            // Seed Dynamic Menus
            modelBuilder.Entity<DynamicMenu>().HasData(
                new DynamicMenu
                {
                    MenuId = new Guid("00000000-0000-0000-0000-000000000501"),
                    Title = "Home",
                    Url = "/",
                    Location = "Main",
                    SortOrder = 1,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new DynamicMenu
                {
                    MenuId = new Guid("00000000-0000-0000-0000-000000000502"),
                    Title = "Find a Home",
                    Url = "/Search",
                    Location = "Main",
                    SortOrder = 2,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new DynamicMenu
                {
                    MenuId = new Guid("00000000-0000-0000-0000-000000000503"),
                    Title = "News",
                    Url = "/News",
                    Location = "Main",
                    SortOrder = 3,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}