using PropertyPlatform.Core.Entities;

namespace PropertyPlatform.Core.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<PropertyListing>> GetRecommendedPropertiesAsync(Guid? currentListingId = null, int count = 3);
    }
}
