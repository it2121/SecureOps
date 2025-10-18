using Bogus.DataSets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureOps.Models
{
    public class Incident
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public string? PhotoPath { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Reported"; // Reported, In Progress, Resolved, Closed

        [Required]
        public DateTime? ReportedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }

        [MaxLength(50)]
        public string? Severity { get; set; } // Low, Medium, High, Critical

        [MaxLength(100)]
        public string? Location { get; set; } // Optional physical site info

        // Multi-tenancy
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        // Who reported
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        // Optional: Who resolved / verified it
        public int? VerifiedById { get; set; }
        [ForeignKey("VerifiedById")]
        public Employee? VerifiedBy { get; set; }

        // Optional: Linked safety tasks or follow-ups
        public ICollection<SafetyTask>? FollowUpTasks { get; set; }

        // Optional: Related documents
        public ICollection<SDocument>? RelatedDocuments { get; set; }

        // Audit fields
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        public User? UpdatedBy { get; set; }
    }
}
