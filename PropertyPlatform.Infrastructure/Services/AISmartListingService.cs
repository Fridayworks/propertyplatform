using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using System.Text;

namespace PropertyPlatform.Infrastructure.Services
{
    public class AISmartListingService : IAISmartListingService
    {
        public Task<string> GenerateSellingPointsAsync(PropertyListing listing)
        {
            // This would typically call an AI service like OpenAI, but for now we'll provide a basic implementation
            var sellingPoints = new List<string>();
            
            // Generate basic selling points based on listing features
            if (!string.IsNullOrEmpty(listing.PropertyType))
            {
                sellingPoints.Add($"Modern {listing.PropertyType} with contemporary design");
            }
            
            var features = listing.Features.ToList();
            var bedrooms = features.FirstOrDefault(f => f.Key == "Bedrooms");
            var bathrooms = features.FirstOrDefault(f => f.Key == "Bathrooms");
            var sqft = features.FirstOrDefault(f => f.Key == "Sqft");
            
            if (bedrooms != null && !string.IsNullOrEmpty(bedrooms.Value))
            {
                sellingPoints.Add($"{bedrooms.Value}-bedroom layout");
            }
            
            if (bathrooms != null && !string.IsNullOrEmpty(bathrooms.Value))
            {
                sellingPoints.Add($"{bathrooms.Value} bathrooms");
            }
            
            if (sqft != null && !string.IsNullOrEmpty(sqft.Value))
            {
                sellingPoints.Add($"{sqft.Value} sqft of luxurious living space");
            }
            
            if (listing.Price > 0)
            {
                sellingPoints.Add($"Premium value at RM {listing.Price:N0}");
            }
            
            if (!string.IsNullOrEmpty(listing.Location))
            {
                sellingPoints.Add($"Prime location in {listing.Location}");
            }
            
            // Add some generic selling points
            sellingPoints.Add("Spacious and well-designed");
            sellingPoints.Add("Perfect for families or professionals");
            sellingPoints.Add("High-quality finishes throughout");
            
            return Task.FromResult(string.Join(", ", sellingPoints));
        }

        public Task<string> GenerateSeoTitleAsync(PropertyListing listing)
        {
            var titleParts = new List<string>();
            
            if (!string.IsNullOrEmpty(listing.PropertyType))
            {
                titleParts.Add(listing.PropertyType);
            }
            
            var features = listing.Features.ToList();
            var bedrooms = features.FirstOrDefault(f => f.Key == "Bedrooms");
            
            if (bedrooms != null && !string.IsNullOrEmpty(bedrooms.Value))
            {
                titleParts.Add($"{bedrooms.Value} bedroom");
            }
            
            if (!string.IsNullOrEmpty(listing.Location))
            {
                titleParts.Add(listing.Location);
            }
            
            if (listing.Price > 0)
            {
                titleParts.Add($"RM {listing.Price:N0}");
            }
            
            var title = string.Join(" ", titleParts);
            return Task.FromResult($"{title} for Sale | PropertyPlatform");
        }

        public Task<string> GenerateDescriptionAsync(PropertyListing listing)
        {
            var description = new StringBuilder();
            
            if (!string.IsNullOrEmpty(listing.PropertyType))
            {
                description.Append($"Beautiful {listing.PropertyType} for sale in {listing.Location}. ");
            }
            
            var features = listing.Features.ToList();
            var bedrooms = features.FirstOrDefault(f => f.Key == "Bedrooms");
            var bathrooms = features.FirstOrDefault(f => f.Key == "Bathrooms");
            var sqft = features.FirstOrDefault(f => f.Key == "Sqft");
            
            if (bedrooms != null && !string.IsNullOrEmpty(bedrooms.Value))
            {
                description.Append($"This {listing.PropertyType} features {bedrooms.Value} bedrooms and {bathrooms?.Value ?? "1"} bathrooms. ");
            }
            
            if (sqft != null && !string.IsNullOrEmpty(sqft.Value))
            {
                description.Append($"The property offers {sqft.Value} sqft of comfortable living space. ");
            }
            
            description.Append("This premium property combines modern design with comfort and convenience. ");
            
            if (listing.Price > 0)
            {
                description.Append($"Available for RM {listing.Price:N0}. ");
            }
            
            description.Append("Perfect for families or professionals seeking quality living.");
            
            return Task.FromResult(description.ToString());
        }

        public Task<string> GenerateFaqAsync(PropertyListing listing)
        {
            var faq = new StringBuilder();
            
            faq.Append("<h3>Frequently Asked Questions</h3>");
            faq.Append("<p><strong>Q: What is the property type?</strong></p>");
            faq.Append($"<p>A: This is a {listing.PropertyType} property.</p>");
            
            var features = listing.Features.ToList();
            var bedrooms = features.FirstOrDefault(f => f.Key == "Bedrooms");
            var bathrooms = features.FirstOrDefault(f => f.Key == "Bathrooms");
            
            if (bedrooms != null && !string.IsNullOrEmpty(bedrooms.Value))
            {
                faq.Append("<p><strong>Q: How many bedrooms does it have?</strong></p>");
                faq.Append($"<p>A: The property has {bedrooms.Value} bedrooms.</p>");
            }
            
            if (bathrooms != null && !string.IsNullOrEmpty(bathrooms.Value))
            {
                faq.Append("<p><strong>Q: How many bathrooms does it have?</strong></p>");
                faq.Append($"<p>A: The property has {bathrooms.Value} bathrooms.</p>");
            }
            
            if (listing.Price > 0)
            {
                faq.Append("<p><strong>Q: What is the price?</strong></p>");
                faq.Append($"<p>A: The property is available for RM {listing.Price:N0}.</p>");
            }
            
            faq.Append("<p><strong>Q: Where is it located?</strong></p>");
            faq.Append($"<p>A: The property is located in {listing.Location}.</p>");
            
            return Task.FromResult(faq.ToString());
        }

        public Task<string> GenerateNearbyAmenitiesNarrativeAsync(PropertyListing listing)
        {
            // This would typically use location data and AI to generate a narrative
            // For now, we'll provide a generic implementation
            var narrative = new StringBuilder();
            
            narrative.Append("This premium property is located in a vibrant area with excellent amenities. ");
            
            if (!string.IsNullOrEmpty(listing.Location))
            {
                narrative.Append($"The property is situated in {listing.Location}, which offers convenient access to various facilities. ");
            }
            
            narrative.Append("Nearby, you'll find shopping centers, restaurants, schools, and transportation hubs. ");
            narrative.Append("The area is known for its safety, convenience, and high quality of life. ");
            narrative.Append("Whether you're looking for daily essentials or recreational activities, everything is within reach.");
            
            return Task.FromResult(narrative.ToString());
        }

        public Task<string> GenerateEditableDraftAsync(PropertyListing listing)
        {
            var draft = new StringBuilder();
            
            draft.Append("<h2>Property Listing Draft</h2>");
            draft.Append("<p>This is an AI-generated draft for your property listing. Please review and edit as needed.</p>");
            
            draft.Append("<h3>SEO Title</h3>");
            draft.Append($"<p><strong>{GenerateSeoTitleAsync(listing).Result}</strong></p>");
            
            draft.Append("<h3>Property Description</h3>");
            draft.Append($"<p>{GenerateDescriptionAsync(listing).Result}</p>");
            
            draft.Append("<h3>Selling Points</h3>");
            draft.Append($"<p>{GenerateSellingPointsAsync(listing).Result}</p>");
            
            draft.Append("<h3>Nearby Amenities</h3>");
            draft.Append($"<p>{GenerateNearbyAmenitiesNarrativeAsync(listing).Result}</p>");
            
            draft.Append("<h3>FAQ Section</h3>");
            draft.Append($"{GenerateFaqAsync(listing).Result}");
            
            return Task.FromResult(draft.ToString());
        }
    }
}