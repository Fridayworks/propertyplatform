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
    public class DevelopersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevelopersController(ApplicationDbContext context)
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

        public class CreateDeveloperRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string ContactPerson { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Website { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string State { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            public string LogoUrl { get; set; } = string.Empty;
            public string LicenseNumber { get; set; } = string.Empty;
            public string RegistrationNumber { get; set; } = string.Empty;
            public string YearsOfOperation { get; set; } = string.Empty;
            public string Awards { get; set; } = string.Empty;
            public string ProjectsCompleted { get; set; } = string.Empty;
            public string Certifications { get; set; } = string.Empty;
            public string CompanyProfile { get; set; } = string.Empty;
            public string ProjectPortfolio { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeveloper([FromBody] CreateDeveloperRequest request)
        {
            var tenantId = GetCurrentTenantId();

            var developer = new Developer
            {
                TenantId = tenantId,
                Name = request.Name,
                Description = request.Description,
                ContactPerson = request.ContactPerson,
                Email = request.Email,
                Phone = request.Phone,
                Website = request.Website,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Country = request.Country,
                PostalCode = request.PostalCode,
                LogoUrl = request.LogoUrl,
                LicenseNumber = request.LicenseNumber,
                RegistrationNumber = request.RegistrationNumber,
                YearsOfOperation = request.YearsOfOperation,
                Awards = request.Awards,
                ProjectsCompleted = request.ProjectsCompleted,
                Certifications = request.Certifications,
                CompanyProfile = request.CompanyProfile,
                ProjectPortfolio = request.ProjectPortfolio,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Developers.Add(developer);
            await _context.SaveChangesAsync();

            return Ok(developer);
        }

        [HttpGet]
        public async Task<IActionResult> GetDevelopers()
        {
            var tenantId = GetCurrentTenantId();

            var developers = await _context.Developers
                .Where(d => d.TenantId == tenantId)
                .ToListAsync();

            return Ok(developers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeveloper(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var developer = await _context.Developers
                .FirstOrDefaultAsync(d => d.DeveloperId == id && d.TenantId == tenantId);

            if (developer == null)
            {
                return NotFound();
            }

            return Ok(developer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeveloper(Guid id, [FromBody] CreateDeveloperRequest request)
        {
            var tenantId = GetCurrentTenantId();

            var developer = await _context.Developers
                .FirstOrDefaultAsync(d => d.DeveloperId == id && d.TenantId == tenantId);

            if (developer == null)
            {
                return NotFound();
            }

            developer.Name = request.Name;
            developer.Description = request.Description;
            developer.ContactPerson = request.ContactPerson;
            developer.Email = request.Email;
            developer.Phone = request.Phone;
            developer.Website = request.Website;
            developer.Address = request.Address;
            developer.City = request.City;
            developer.State = request.State;
            developer.Country = request.Country;
            developer.PostalCode = request.PostalCode;
            developer.LogoUrl = request.LogoUrl;
            developer.LicenseNumber = request.LicenseNumber;
            developer.RegistrationNumber = request.RegistrationNumber;
            developer.YearsOfOperation = request.YearsOfOperation;
            developer.Awards = request.Awards;
            developer.ProjectsCompleted = request.ProjectsCompleted;
            developer.Certifications = request.Certifications;
            developer.CompanyProfile = request.CompanyProfile;
            developer.ProjectPortfolio = request.ProjectPortfolio;
            developer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(developer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeveloper(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var developer = await _context.Developers
                .FirstOrDefaultAsync(d => d.DeveloperId == id && d.TenantId == tenantId);

            if (developer == null)
            {
                return NotFound();
            }

            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}