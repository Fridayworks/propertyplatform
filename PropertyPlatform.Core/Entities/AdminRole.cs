using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyPlatform.Core.Entities
{
    public class AdminRole
    {
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Comma-separated list of permission codes (e.g., "CMS.Manage", "Users.Manage").
        /// Represents the Role Permission Matrix capabilities.
        /// </summary>
        public string Permissions { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
