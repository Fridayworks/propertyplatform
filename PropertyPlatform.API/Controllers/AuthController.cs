using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RegisterRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
        }

        public class RefreshRequest
        {
            public string RefreshToken { get; set; } = string.Empty;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (_context.Tenants.Any(t => t.Email == request.Email))
            {
                return BadRequest("Email already exists");
            }

            var tenant = new Tenant
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            // Create agent profile
            var agentProfile = new AgentProfile
            {
                TenantId = tenant.TenantId,
                Name = request.Name,
                Phone = request.Phone,
                REN_ID = string.Empty, // To be filled later
                OfficeAddress = string.Empty,
                ProfilePhotoUrl = string.Empty,
                CompanyLogoUrl = string.Empty
            };

            _context.AgentProfiles.Add(agentProfile);
            await _context.SaveChangesAsync();

            return Ok("Agent registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var tenant = _context.Tenants.FirstOrDefault(t => t.Email == request.Email);
            if (tenant == null || !BCrypt.Net.BCrypt.Verify(request.Password, tenant.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            var accessToken = GenerateJwtToken(tenant);
            var refreshToken = Guid.NewGuid().ToString("N");

            _context.RefreshTokens.Add(new RefreshToken
            {
                TenantId = tenant.TenantId,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = accessToken,
                refreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var tokenRecord = _context.RefreshTokens
                .Include(t => t.Tenant)
                .FirstOrDefault(t => t.Token == request.RefreshToken && !t.IsRevoked && t.ExpiryDate > DateTime.UtcNow);

            if (tokenRecord == null || tokenRecord.Tenant == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            var newAccessToken = GenerateJwtToken(tokenRecord.Tenant);
            return Ok(new { token = newAccessToken });
        }

        private string GenerateJwtToken(Tenant tenant)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, tenant.TenantId.ToString()),
                new Claim(ClaimTypes.Name, tenant.Email)
            };

            var jwtKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
