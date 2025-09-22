using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Users.Any())
            return; // DB has been seeded

        var users = new List<User>
        {
            new User
            {
                Username = "admin",
                FullName = "System Administrator",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin"
            },
            new User
            {
                Username = "john.doe",
                FullName = "John Doe",
                Email = "john@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("John123!"),
                Role = "User"
            },
            new User
            {
                Username = "jane.smith",
                FullName = "Jane Smith",
                Email = "jane@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Jane123!"),
                Role = "Manager"
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}
