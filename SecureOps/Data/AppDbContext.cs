using Microsoft.EntityFrameworkCore;
using SecureOps.Models;
using System.Reflection.Metadata;

namespace SecureOps.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<SafetyTask> SafetyTasks { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;
        public DbSet<SDocument> SDocuments { get; set; } = null!;
        public DbSet<EmployeeDocumentAcknowledgement> EmployeeDocumentAcknowledgements { get; set; } = null!;


    }
}
