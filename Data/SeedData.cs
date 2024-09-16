using Microsoft.AspNetCore.Identity;
using CarManagement.Models;
using System.Threading.Tasks;

namespace CarManagement.Data
{
    public static class SeedData
    {
        //SIMPLE ROLE BASED AUTHENTICATION making an WebAdmin Role and User Role
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed roles
            if (!await roleManager.RoleExistsAsync("WebAdmin"))
            {
                await roleManager.CreateAsync(new IdentityRole("WebAdmin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed admin user
            var adminEmail = "admin@carmanagement.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User"
                };
                var createAdminResult = await userManager.CreateAsync(newAdminUser, "Dd-7199");
                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "WebAdmin");
                }
            }
            else
            {
                // If the user exists, make sure they are in the WebAdmin role
                if (!await userManager.IsInRoleAsync(adminUser, "WebAdmin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "WebAdmin");
                }
            }
        }
    }
}
