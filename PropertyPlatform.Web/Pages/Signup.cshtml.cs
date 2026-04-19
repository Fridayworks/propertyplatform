using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages
{
    public class SignupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SignupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;
        
        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Phone { get; set; } = string.Empty;

        [BindProperty]
        public string REN_ID { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public Guid? ReferrerId { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet(Guid? referrer)
        {
            if (referrer.HasValue) ReferrerId = referrer;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existingTenant = _context.Tenants.FirstOrDefault(t => t.Email == Email);
            if (existingTenant != null)
            {
                ErrorMessage = "Email already in use.";
                return Page();
            }

            var tenant = new Tenant
            {
                Email = Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password)
            };

            var agentProfile = new AgentProfile
            {
                TenantId = tenant.TenantId,
                Name = Name,
                Phone = Phone,
                REN_ID = REN_ID
            };

            _context.Tenants.Add(tenant);
            _context.AgentProfiles.Add(agentProfile);

            if (ReferrerId.HasValue && _context.Tenants.Any(t => t.TenantId == ReferrerId.Value))
            {
                _context.Referrals.Add(new Referral
                {
                    ReferrerTenantId = ReferrerId.Value,
                    NewTenantId = tenant.TenantId
                });

                // Reward referrer with 50 credits
                var referrerProfile = _context.AgentProfiles.FirstOrDefault(p => p.TenantId == ReferrerId.Value);
                if (referrerProfile != null)
                {
                    referrerProfile.Credits += 50;
                }
            }

            await _context.SaveChangesAsync();

            // Auto log in after signup
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, tenant.TenantId.ToString()),
                new Claim(ClaimTypes.Name, tenant.Email)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return RedirectToPage("/Agents");
        }
    }
}
