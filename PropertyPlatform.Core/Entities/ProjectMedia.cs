using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class ProjectMedia
    {
        public Guid MediaId { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public string MediaTitle { get; set; } = string.Empty;
        public string MediaDescription { get; set; } = string.Empty;
        public string MediaType { get; set; } = "image"; // image, video, pdf, document
        public string MediaFileName { get; set; } = string.Empty;
        public long MediaFileSize { get; set; }
        public string MediaFileType { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Project? Project { get; set; }
    }
}