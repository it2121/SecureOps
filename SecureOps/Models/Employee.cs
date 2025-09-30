using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? JobTitle { get; set; }

        // One-to-one → navigation property
        public User? User { get; set; }   // Each Employee has one User
        public int? UserId { get; set; }  // optional foreign key


        // Relationship: One employee can have many tasks & incidents
        public ICollection<SafetyTask>? SafetyTasks { get; set; }
        public ICollection<Incident>? Incidents { get; set; }
        public ICollection<EmployeeDocumentAcknowledgement>? Acknowledgements { get; set; }
    }
}
