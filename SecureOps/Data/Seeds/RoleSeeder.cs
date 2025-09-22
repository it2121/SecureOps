using Microsoft.AspNetCore.Identity;

namespace SecureOps.Data.Seeds
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "SafetyAdmin", "SafetyManager", "SafetyOfficer", "FieldEmployee", "Supervisor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser { UserName = "t@t.com", Email = "t@t.com", FullName = "ttt ass nigga" };
            await userManager.CreateAsync(user, "addasd!@#__123aAA");

            // Assign multiple roles
            await userManager.AddToRolesAsync(user, new[] { "SafetyManager", "Supervisor" });
        }



    }
}
