using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace BeautyBook.Web.Infrastructure
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var config = services.GetRequiredService<IConfiguration>();

            foreach (var role in new[] { RoleNames.Admin, RoleNames.User })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var email = config["AdminUser:Email"];
            var password = config["AdminUser:Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    FirstName = "Админ",
                    LastName = "Администратор",
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, password);
                if (!result.Succeeded) return;
            }

            if (!await userManager.IsInRoleAsync(admin, RoleNames.Admin))
            {
                await userManager.AddToRoleAsync(admin, RoleNames.Admin);
            }
        }
    }
}