using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using System.Security.Claims;

namespace PropertyPlatform.Web.Pages.Admin.CMS
{
    public class EditorModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EditorModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public Article Article { get; set; } = new();

        public string Mode { get; set; } = "Create";

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (!IsCurrentUserAdmin()) return RedirectToPage("/Login");

            if (id.HasValue)
            {
                var existing = await _context.Articles.FindAsync(id.Value);
                if (existing == null) return NotFound();
                Article = existing;
                Mode = "Edit";
            }
            else
            {
                Article = new Article
                {
                    Status = "Draft",
                    Author = User.FindFirstValue(ClaimTypes.GivenName) ?? "Admin"
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            if (!ModelState.IsValid)
            {
                Mode = Article.ArticleId == Guid.Empty ? "Create" : "Edit";
                return Page();
            }

            if (Article.ArticleId == Guid.Empty || !_context.Articles.Any(a => a.ArticleId == Article.ArticleId))
            {
                // Create
                Article.ArticleId = Guid.NewGuid();
                Article.CreatedAt = DateTime.UtcNow;
                Article.UpdatedAt = DateTime.UtcNow;
                
                if (Article.Status == "Published")
                {
                    Article.PublishedAt = DateTime.UtcNow;
                }

                _context.Articles.Add(Article);
            }
            else
            {
                // Update
                var existing = await _context.Articles.FindAsync(Article.ArticleId);
                if (existing == null) return NotFound();

                existing.Title = Article.Title;
                existing.Slug = Article.Slug;
                existing.Excerpt = Article.Excerpt;
                existing.Content = Article.Content;
                existing.Author = Article.Author;
                existing.ThumbnailUrl = Article.ThumbnailUrl;
                existing.Category = Article.Category;
                
                if (existing.Status != "Published" && Article.Status == "Published")
                {
                    existing.PublishedAt = DateTime.UtcNow;
                }
                
                existing.Status = Article.Status;
                existing.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
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
