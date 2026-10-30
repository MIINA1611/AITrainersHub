using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace AITrainersHub.Infrastructure.DbInitializer
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            string[] roleNames = { Helpers.AdminRole, Helpers.LeaderRole };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Id = Guid.NewGuid(), Name = roleName });
                }
            }

            if (!userManager.Users.Any())
            {
                var defaultUser = new ApplicationUser
                {
                    DisplayName = "Dev Mina",
                    UserName = "twins.jack26@gmail.com",
                    Email = "twins.jack26@gmail.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(defaultUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, Helpers.AdminRole);
                }
            }
        }
    }
}