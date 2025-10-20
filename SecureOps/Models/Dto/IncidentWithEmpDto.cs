namespace SecureOps.Models.Dto
{
    public class IncidentWithEmpDto
    {
        public int? Id { get; set; }

        // Employee who reported the incident
        public int EmpId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? PhotoPath { get; set; }

        public string Status { get; set; } = "In Progress"; // Reported, In Progress, Resolved, Closed

        public DateTime? ReportedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }

        public string? Severity { get; set; } // Low, Medium, High, Critical

        public string? Location { get; set; } // Optional physical site info

        // Multi-tenancy
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        // Optional: Who verified/resolved it
        public int? VerifiedById { get; set; }
        public string? VerifiedByName { get; set; }

        // Optional: Audit info
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
        public string? UpdatedByName { get; set; }

        // Optional: For convenience when viewing
        public string? EmployeeName { get; set; }

        // Optional: Related counts (if you don’t want full objects)
        public int? FollowUpTasksCount { get; set; }
        public int? RelatedDocumentsCount { get; set; }
    }
}
