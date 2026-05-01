using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Article> Articles { get; set; } = new();

        public async Task OnGetAsync(string? category = null)
        {
            var query = _context.Articles
                .Where(a => a.Status == "Published")
                .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt);

            if (!string.IsNullOrEmpty(category))
            {
                Articles = await query.Where(a => a.Category == category).ToListAsync();
            }
            else
            {
                Articles = await query.ToListAsync();
            }
        }
    }
}
