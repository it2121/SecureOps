using Microsoft.EntityFrameworkCore;
using SecureOps.Models;
using System.Collections.Generic;

namespace SecureOps.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<SafetyTask> SafetyTasks { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;
        public DbSet<SDocument> SDocuments { get; set; } = null!;
        public DbSet<EmployeeDocumentAcknowledgement> EmployeeDocumentAcknowledgements { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Departments ---
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction); // <- changed from NoAction to avoid multiple NoAction paths

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Company)
                .WithMany(c => c.Departments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Employee ↔ User ---
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Employee Hierarchy ---
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Employee ↔ Company ---
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Employee Audit Fields ---
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.UpdatedBy)
                .WithMany()
                .HasForeignKey(e => e.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Incident ---
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Employee)
                .WithMany(e => e.Incidents)
                .HasForeignKey(i => i.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Company)
                .WithMany(c => c.Incidents)
                .HasForeignKey(i => i.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.UpdatedBy)
                .WithMany()
                .HasForeignKey(i => i.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);

            // --- SafetyTask ---
            modelBuilder.Entity<SafetyTask>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.SafetyTasks)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SafetyTask>()
                .HasOne(t => t.Company)
                .WithMany(c => c.SafetyTasks)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Documents ---
            modelBuilder.Entity<SDocument>()
                .HasOne(d => d.UploadedBy)
                .WithMany()
                .HasForeignKey(d => d.UploadedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SDocument>()
                .HasOne(d => d.Company)
                .WithMany(c => c.SDocuments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SDocument>()
                .HasMany(d => d.Acknowledgements)
                .WithOne(a => a.SDocument)
                .HasForeignKey(a => a.DocumentId);

            modelBuilder.Entity<SDocument>()
                .HasOne(d => d.RelatedTask)
                .WithOne(t => t.RelatedDocument)
                .HasForeignKey<SafetyTask>(t => t.RelatedDocumentId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Department Audit ---
            modelBuilder.Entity<Department>()
                .HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.UpdatedBy)
                .WithMany()
                .HasForeignKey(d => d.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Role Audit ---
            modelBuilder.Entity<Role>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.UpdatedBy)
                .WithMany()
                .HasForeignKey(r => r.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Page ↔ Role many-to-many ---
            modelBuilder.Entity<Page>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Pages)
                .UsingEntity<Dictionary<string, object>>(
                    "PageRole",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Page>().WithMany().HasForeignKey("PageId").OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("PageId", "RoleId");
                        j.ToTable("PageRoles");
                    });

            // --- User ↔ Role many-to-many ---
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("UserRoles");
                    });
        }


    }
}
