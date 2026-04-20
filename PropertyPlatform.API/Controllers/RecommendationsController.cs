using Microsoft.AspNetCore.Mvc;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationsController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("listings/{listingId}")]
        public async Task<IActionResult> GetRecommendedListings(Guid listingId, [FromQuery] int count = 3)
        {
            var recommendations = await _recommendationService.GetRecommendedPropertiesAsync(listingId, count);
            return Ok(recommendations);
        }

        [HttpGet("listings")]
        public async Task<IActionResult> GetRecommendedListings([FromQuery] int count = 3)
        {
            var recommendations = await _recommendationService.GetRecommendedPropertiesAsync(null, count);
            return Ok(recommendations);
        }
    }
}