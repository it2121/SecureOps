using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class Incident
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? PhotoPath { get; set; }

        public string Status { get; set; } = "In Progress"; // Reported, In Progress, Resolved

        public DateTime? ReportedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key → Employee who reported
        public int EmployeeId { get; set; }


        public Employee? Employee { get; set; }
    }
}
