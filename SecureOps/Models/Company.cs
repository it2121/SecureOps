using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    [Table("Company")]

    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(150), EmailAddress]
        public string? Email { get; set; }

        [MaxLength(150)]
        public string? ContactPerson { get; set; } // Primary contact or safety manager

        [MaxLength(150)]
        public string? IndustryType { get; set; } // e.g., Construction, Oil & Gas, Manufacturing

        [MaxLength(50)]
        public string? SubscriptionPlan { get; set; } = "Free"; // Free, Standard, Enterprise

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Relationships
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<Incident>? Incidents { get; set; }
        public ICollection<SafetyTask>? SafetyTasks { get; set; }
        public ICollection<SDocument>? SDocuments { get; set; }
        public ICollection<Role>? Roles { get; set; }
        public ICollection<Page>? Pages { get; set; }
        public ICollection<Department>? Departments { get; set; }
    }
}
