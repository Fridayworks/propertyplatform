using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class DynamicMenu
    {
        [Key]
        public Guid MenuId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Url { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Location { get; set; } = "Main"; // Main, Footer

        public int SortOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
