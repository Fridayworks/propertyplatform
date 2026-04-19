using System;

namespace PropertyPlatform.Core.Entities
{
    public class PropertyMedia
    {
        public Guid MediaId { get; set; } = Guid.NewGuid();
        public Guid ListingId { get; set; }
        
        public string MediaUrl { get; set; } = string.Empty;
        public string MediaType { get; set; } = "Image"; // Image, Video, FloorPlan
        public int SortOrder { get; set; } = 0;

        public PropertyListing? Listing { get; set; }
    }
}
