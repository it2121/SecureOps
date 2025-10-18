using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    public class SafetyTask
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Open"; // Open, Done, Overdue, In Progress

        // Multi-tenancy
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        // Assigned employee
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        // Optional link to related incident
        public int? RelatedIncidentId { get; set; }
        public Incident? RelatedIncident { get; set; }

        // Optional link to related document
        public int? RelatedDocumentId { get; set; }

        public SDocument? RelatedDocument { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int? CompletedById { get; set; }
        [ForeignKey("CompletedById")]
        public User? CompletedBy { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; } // Optional progress notes
    }
}
