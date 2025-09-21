using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class SafetyTask
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Open"; // Open, Done, Overdue

        // Foreign Key → Assigned Employee
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
