using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    public class User
    {


        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? FullName { get; set; } // Optional but useful for reporting

        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty; // never store plain text

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        public bool IsActive { get; set; } = true; // Can disable inactive users

        // Multi-tenancy
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        // Many-to-many: roles
        public ICollection<Role> Roles { get; set; } = new List<Role>();

        // One-to-one: employee
        public Employee? Employee { get; set; }

        // Optional audit info
        public DateTime? PasswordChangedAt { get; set; }

        public int? CreatedById { get; set; } // for admin-created accounts
        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }


    }
}
