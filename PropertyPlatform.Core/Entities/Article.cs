using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class Article
    {
        [Key]
        public Guid ArticleId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Excerpt { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Author { get; set; } = "System Admin";

        [MaxLength(500)]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = "General";

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Draft"; // Draft, Published

        public DateTime? PublishedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
