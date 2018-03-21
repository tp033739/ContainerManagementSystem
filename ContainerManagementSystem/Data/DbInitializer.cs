using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ContainerManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ContainerManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            context.Database.Migrate();

            SeedRoles(serviceProvider).Wait();
            SeedUsers(serviceProvider).Wait();

            context.SaveChanges();
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new string[] { Roles.Administrator, Roles.Agent };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var identityRole = new IdentityRole(role);
                    await roleManager.CreateAsync(identityRole);
                }
            }
        }

        private static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var users = new []
            {
                new
                {
                    username = "admin",
                    email = "admin@localhost.localdomain",
                    password = "fghjFGHJ4567$%^&",
                    role = Roles.Administrator
                },
                new
                {
                    username = "agent",
                    email = "agent@localhost.localdomain",
                    password = "fghjFGHJ4567$%^&",
                    role = Roles.Agent
                }
            };

            foreach (var user in users) {
                if (await userManager.FindByNameAsync(user.username) == null)
                {
                    var applicationUser = new ApplicationUser
                    {
                        UserName = user.username,
                        Email = user.email
                    };
                    var identityResult = await userManager.CreateAsync(applicationUser, user.password);
                    if (identityResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(applicationUser, user.role);
                    }
                }
            }
        }
    }
}
