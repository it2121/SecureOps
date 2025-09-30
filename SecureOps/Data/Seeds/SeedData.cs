using BCrypt.Net;
using SecureOps.Data;
using SecureOps.Models;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Users.Any() || context.Employees.Any())
            return; // DB has been seeded

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

        var dbRoles = context.Roles.ToList();

        // 2. Seed 10 users first
        var users = new List<User>();
        for (int i = 1; i <= 10; i++)
        {
            var user = new User
            {
                Username = $"user{i}",
                FullName = $"Employee {i}",
                Email = $"employee{i}@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword($"Pass{i}123!"),
            };

            users.Add(user);
        }

        context.Users.AddRange(users);
        context.SaveChanges(); // now Users have IDs

        // 3. Seed employees linked to users
        var employees = new List<Employee>();
        for (int i = 0; i < users.Count; i++)
        {
            var employee = new Employee
            {
                FullName = users[i].FullName,
                Email = users[i].Email,
                JobTitle = (i + 1) % 2 == 0 ? "Engineer" : "Manager",
                UserId = users[i].Id // set FK
            };

            employees.Add(employee);
        }

        context.Employees.AddRange(employees);
        context.SaveChanges();

        // 4. Assign roles (seed join table UserRoles)
        foreach (var user in users)
        {
            var role1 = dbRoles[user.Id % dbRoles.Count];
            var role2 = dbRoles[(user.Id + 1) % dbRoles.Count];

            user.Roles.Add(role1);
            user.Roles.Add(role2);
        }

        context.SaveChanges();
    }
}
