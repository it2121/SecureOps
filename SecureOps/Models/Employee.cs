using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? JobTitle { get; set; }

        [MaxLength(100)]
        public string? EmployeeCode { get; set; } // Optional unique code/id

        // Multi-tenancy
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        // One-to-one navigation properties
        public User? User { get; set; }   // Each Employee has one User account
        public int? UserId { get; set; }

        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }

        // One-to-many relationships
        public ICollection<SafetyTask>? SafetyTasks { get; set; }
        public ICollection<Incident>? Incidents { get; set; }
        public ICollection<EmployeeDocumentAcknowledgement>? Acknowledgements { get; set; }

        // Optional supervisor/manager
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee>? Subordinates { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public int? UpdatedById { get; set; }
        public User? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true; // Soft delete / deactivate
    }
}
