using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = "Department Name";

        // Multi-tenancy
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        // Optional manager
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        // Employees in the department
        public ICollection<Employee>? Employees { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        public User? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true; // Soft delete / deactivate
    }
}
