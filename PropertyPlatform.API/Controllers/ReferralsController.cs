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
    public class ReferralsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReferralsController(ApplicationDbContext context)
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

        public class CreateReferralRequest
        {
            public Guid NewTenantId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferral([FromBody] CreateReferralRequest request)
        {
            var referrerTenantId = GetCurrentTenantId();

            var referral = new Referral
            {
                ReferrerTenantId = referrerTenantId,
                NewTenantId = request.NewTenantId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Referrals.Add(referral);
            await _context.SaveChangesAsync();

            return Ok(referral);
        }

        [HttpGet]
        public async Task<IActionResult> GetReferrals()
        {
            var tenantId = GetCurrentTenantId();

            var referrals = await _context.Referrals
                .Where(r => r.ReferrerTenantId == tenantId)
                .Include(r => r.NewTenant)
                .ToListAsync();

            return Ok(referrals);
        }
    }
}