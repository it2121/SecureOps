using BCrypt.Net;
using SecureOps.Data;
using SecureOps.Models;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
       /* if (context.Users.Any())
            return; // DB has been seeded*/

        // 1. Seed roles
        var roles = new List<Role>
        {
            new Role { RoleName = "Admin" },
            new Role { RoleName = "Manager" },
            new Role { RoleName = "Developer" },
            new Role { RoleName = "Tester" },
            new Role { RoleName = "Support" },
            new Role { RoleName = "Viewer" }
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();

        // Reload roles from DB so they have IDs
        var dbRoles = context.Roles.ToList();

        // 2. Seed users with roles
        var users = new List<User>
        {
            new User
            {
                Username = "admin",
                FullName = "System Administrator",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Roles = new List<Role>
                {
                    dbRoles.First(r => r.RoleName == "Admin"),
                    dbRoles.First(r => r.RoleName == "Developer"),
                    dbRoles.First(r => r.RoleName == "Support")
                }
            },
            new User
            {
                Username = "john.doe",
                FullName = "John Doe",
                Email = "john@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("John123!"),
                Roles = new List<Role>
                {
                    dbRoles.First(r => r.RoleName == "Manager"),
                    dbRoles.First(r => r.RoleName == "Tester"),
                    dbRoles.First(r => r.RoleName == "Viewer")
                }
            },
            new User
            {
                Username = "jane.smith",
                FullName = "Jane Smith",
                Email = "jane@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Jane123!"),
                Roles = new List<Role>
                {
                    dbRoles.First(r => r.RoleName == "Admin"),
                    dbRoles.First(r => r.RoleName == "Manager"),
                    dbRoles.First(r => r.RoleName == "Developer")
                }
            },
            new User
            {
                Username = "bob.brown",
                FullName = "Bob Brown",
                Email = "bob@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Bob123!"),
                Roles = new List<Role>
                {
                    dbRoles.First(r => r.RoleName == "Tester"),
                    dbRoles.First(r => r.RoleName == "Support"),
                    dbRoles.First(r => r.RoleName == "Viewer")
                }
            },
            new User
            {
                Username = "diana.prince",
                FullName = "Diana Prince",
                Email = "diana@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Diana123!"),
                Roles = new List<Role>
                {
                    dbRoles.First(r => r.RoleName == "Admin"),
                    dbRoles.First(r => r.RoleName == "Support"),
                    dbRoles.First(r => r.RoleName == "Viewer")
                }
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}
