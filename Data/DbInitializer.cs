using Microsoft.AspNetCore.Identity;
using Vehicle_Service_Center.Models;

namespace Vehicle_Service_Center.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Create roles
            string[] roles = { "Admin", "Mechanic", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Check Admin
            var admin = await userManager.FindByEmailAsync("admin@gmail.com");

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    FullName = "Administrator",
                    PhoneNumber = "0000000000"
                };

                var result = await userManager.CreateAsync(
                    admin,
                    "Admin@123"
                );

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}