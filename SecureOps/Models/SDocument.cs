using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class SDocument
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        // Original file details
        public string? FileName { get; set; }
        public string? ContentType { get; set; } // MIME type (pdf, docx, jpg, etc.)
        public long FileSize { get; set; }

        // Audit fields
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Who uploaded it
        public int? UploadedById { get; set; }
        public Employee? UploadedBy { get; set; }

        // Status
        public bool IsActive { get; set; } = true;
        public bool IsConfidential { get; set; } = false;

        // Categorization
        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(500)]
        public string? Tags { get; set; } // comma-separated, can later convert to separate table

        // Multi-tenancy
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        // Optional relationships
        public ICollection<EmployeeDocumentAcknowledgement>? Acknowledgements { get; set; }

        // Optional: Related Incidents or Tasks
        public int? RelatedIncidentId { get; set; }
        public Incident? RelatedIncident { get; set; }

        public int? RelatedTaskId { get; set; }
        public SafetyTask? RelatedTask { get; set; }
    }
}
