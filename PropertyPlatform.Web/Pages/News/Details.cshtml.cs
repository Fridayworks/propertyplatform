using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages.News
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Article Article { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.Slug == slug && a.Status == "Published");

            if (article == null) return NotFound();

            Article = article;
            return Page();
        }
    }
}
