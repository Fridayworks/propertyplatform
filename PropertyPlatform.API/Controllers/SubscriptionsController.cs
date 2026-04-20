using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Guid GetCurrentTenantId()
        {
            var tenantIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(tenantIdString, out Guid tenantId))
            {
                return tenantId;
            }
            throw new UnauthorizedAccessException("Invalid tenant");
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscription()
        {
            var tenantId = GetCurrentTenantId();

            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.TenantId == tenantId && s.EndDate > DateTime.UtcNow);

            return Ok(subscription);
        }

        // Admin endpoint to create subscription
        [HttpPost]
        [Authorize] // In real app, check for admin role
        public async Task<IActionResult> CreateSubscription([FromBody] Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok(subscription);
        }
    }
}