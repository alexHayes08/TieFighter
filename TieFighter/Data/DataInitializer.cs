using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TieFighter.Models;

namespace TieFighter.Data
{
    public static class DataInitializer
    {
        public enum Roles
        {
            Administrator
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = userManager.Users.Where(u => u.Email == "alex.c.hayes08@gmail.com").FirstOrDefault();
            if (user != null)
            {
                if (!await userManager.IsInRoleAsync(user, "Administrator"))
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
        }
    }
}
