using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureOps.Models;
using System.Reflection.Metadata;

namespace SecureOps.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<SafetyTask> SafetyTasks { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;
        public DbSet<SDocument> SDocuments { get; set; } = null!;
        public DbSet<EmployeeDocumentAcknowledgement> EmployeeDocumentAcknowledgements { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many configuration
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles")); // join table


            modelBuilder.Entity<Employee>()
        .HasOne(e => e.User)
        .WithOne(u => u.Employee)
        .HasForeignKey<Employee>(e => e.UserId);



            modelBuilder.Entity<Incident>()
    .HasOne(i => i.Employee)
    .WithMany(e => e.Incidents)
    .HasForeignKey(i => i.EmployeeId)
    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
