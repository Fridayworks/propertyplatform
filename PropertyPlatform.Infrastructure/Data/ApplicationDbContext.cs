using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using System.Security.Claims;

namespace PropertyPlatform.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly Guid _currentTenantId;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            var userIdString = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out Guid tenantId))
            {
                _currentTenantId = tenantId;
            }
        }

        // Core
        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<AgentProfile> AgentProfiles { get; set; } = null!;
        public DbSet<PropertyListing> PropertyListings { get; set; } = null!;
        public DbSet<PropertyMedia> PropertyMedia { get; set; } = null!;
        public DbSet<Lead> Leads { get; set; } = null!;

        // Phase 3
        public DbSet<PropertyFeature> PropertyFeatures { get; set; } = null!;
        public DbSet<FloorPlan> FloorPlans { get; set; } = null!;
        public DbSet<ListingAnalytic> ListingAnalytics { get; set; } = null!;
        public DbSet<UserEvent> UserEvents { get; set; } = null!;
        public DbSet<Referral> Referrals { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<FeaturedListing> FeaturedListings { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filters for multi-tenancy
            modelBuilder.Entity<AgentProfile>().HasQueryFilter(e => _currentTenantId == Guid.Empty || e.TenantId == _currentTenantId);
            modelBuilder.Entity<PropertyListing>().HasQueryFilter(e => _currentTenantId == Guid.Empty || e.TenantId == _currentTenantId);
            modelBuilder.Entity<Lead>().HasQueryFilter(e => _currentTenantId == Guid.Empty || e.TenantId == _currentTenantId);

            // Primary Keys
            modelBuilder.Entity<AgentProfile>().HasKey(a => a.AgentId);
            modelBuilder.Entity<PropertyListing>().HasKey(l => l.ListingId);
            modelBuilder.Entity<PropertyMedia>().HasKey(m => m.MediaId);
            modelBuilder.Entity<Lead>().HasKey(l => l.LeadId);
            modelBuilder.Entity<PropertyFeature>().HasKey(f => f.FeatureId);
            modelBuilder.Entity<FloorPlan>().HasKey(f => f.FloorPlanId);
            modelBuilder.Entity<ListingAnalytic>().HasKey(a => a.AnalyticsId);
            modelBuilder.Entity<UserEvent>().HasKey(e => e.EventId);
            modelBuilder.Entity<Referral>().HasKey(r => r.ReferralId);
            modelBuilder.Entity<Subscription>().HasKey(s => s.SubscriptionId);
            modelBuilder.Entity<FeaturedListing>().HasKey(f => f.FeatureId);
            modelBuilder.Entity<RefreshToken>().HasKey(t => t.TokenId);

            // Tenant → AgentProfile (1:1)
            modelBuilder.Entity<Tenant>()
                .HasOne(t => t.AgentProfile)
                .WithOne(a => a.Tenant)
                .HasForeignKey<AgentProfile>(a => a.TenantId);

            // Tenant → Listings (1:N)
            modelBuilder.Entity<Tenant>()
                .HasMany(t => t.PropertyListings)
                .WithOne(p => p.Tenant)
                .HasForeignKey(p => p.TenantId);

            // Listing → Media (1:N)
            modelBuilder.Entity<PropertyListing>()
                .HasMany(l => l.Media)
                .WithOne(m => m.Listing)
                .HasForeignKey(m => m.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Listing → Features (1:N)
            modelBuilder.Entity<PropertyListing>()
                .HasMany(l => l.Features)
                .WithOne(f => f.Listing)
                .HasForeignKey(f => f.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Listing → FloorPlans (1:N)
            modelBuilder.Entity<PropertyListing>()
                .HasMany(l => l.FloorPlans)
                .WithOne(f => f.Listing)
                .HasForeignKey(f => f.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Listing → Leads (1:N)
            modelBuilder.Entity<PropertyListing>()
                .HasMany(l => l.Leads)
                .WithOne(lead => lead.Listing)
                .HasForeignKey(lead => lead.ListingId);

            modelBuilder.Entity<Lead>()
                .HasOne(lead => lead.Tenant)
                .WithMany()
                .HasForeignKey(lead => lead.TenantId);

            // Listing → Analytics (1:1)
            modelBuilder.Entity<PropertyListing>()
                .HasOne(l => l.Analytics)
                .WithOne(a => a.Listing)
                .HasForeignKey<ListingAnalytic>(a => a.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Listing → FeaturedListing (1:1)
            modelBuilder.Entity<PropertyListing>()
                .HasOne(l => l.FeaturedListing)
                .WithOne(f => f.Listing)
                .HasForeignKey<FeaturedListing>(f => f.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Listing → UserEvents (1:N)
            modelBuilder.Entity<PropertyListing>()
                .HasMany(l => l.UserEvents)
                .WithOne(e => e.Listing)
                .HasForeignKey(e => e.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Referral
            modelBuilder.Entity<Referral>()
                .HasOne(r => r.Referrer)
                .WithMany()
                .HasForeignKey(r => r.ReferrerTenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Referral>()
                .HasOne(r => r.NewTenant)
                .WithMany()
                .HasForeignKey(r => r.NewTenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subscription
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Tenant)
                .WithMany()
                .HasForeignKey(s => s.TenantId);

            // RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasOne(t => t.Tenant)
                .WithMany()
                .HasForeignKey(t => t.TenantId);

            // SQLite decimal fix
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                modelBuilder.Entity<PropertyListing>()
                    .Property(p => p.Price)
                    .HasConversion<double>();
            }

            // Indexes
            modelBuilder.Entity<PropertyListing>().HasIndex(l => l.TenantId);
            modelBuilder.Entity<PropertyListing>().HasIndex(l => l.Location);
            modelBuilder.Entity<UserEvent>().HasIndex(e => e.ListingId);
        }
    }
}
