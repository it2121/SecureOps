namespace SecureOps.Models.Dto
{
    public class SaftyTaskDto
    {
        public int? Id { get; set; } // null for create, filled for update

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Open"; // Open, Done, Overdue

        public int? EmployeeId { get; set; } // optional — can be unassigned
    }
}
