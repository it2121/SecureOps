using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class SDocument
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Relationship → Many acknowledgements
        public ICollection<EmployeeDocumentAcknowledgement>? Acknowledgements { get; set; }
    }
}
