using PropertyPlatform.Core.Entities;

namespace PropertyPlatform.Core.Interfaces
{
    public interface IAISmartListingService
    {
        Task<string> GenerateSellingPointsAsync(PropertyListing listing);
        Task<string> GenerateSeoTitleAsync(PropertyListing listing);
        Task<string> GenerateDescriptionAsync(PropertyListing listing);
        Task<string> GenerateFaqAsync(PropertyListing listing);
        Task<string> GenerateNearbyAmenitiesNarrativeAsync(PropertyListing listing);
        Task<string> GenerateEditableDraftAsync(PropertyListing listing);
    }
}