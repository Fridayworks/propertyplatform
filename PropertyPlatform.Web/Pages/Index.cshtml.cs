using Microsoft.AspNetCore.Mvc.RazorPages;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;

namespace PropertyPlatform.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRecommendationService _recommendationService;

        public IndexModel(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public List<PropertyListing> RecommendedProperties { get; set; } = new List<PropertyListing>();

        public async Task OnGetAsync()
        {
            RecommendedProperties = await _recommendationService.GetRecommendedPropertiesAsync(count: 6);
        }
    }
}
