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
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
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

        public class CreateProjectRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string State { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? CompletionDate { get; set; }
            public string Status { get; set; } = "Active";
            public string ProjectType { get; set; } = "Residential";
            public string DeveloperName { get; set; } = string.Empty;
            public string DeveloperContact { get; set; } = string.Empty;
            public string DeveloperEmail { get; set; } = string.Empty;
            public string DeveloperPhone { get; set; } = string.Empty;
            public string DeveloperWebsite { get; set; } = string.Empty;
            public string DeveloperLogoUrl { get; set; } = string.Empty;
            public string BrochureUrl { get; set; } = string.Empty;
            public string ProjectWebsite { get; set; } = string.Empty;
            public string ProjectVideoUrl { get; set; } = string.Empty;
            public string ProjectHighlights { get; set; } = string.Empty;
            public string ProjectFeatures { get; set; } = string.Empty;
            public string ProjectAmenities { get; set; } = string.Empty;
            public string ProjectSpecifications { get; set; } = string.Empty;
            public string ProjectDocuments { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var tenantId = GetCurrentTenantId();

            var project = new Project
            {
                TenantId = tenantId,
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Country = request.Country,
                PostalCode = request.PostalCode,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                StartDate = request.StartDate,
                CompletionDate = request.CompletionDate,
                Status = request.Status,
                ProjectType = request.ProjectType,
                DeveloperName = request.DeveloperName,
                DeveloperContact = request.DeveloperContact,
                DeveloperEmail = request.DeveloperEmail,
                DeveloperPhone = request.DeveloperPhone,
                DeveloperWebsite = request.DeveloperWebsite,
                DeveloperLogoUrl = request.DeveloperLogoUrl,
                BrochureUrl = request.BrochureUrl,
                ProjectWebsite = request.ProjectWebsite,
                ProjectVideoUrl = request.ProjectVideoUrl,
                ProjectHighlights = request.ProjectHighlights,
                ProjectFeatures = request.ProjectFeatures,
                ProjectAmenities = request.ProjectAmenities,
                ProjectSpecifications = request.ProjectSpecifications,
                ProjectDocuments = request.ProjectDocuments,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var tenantId = GetCurrentTenantId();

            var projects = await _context.Projects
                .Where(p => p.TenantId == tenantId)
                .ToListAsync();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id && p.TenantId == tenantId);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] CreateProjectRequest request)
        {
            var tenantId = GetCurrentTenantId();

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id && p.TenantId == tenantId);

            if (project == null)
            {
                return NotFound();
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.Location = request.Location;
            project.Address = request.Address;
            project.City = request.City;
            project.State = request.State;
            project.Country = request.Country;
            project.PostalCode = request.PostalCode;
            project.Latitude = request.Latitude;
            project.Longitude = request.Longitude;
            project.StartDate = request.StartDate;
            project.CompletionDate = request.CompletionDate;
            project.Status = request.Status;
            project.ProjectType = request.ProjectType;
            project.DeveloperName = request.DeveloperName;
            project.DeveloperContact = request.DeveloperContact;
            project.DeveloperEmail = request.DeveloperEmail;
            project.DeveloperPhone = request.DeveloperPhone;
            project.DeveloperWebsite = request.DeveloperWebsite;
            project.DeveloperLogoUrl = request.DeveloperLogoUrl;
            project.BrochureUrl = request.BrochureUrl;
            project.ProjectWebsite = request.ProjectWebsite;
            project.ProjectVideoUrl = request.ProjectVideoUrl;
            project.ProjectHighlights = request.ProjectHighlights;
            project.ProjectFeatures = request.ProjectFeatures;
            project.ProjectAmenities = request.ProjectAmenities;
            project.ProjectSpecifications = request.ProjectSpecifications;
            project.ProjectDocuments = request.ProjectDocuments;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var tenantId = GetCurrentTenantId();

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id && p.TenantId == tenantId);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}