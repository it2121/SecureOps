using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models.Dto
{
    public class SDocumentDto
    {
        public int Id { get; set; }

     
        public string Title { get; set; } = string.Empty;

       
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
        public string? Category { get; set; }
        public string? Tags { get; set; } // comma separated or later turned into related table

        // Relationship → Many acknowledgements
        public ICollection<EmployeeDocumentAcknowledgement>? Acknowledgements { get; set; }
    }
}
