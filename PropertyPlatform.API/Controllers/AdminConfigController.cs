using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/admin/config")]
    [Authorize]
    public class AdminConfigController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminConfigController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public class UpdateFeatureRequest
        {
            public string FeatureKey { get; set; } = string.Empty;
            public bool IsEnabled { get; set; }
        }

        [HttpPut("features")]
        public async Task<IActionResult> UpdateFeatures([FromBody] List<UpdateFeatureRequest> request)
        {
            if (!IsCurrentUserAdmin())
            {
                return Forbid();
            }

            var requestedKeys = request.Select(r => r.FeatureKey).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var configs = await _context.FeatureConfigs
                .Where(f => requestedKeys.Contains(f.FeatureKey))
                .ToListAsync();

            foreach (var config in configs)
            {
                var update = request.First(r => string.Equals(r.FeatureKey, config.FeatureKey, StringComparison.OrdinalIgnoreCase));
                config.IsEnabled = update.IsEnabled;
                config.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok(configs.OrderBy(f => f.SortOrder));
        }

        private bool IsCurrentUserAdmin()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(email)) return false;

            var adminEmails = _configuration.GetSection("Admin:Emails").Get<string[]>() ?? [];
            return adminEmails.Any(admin => string.Equals(admin, email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
