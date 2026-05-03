using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;
using PropertyPlatform.Infrastructure.Services;

namespace PropertyPlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseProvider = configuration["DatabaseProvider"] ?? "PostgreSQL";
            
            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                if (databaseProvider.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
                {
                    var connectionString = configuration.GetConnectionString("SqliteConnection") 
                                           ?? configuration.GetConnectionString("DefaultConnection");
                    options.UseSqlite(connectionString);
                }
                else
                {
                    var connectionString = configuration.GetConnectionString("PostgreSqlConnection") 
                                           ?? configuration.GetConnectionString("DefaultConnection");
                    options.UseNpgsql(connectionString);
                }
            });
            
            // Add a factory or interceptor to set TenantId if we needed, but 
            // since DbContext is scoped, it's better to just pass it or resolve it inside DbContext.
            // Let's keep it simple for MVP: controllers/pages will pass the TenantId explicitly where needed,
            // or we'll update ApplicationDbContext to accept IHttpContextAccessor.

            services.AddScoped<IRecommendationService, RecommendationService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IGamificationService, GamificationService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IBrochureIngestionService, BrochureIngestionService>();
            services.AddScoped<IAISmartListingService, AISmartListingService>();

            return services;
        }

        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            if (context.Database.IsSqlite())
            {
                context.Database.EnsureCreated();
            }
            else
            {
                context.Database.Migrate();
            }

            return app;
        }
    }
}
