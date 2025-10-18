using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    public class Page
    {
        [Key]
        public int PageId { get; set; }

        [Required, MaxLength(100)]
        public string PageName { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string PageUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; } // Optional: explain purpose of the page

        // Multi-tenancy
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

      

        // Many-to-many → roles with access
        public ICollection<Role> Roles { get; set; } = new List<Role>();

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        public User? UpdatedBy { get; set; }
    }
}
