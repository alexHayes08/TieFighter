using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TieFighter.Data;
using TieFighter.Models;

namespace TieFighter
{
    public static class Seed
    {
        public static async Task InitializeAsync(IServiceProvider provider)
        {
            var _context = provider.GetRequiredService<ApplicationDbContext>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetNames(typeof(UserRoles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var users = userManager.Users;
            foreach (var user in users)
            {
                await userManager.AddToRoleAsync(user, "Registered");
            }
        }

        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(UserRoles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var users = userManager.Users;
            foreach (var user in users)
            {
                if (!await userManager.IsInRoleAsync(user, "Registered"))
                {
                    await userManager.AddToRoleAsync(user, "Registered");
                }
            }
        }
    }
}