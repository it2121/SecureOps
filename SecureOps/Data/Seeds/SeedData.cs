using BCrypt.Net;
using SecureOps.Data;
using SecureOps.Models;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        //if (context.Companies.Any())
        //    return; // Already seeded

        // -------------------------
        // 1. Companies (5)
        // -------------------------
        var companies = new List<Company>
            {
                new Company { Name = "Acme Corp", City = "Baghdad", Country = "Iraq" },
                new Company { Name = "GlobalTech", City = "Basra", Country = "Iraq" },
                new Company { Name = "BuildIt", City = "Erbil", Country = "Iraq" },
                new Company { Name = "OilPro", City = "Najaf", Country = "Iraq" },
                new Company { Name = "SafeWorks", City = "Sulaymaniyah", Country = "Iraq" }
            };
        context.Companies.AddRange(companies);
        context.SaveChanges();

        // -------------------------
        // 2. Departments (5)
        // -------------------------
        var departments = new List<Department>
            {
                new Department { Name = "IT", CompanyId = companies[0].Id },
                new Department { Name = "HR", CompanyId = companies[0].Id },
                new Department { Name = "Engineering", CompanyId = companies[1].Id },
                new Department { Name = "Safety", CompanyId = companies[2].Id },
                new Department { Name = "Operations", CompanyId = companies[3].Id }
            };
        context.Departments.AddRange(departments);
        context.SaveChanges();

        // -------------------------
        // 3. Employees (5)
        // -------------------------
        var employees = new List<Employee>
            {
                new Employee { FullName = "Alice Smith", Email = "alice@acme.com", CompanyId = companies[0].Id, DepartmentId = departments[0].Id },
                new Employee { FullName = "Bob Johnson", Email = "bob@acme.com", CompanyId = companies[0].Id, DepartmentId = departments[1].Id },
                new Employee { FullName = "Charlie Brown", Email = "charlie@globaltech.com", CompanyId = companies[1].Id, DepartmentId = departments[2].Id },
                new Employee { FullName = "Diana Prince", Email = "diana@buildit.com", CompanyId = companies[2].Id, DepartmentId = departments[3].Id },
                new Employee { FullName = "Evan Wright", Email = "evan@oilpro.com", CompanyId = companies[3].Id, DepartmentId = departments[4].Id }
            };
        context.Employees.AddRange(employees);
        context.SaveChanges();

        // -------------------------
        // 4. Users (5) with BCrypt passwords
        // -------------------------
        var users = new List<User>();
        for (int i = 0; i < employees.Count; i++)
        {
            var emp = employees[i];
            users.Add(new User
            {
                Username = emp.FullName.Split(' ')[0].ToLower(),
                FullName = emp.FullName,
                Email = emp.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword($"Pass{i + 1}123!"),
                CompanyId = emp.CompanyId,
                Employee = emp
            });
        }
        context.Users.AddRange(users);
        context.SaveChanges();

        // -------------------------
        // 5. Roles (5)
        // -------------------------
        var roles = new List<Role>
            {
                new Role { RoleName = "Admin", CompanyId = companies[0].Id },
                new Role { RoleName = "User", CompanyId = companies[0].Id },
                new Role { RoleName = "Manager", CompanyId = companies[1].Id },
                new Role { RoleName = "Supervisor", CompanyId = companies[2].Id },
                new Role { RoleName = "Auditor", CompanyId = companies[3].Id }
            };
        context.Roles.AddRange(roles);
        context.SaveChanges();

        // Assign users to roles
        users[0].Roles.Add(roles[0]); // Alice → Admin
        users[1].Roles.Add(roles[1]); // Bob → User
        users[2].Roles.Add(roles[2]); // Charlie → Manager
        users[3].Roles.Add(roles[3]); // Diana → Supervisor
        users[4].Roles.Add(roles[4]); // Evan → Auditor
        context.SaveChanges();

        // -------------------------
        // 6. Pages (5)
        // -------------------------
        var pages = new List<Page>
            {
                new Page { PageName = "Dashboard", PageUrl = "/dashboard", CompanyId = companies[0].Id },
                new Page { PageName = "Incidents", PageUrl = "/incidents", CompanyId = companies[0].Id },
                new Page { PageName = "Reports", PageUrl = "/reports", CompanyId = companies[1].Id },
                new Page { PageName = "Tasks", PageUrl = "/tasks", CompanyId = companies[2].Id },
                new Page { PageName = "Documents", PageUrl = "/documents", CompanyId = companies[3].Id }
            };
        context.Pages.AddRange(pages);
        context.SaveChanges();

        // Assign pages to roles
        roles[0].Pages.Add(pages[0]); // Admin → Dashboard
        roles[1].Pages.Add(pages[1]); // User → Incidents
        roles[2].Pages.Add(pages[2]); // Manager → Reports
        roles[3].Pages.Add(pages[3]); // Supervisor → Tasks
        roles[4].Pages.Add(pages[4]); // Auditor → Documents
        context.SaveChanges();

        // -------------------------
        // 7. Incidents (5)
        // -------------------------
        var incidents = new List<Incident>
            {
                new Incident { Title = "Site Accident", Status = "Reported", CompanyId = companies[0].Id, EmployeeId = employees[0].Id },
                new Incident { Title = "Fire Drill", Status = "Resolved", CompanyId = companies[0].Id, EmployeeId = employees[1].Id },
                new Incident { Title = "Equipment Failure", Status = "In Progress", CompanyId = companies[1].Id, EmployeeId = employees[2].Id },
                new Incident { Title = "Chemical Spill", Status = "Reported", CompanyId = companies[2].Id, EmployeeId = employees[3].Id },
                new Incident { Title = "Power Outage", Status = "Closed", CompanyId = companies[3].Id, EmployeeId = employees[4].Id }
            };
        context.Incidents.AddRange(incidents);
        context.SaveChanges();

        // -------------------------
        // 8. SafetyTasks (5)
        // -------------------------
        var tasks = new List<SafetyTask>
            {
                new SafetyTask { Title = "Inspect scaffolding", Status = "Open", CompanyId = companies[0].Id, EmployeeId = employees[0].Id, DueDate = DateTime.UtcNow.AddDays(3) },
                new SafetyTask { Title = "Check fire extinguishers", Status = "Done", CompanyId = companies[0].Id, EmployeeId = employees[1].Id, DueDate = DateTime.UtcNow.AddDays(1) },
                new SafetyTask { Title = "Inspect machinery", Status = "In Progress", CompanyId = companies[1].Id, EmployeeId = employees[2].Id, DueDate = DateTime.UtcNow.AddDays(2) },
                new SafetyTask { Title = "Emergency drill", Status = "Open", CompanyId = companies[2].Id, EmployeeId = employees[3].Id, DueDate = DateTime.UtcNow.AddDays(4) },
                new SafetyTask { Title = "Site cleanup", Status = "Open", CompanyId = companies[3].Id, EmployeeId = employees[4].Id, DueDate = DateTime.UtcNow.AddDays(5) }
            };
        context.SafetyTasks.AddRange(tasks);
        context.SaveChanges();

        // -------------------------
        // 9. SDocuments (5)
        // -------------------------
        var docs = new List<SDocument>
            {
                new SDocument { Title = "Safety Manual", FilePath = "/docs/safety1.pdf", CompanyId = companies[0].Id, FileSize = 102400 },
                new SDocument { Title = "HR Policy", FilePath = "/docs/hr_policy.pdf", CompanyId = companies[0].Id, FileSize = 204800 },
                new SDocument { Title = "Machine Guidelines", FilePath = "/docs/machine.pdf", CompanyId = companies[1].Id, FileSize = 512000 },
                new SDocument { Title = "Chemical Safety", FilePath = "/docs/chemical.pdf", CompanyId = companies[2].Id, FileSize = 256000 },
                new SDocument { Title = "Emergency Plan", FilePath = "/docs/emergency.pdf", CompanyId = companies[3].Id, FileSize = 128000 }
            };
        context.SDocuments.AddRange(docs);
        context.SaveChanges();
    
    //if (context.Users.Any() || context.Employees.Any())
    //    return; // DB has been seeded

    //context.EmployeeDocumentAcknowledgements.RemoveRange(context.EmployeeDocumentAcknowledgements);
    //context.SDocuments.RemoveRange(context.SDocuments);
    //context.Incidents.RemoveRange(context.Incidents);
    //context.SafetyTasks.RemoveRange(context.SafetyTasks);
    //context.Employees.RemoveRange(context.Employees);
    //context.Users.RemoveRange(context.Users);
    //context.Roles.RemoveRange(context.Roles);




    //context.SaveChanges();
    //// 1. Seed roles
    //var roles = new List<Role>
    //{
    //    new Role { RoleName = "Admin" },
    //    new Role { RoleName = "Manager" },
    //    new Role { RoleName = "Developer" },
    //    new Role { RoleName = "Tester" },
    //    new Role { RoleName = "Support" },
    //    new Role { RoleName = "Viewer" }
    //};

    //context.Roles.AddRange(roles);
    //context.SaveChanges();

    //var dbRoles = context.Roles.ToList();
    //var adminRole = dbRoles.First(r => r.RoleName == "Admin");

    //// 2. Seed 10 users first
    //var users = new List<User>();
    //for (int i = 1; i <= 10; i++)
    //{
    //    var user = new User
    //    {
    //        Username = $"user{i}",

    //        Email = $"employee{i}@company.com",
    //        PasswordHash = BCrypt.Net.BCrypt.HashPassword($"Pass{i}123!"),
    //    };

    //    users.Add(user);
    //}

    //context.Users.AddRange(users);
    //context.SaveChanges(); // now Users have IDs

    //// 3. Seed employees linked to users
    //var employees = new List<Employee>();
    //for (int i = 0; i < users.Count; i++)
    //{
    //    var employee = new Employee
    //    {
    //        FullName = $"Employee {i}",
    //        Email = users[i].Email,
    //        JobTitle = (i + 1) % 2 == 0 ? "Engineer" : "Manager",
    //        UserId = users[i].Id // set FK
    //    };

    //    employees.Add(employee);
    //}

    //context.Employees.AddRange(employees);
    //context.SaveChanges();

    //// 4. Assign roles (seed join table UserRoles)
    //foreach (var user in users)
    //{
    //    var role1 = dbRoles[user.Id % dbRoles.Count];
    //    var role2 = dbRoles[(user.Id + 1) % dbRoles.Count];

    //    user.Roles.Add(role1);
    //    user.Roles.Add(role2);
    //}

    //context.SaveChanges();
    //// 5. Seed Pages → All assigned to Admin
    //var pages = new List<Page>
    //{
    //    new Page { PageName = "Home", PageUrl = "/Index", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Counter", PageUrl = "/Counter", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Fetch Data", PageUrl = "/FetchData", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Login", PageUrl = "/Login", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Register", PageUrl = "/Register", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Profile", PageUrl = "/Profile", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Users", PageUrl = "/Users", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Roles Dialog", PageUrl = "/RolesDialog", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "User Management", PageUrl = "/UserManagementDialog", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Incidents", PageUrl = "/Incidents", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Employee Setup", PageUrl = "/Employee-setup", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Documents", PageUrl = "/SDocument", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "Pages Management", PageUrl = "/PagesDialog", Roles = new List<Role>{ adminRole } },
    //    new Page { PageName = "User Roles Management", PageUrl = "/UserRolesManagmentDialog", Roles = new List<Role>{ adminRole } },
    //    // 👉 Add the rest of your .razor files if needed
    //};

    //context.Pages.AddRange(pages);
    //context.SaveChanges();
    //// 5. Seed SDocuments
    //var rnd = new Random();
    //var docs = new List<SDocument>
    //{
    //    new SDocument
    //    {
    //        Title = "Safety Guidelines",
    //        FilePath = "documents/safety_guidelines.pdf",
    //        FileName = "safety_guidelines.pdf",
    //        ContentType = "application/pdf",
    //        FileSize = 120000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-10),
    //        UpdatedAt = DateTime.UtcNow.AddDays(-5),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Safety",
    //        Tags = "safety,manual,guidelines"
    //    },
    //    new SDocument
    //    {
    //        Title = "Employee Handbook",
    //        FilePath = "documents/employee_handbook.pdf",
    //        FileName = "employee_handbook.pdf",
    //        ContentType = "application/pdf",
    //        FileSize = 250000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-20),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = true,
    //        Category = "HR",
    //        Tags = "handbook,hr,policies"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    },
    //    new SDocument
    //    {
    //        Title = "Incident Report Template",
    //        FilePath = "documents/incident_report_template.docx",
    //        FileName = "incident_report_template.docx",
    //        ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //        FileSize = 35000,
    //        UploadedAt = DateTime.UtcNow.AddDays(-2),
    //        UploadedById = employees[rnd.Next(employees.Count)].Id,
    //        IsActive = true,
    //        IsConfidential = false,
    //        Category = "Forms",
    //        Tags = "incident,report,template"
    //    }
    //};

    //context.SDocuments.AddRange(docs);


    //context.SaveChanges();
}
}
