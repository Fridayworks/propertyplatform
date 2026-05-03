using Microsoft.Extensions.DependencyInjection;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using PropertyPlatform.Infrastructure.Services;
using System.Threading.Tasks;
using Xunit;

namespace PropertyPlatform.Tests
{
    public class AISmartListingTests
    {
        [Fact]
        public async Task GenerateSellingPointsAsync_Should_Return_Selling_Points()
        {
            // Arrange
            var listing = new PropertyListing
            {
                Title = "Modern 3-Bedroom Condo",
                Description = "Beautiful modern condo",
                Location = "KLCC, Kuala Lumpur",
                Price = 850000,
                PropertyType = "Condo",
                Features = new List<PropertyFeature>
                {
                    new PropertyFeature { Key = "Bedrooms", Value = "3" },
                    new PropertyFeature { Key = "Bathrooms", Value = "2" },
                    new PropertyFeature { Key = "Sqft", Value = "1200" }
                }
            };

            var aiService = new AISmartListingService();

            // Act
            var result = await aiService.GenerateSellingPointsAsync(listing);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("3-bedroom", result);
            Assert.Contains("Condo", result);
            Assert.Contains("1200 sqft", result);
        }

        [Fact]
        public async Task GenerateSeoTitleAsync_Should_Return_SEO_Title()
        {
            // Arrange
            var listing = new PropertyListing
            {
                Title = "Modern 3-Bedroom Condo",
                Description = "Beautiful modern condo",
                Location = "KLCC, Kuala Lumpur",
                Price = 850000,
                PropertyType = "Condo",
                Features = new List<PropertyFeature>
                {
                    new PropertyFeature { Key = "Bedrooms", Value = "3" },
                    new PropertyFeature { Key = "Bathrooms", Value = "2" },
                    new PropertyFeature { Key = "Sqft", Value = "1200" }
                }
            };

            var aiService = new AISmartListingService();

            // Act
            var result = await aiService.GenerateSeoTitleAsync(listing);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("3 bedroom", result);
            Assert.Contains("KLCC", result);
            Assert.Contains("PropertyPlatform", result);
        }

        [Fact]
        public async Task GenerateDescriptionAsync_Should_Return_Description()
        {
            // Arrange
            var listing = new PropertyListing
            {
                Title = "Modern 3-Bedroom Condo",
                Description = "Beautiful modern condo",
                Location = "KLCC, Kuala Lumpur",
                Price = 850000,
                PropertyType = "Condo",
                Features = new List<PropertyFeature>
                {
                    new PropertyFeature { Key = "Bedrooms", Value = "3" },
                    new PropertyFeature { Key = "Bathrooms", Value = "2" },
                    new PropertyFeature { Key = "Sqft", Value = "1200" }
                }
            };

            var aiService = new AISmartListingService();

            // Act
            var result = await aiService.GenerateDescriptionAsync(listing);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("Condo", result);
            Assert.Contains("3 bedrooms", result);
            Assert.Contains("1200 sqft", result);
        }

        [Fact]
        public async Task GenerateFaqAsync_Should_Return_FAQ()
        {
            // Arrange
            var listing = new PropertyListing
            {
                Title = "Modern 3-Bedroom Condo",
                Description = "Beautiful modern condo",
                Location = "KLCC, Kuala Lumpur",
                Price = 850000,
                PropertyType = "Condo",
                Features = new List<PropertyFeature>
                {
                    new PropertyFeature { Key = "Bedrooms", Value = "3" },
                    new PropertyFeature { Key = "Bathrooms", Value = "2" },
                    new PropertyFeature { Key = "Sqft", Value = "1200" }
                }
            };

            var aiService = new AISmartListingService();

            // Act
            var result = await aiService.GenerateFaqAsync(listing);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("<h3>Frequently Asked Questions</h3>", result);
            Assert.Contains("3 bedroom", result);
            Assert.Contains("KLCC", result);
        }

        [Fact]
        public async Task GenerateNearbyAmenitiesNarrativeAsync_Should_Return_Narrative()
        {
            // Arrange
            var listing = new PropertyListing
            {
                Title = "Modern 3-Bedroom Condo",
                Description = "Beautiful modern condo",
                Location = "KLCC, Kuala Lumpur",
                Price = 850000,
                PropertyType = "Condo",
                Features = new List<PropertyFeature>
                {
                    new PropertyFeature { Key = "Bedrooms", Value = "3" },
                    new PropertyFeature { Key = "Bathrooms", Value = "2" },
                    new PropertyFeature { Key = "Sqft", Value = "1200" }
                }
            };

            var aiService = new AISmartListingService();

            // Act
            var result = await aiService.GenerateNearbyAmenitiesNarrativeAsync(listing);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("KLCC", result);
            Assert.Contains("amenities", result);
        }

        [Fact]
        public async Task GenerateEditableDraftAsync_Should_Return_Draft()
        {
            // Arrange
            var listing = new PropertyListing
            {
                Title = "Modern 3-Bedroom Condo",
                Description = "Beautiful modern condo",
                Location = "KLCC, Kuala Lumpur",
                Price = 850000,
                PropertyType = "Condo",
                Features = new List<PropertyFeature>
                {
                    new PropertyFeature { Key = "Bedrooms", Value = "3" },
                    new PropertyFeature { Key = "Bathrooms", Value = "2" },
                    new PropertyFeature { Key = "Sqft", Value = "1200" }
                }
            };

            var aiService = new AISmartListingService();

            // Act
            var result = await aiService.GenerateEditableDraftAsync(listing);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("<h2>Property Listing Draft</h2>", result);
            Assert.Contains("3 bedroom", result);
            Assert.Contains("KLCC", result);
        }
    }
}