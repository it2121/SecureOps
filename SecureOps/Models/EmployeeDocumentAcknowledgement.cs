using System.Reflection.Metadata;

namespace SecureOps.Models
{
    public class EmployeeDocumentAcknowledgement
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int DocumentId { get; set; }
        public SDocument? SDocument { get; set; }

        public DateTime AcknowledgedAt { get; set; } = DateTime.UtcNow;
    }
}
