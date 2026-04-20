using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Data;

namespace PropertyPlatform.Web.Pages
{
    public class SubmitReviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IReviewService _reviewService;

        public SubmitReviewModel(ApplicationDbContext context, IReviewService reviewService)
        {
            _context = context;
            _reviewService = reviewService;
        }

        [BindProperty(SupportsGet = true)]
        public Guid AgentId { get; set; }

        public AgentProfile AgentProfile { get; set; } = null!;

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string Comment { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            AgentProfile = await _context.AgentProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.TenantId == AgentId);

            if (AgentProfile == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (AgentId == Guid.Empty) return BadRequest();

            var success = await _reviewService.SubmitReviewAsync(AgentId, Name, Email, Rating, Comment);

            if (success)
            {
                TempData["SuccessMessage"] = "Thank you for your feedback!";
                
                // Find agent slug to redirect back
                var agent = await _context.AgentProfiles
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.TenantId == AgentId);
                
                if (agent != null && !string.IsNullOrEmpty(agent.Slug))
                {
                    return Redirect($"/a/{agent.Slug}");
                }
                
                return RedirectToPage("/Search");
            }

            ModelState.AddModelError(string.Empty, "Failed to submit review. Please check your inputs.");
            return await OnGetAsync();
        }
    }
}
