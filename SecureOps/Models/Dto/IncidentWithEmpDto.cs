namespace SecureOps.Models.Dto
{
    public class IncidentWithEmpDto
    {


        public int? Id { get; set; }
        public int EmpId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? PhotoPath { get; set; }

        public string Status { get; set; } = "In Progress"; // Reported, In Progress, Resolved

        public DateTime? ReportedAt { get; set; } = DateTime.UtcNow;


    }
}
