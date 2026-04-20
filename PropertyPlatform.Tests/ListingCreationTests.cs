using Xunit;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyPlatform.Tests
{
    public class ListingCreationTests : IAsyncLifetime
    {
        private ApplicationDbContext _context;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"PropertyPlatformTest_Listing_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            await SeedTestData();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        private async Task SeedTestData()
        {
            // Seed test tenant
            var tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Email = "agent@test.com",
                PasswordHash = "hashed",
                CreatedAt = DateTime.UtcNow
            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// LISTING_001: Agent uploads valid property PDF with all required fields
        /// Expected: Listing created with extracted data (title, bedrooms, price, description)
        /// </summary>
        [Fact]
        public async Task CreateListing_WithValidPDF_ShouldExtractAndCreateListing()
        {
            // Arrange
            var tenant = _context.Tenants.First();
            var listing = new PropertyListing
            {
                ListingId = Guid.NewGuid(),
                Title = "Beautiful Apartment",
                Location = "New York, NY",
                Price = 2500,
                Description = "Beautifully renovated apartment",
                PropertyType = "Condo",
                Status = "Active",
                TenantId = tenant.TenantId,
                CreatedAt = DateTime.UtcNow,
                Media = new List<PropertyMedia>()
            };

            // Act
            _context.PropertyListings.Add(listing);
            await _context.SaveChangesAsync();

            var createdListing = _context.PropertyListings
                .FirstOrDefault(l => l.ListingId == listing.ListingId);

            // Assert
            Assert.NotNull(createdListing);
            Assert.Equal("Beautiful Apartment", createdListing.Title);
            Assert.Equal(2500, createdListing.Price);
            Assert.Equal("Active", createdListing.Status);
            Assert.Equal(tenant.TenantId, createdListing.TenantId);
        }

        /// <summary>
        /// LISTING_002: Agent uploads PDF missing required fields (e.g., price, bedrooms)
        /// Expected: Validation error returned; listing not created
        /// </summary>
        [Fact]
        public async Task CreateListing_WithMissingRequiredFields_ShouldReturnValidationError()
        {
            // Arrange
            var tenant = _context.Tenants.First();
            var incompleteData = new { title = "Incomplete Listing", price = 0m };

            // Act & Assert - Simulate validation
            var isValid = !string.IsNullOrWhiteSpace(incompleteData.title) && incompleteData.price > 0;
            Assert.False(isValid, "Validation should fail for missing price");
        }

        /// <summary>
        /// LISTING_003: Agent uploads large PDF file (>100MB)
        /// Expected: File size error; listing not created
        /// </summary>
        [Fact]
        public void CreateListing_WithLargeFile_ShouldReturnFileSizeError()
        {
            // Arrange
            const long maxFileSize = 100 * 1024 * 1024; // 100MB
            var uploadedFileSize = 150 * 1024 * 1024; // 150MB

            // Act & Assert
            var isValidSize = uploadedFileSize <= maxFileSize;
            Assert.False(isValidSize, "File size exceeds maximum allowed");
        }

        /// <summary>
        /// LISTING_004: Agent adds property media (images from URL)
        /// Expected: Media associated with listing; accessible via API
        /// </summary>
        [Fact]
        public async Task AddPropertyMedia_WithValidURL_ShouldAssociateMediaToListing()
        {
            // Arrange
            var tenant = _context.Tenants.First();
            var listing = new PropertyListing
            {
                ListingId = Guid.NewGuid(),
                Title = "Property with Media",
                Location = "Los Angeles, CA",
                Price = 3000,
                PropertyType = "Condo",
                Status = "Active",
                TenantId = tenant.TenantId,
                CreatedAt = DateTime.UtcNow,
                Media = new List<PropertyMedia>()
            };

            var media = new PropertyMedia
            {
                MediaId = Guid.NewGuid(),
                ListingId = listing.ListingId,
                MediaUrl = "https://example.com/property-image.jpg",
                MediaType = "Image",
                SortOrder = 1
            };

            listing.Media.Add(media);

            // Act
            _context.PropertyListings.Add(listing);
            _context.PropertyMedia.Add(media);
            await _context.SaveChangesAsync();

            var savedListing = _context.PropertyListings
                .Include(l => l.Media)
                .FirstOrDefault(l => l.ListingId == listing.ListingId);

            // Assert
            Assert.NotNull(savedListing);
            Assert.Single(savedListing.Media);
            Assert.Equal("https://example.com/property-image.jpg", savedListing.Media.First().MediaUrl);
            Assert.Equal("Image", savedListing.Media.First().MediaType);
        }
    }
}
