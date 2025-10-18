using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; } // Optional explanation of role permissions

        // Multi-tenancy
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        // Many-to-many → Users
        public ICollection<User> Users { get; set; } = new List<User>();

        // Many-to-many → Pages (permissions)
        public ICollection<Page> Pages { get; set; } = new List<Page>();

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
