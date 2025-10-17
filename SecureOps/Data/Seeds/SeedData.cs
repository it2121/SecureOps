using BCrypt.Net;
using SecureOps.Data;
using SecureOps.Models;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Users.Any() || context.Employees.Any())
            return; // DB has been seeded

        context.EmployeeDocumentAcknowledgements.RemoveRange(context.EmployeeDocumentAcknowledgements);
        context.SDocuments.RemoveRange(context.SDocuments);
        context.Incidents.RemoveRange(context.Incidents);
        context.SafetyTasks.RemoveRange(context.SafetyTasks);
        context.Employees.RemoveRange(context.Employees);
        context.Users.RemoveRange(context.Users);
        context.Roles.RemoveRange(context.Roles);




        context.SaveChanges();
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
        var adminRole = dbRoles.First(r => r.RoleName == "Admin");

        // 2. Seed 10 users first
        var users = new List<User>();
        for (int i = 1; i <= 10; i++)
        {
            var user = new User
            {
                Username = $"user{i}",
               
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
                FullName = $"Employee {i}",
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
        // 5. Seed Pages → All assigned to Admin
        var pages = new List<Page>
        {
            new Page { PageName = "Home", PageUrl = "/Index", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Counter", PageUrl = "/Counter", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Fetch Data", PageUrl = "/FetchData", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Login", PageUrl = "/Login", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Register", PageUrl = "/Register", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Profile", PageUrl = "/Profile", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Users", PageUrl = "/Users", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Roles Dialog", PageUrl = "/RolesDialog", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "User Management", PageUrl = "/UserManagementDialog", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Incidents", PageUrl = "/Incidents", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Employee Setup", PageUrl = "/Employee-setup", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Documents", PageUrl = "/SDocument", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "Pages Management", PageUrl = "/PagesDialog", Roles = new List<Role>{ adminRole } },
            new Page { PageName = "User Roles Management", PageUrl = "/UserRolesManagmentDialog", Roles = new List<Role>{ adminRole } },
            // 👉 Add the rest of your .razor files if needed
        };

        context.Pages.AddRange(pages);
        context.SaveChanges();
        // 5. Seed SDocuments
        var rnd = new Random();
        var docs = new List<SDocument>
        {
            new SDocument
            {
                Title = "Safety Guidelines",
                FilePath = "documents/safety_guidelines.pdf",
                FileName = "safety_guidelines.pdf",
                ContentType = "application/pdf",
                FileSize = 120000,
                UploadedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-5),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Safety",
                Tags = "safety,manual,guidelines"
            },
            new SDocument
            {
                Title = "Employee Handbook",
                FilePath = "documents/employee_handbook.pdf",
                FileName = "employee_handbook.pdf",
                ContentType = "application/pdf",
                FileSize = 250000,
                UploadedAt = DateTime.UtcNow.AddDays(-20),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = true,
                Category = "HR",
                Tags = "handbook,hr,policies"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            },
            new SDocument
            {
                Title = "Incident Report Template",
                FilePath = "documents/incident_report_template.docx",
                FileName = "incident_report_template.docx",
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileSize = 35000,
                UploadedAt = DateTime.UtcNow.AddDays(-2),
                UploadedById = employees[rnd.Next(employees.Count)].Id,
                IsActive = true,
                IsConfidential = false,
                Category = "Forms",
                Tags = "incident,report,template"
            }
        };

        context.SDocuments.AddRange(docs);


        context.SaveChanges();
    }
}
