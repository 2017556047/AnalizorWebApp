using AnalizorWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace AnalizorWebApp.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            // ROLES
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("Operator"))
                await roleManager.CreateAsync(new IdentityRole("Operator"));

            // ADMIN USER
            var adminEmail = "admin@site.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // OPERATOR USER
            var opEmail = "operator@site.com";
            var opUser = await userManager.FindByEmailAsync(opEmail);

            if (opUser == null)
            {
                opUser = new AppUser
                {
                    UserName = opEmail,
                    Email = opEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(opUser, "Operator123!");
                await userManager.AddToRoleAsync(opUser, "Operator");
            }
        }
    }
}
