using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/cms")]
    public class CMSController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CMSController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // --- Public Endpoints ---

        [HttpGet("articles")]
        public async Task<IActionResult> GetArticles([FromQuery] string? category = null)
        {
            var query = _context.Articles
                .Where(a => a.Status == "Published")
                .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt);

            if (!string.IsNullOrEmpty(category))
            {
                query = (IOrderedQueryable<Article>)query.Where(a => a.Category == category);
            }

            var articles = await query.ToListAsync();
            return Ok(articles);
        }

        [HttpGet("articles/{slug}")]
        public async Task<IActionResult> GetArticleBySlug(string slug)
        {
            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.Slug == slug && a.Status == "Published");

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // --- Admin Endpoints ---

        [HttpGet("admin/articles")]
        [Authorize]
        public async Task<IActionResult> GetAllArticles()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            var articles = await _context.Articles
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return Ok(articles);
        }

        [HttpPost("admin/articles")]
        [Authorize]
        public async Task<IActionResult> CreateArticle([FromBody] Article article)
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            article.ArticleId = Guid.NewGuid();
            article.CreatedAt = DateTime.UtcNow;
            article.UpdatedAt = DateTime.UtcNow;
            
            if (article.Status == "Published" && !article.PublishedAt.HasValue)
            {
                article.PublishedAt = DateTime.UtcNow;
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticleBySlug), new { slug = article.Slug }, article);
        }

        [HttpPut("admin/articles/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(Guid id, [FromBody] Article articleUpdate)
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            article.Title = articleUpdate.Title;
            article.Slug = articleUpdate.Slug;
            article.Excerpt = articleUpdate.Excerpt;
            article.Content = articleUpdate.Content;
            article.Author = articleUpdate.Author;
            article.ThumbnailUrl = articleUpdate.ThumbnailUrl;
            article.Category = articleUpdate.Category;
            
            if (article.Status != "Published" && articleUpdate.Status == "Published")
            {
                article.PublishedAt = DateTime.UtcNow;
            }
            
            article.Status = articleUpdate.Status;
            article.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(article);
        }

        [HttpDelete("admin/articles/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
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
