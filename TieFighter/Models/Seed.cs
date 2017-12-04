using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TieFighter.Models;

namespace TieFighter
{
    public static class Seed
    {
        public static async Task InitializeAsync(IServiceProvider provider)
        {
            //var _context = provider.GetRequiredService<TieFighterContext>();
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
                    var roleCreatedResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleCreatedResult.Succeeded)
                    {
                        Console.WriteLine("Failed to create role...");
                    }
                }
            }

            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                if (user.CreatedOn == null || user.CreatedOn == default(DateTime))
                {
                    user.CreatedOn = DateTime.Now.AddDays(-20);
                    if (user.MostRecentActivity == null || user.MostRecentActivity == default(DateTime))
                    {
                        user.MostRecentActivity = DateTime.Now.AddDays(-1);
                    }
                    var result = await userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        Console.WriteLine("Failed to update users CreatedOn property.");
                    }
                }

                if (!await userManager.IsInRoleAsync(user, "Registered"))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(user, "Registered");
                    if (!addToRoleResult.Succeeded)
                    {
                        Console.WriteLine("Failed to add user to role...");
                    }
                }

                if (user.Email == "alex.c.hayes08@gmail.com")
                {
                    if (!await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        var result = await userManager.AddToRoleAsync(user, "Admin");
                        if (!result.Succeeded)
                        {
                            Console.WriteLine("Failed to add user to Admin role!");
                        }
                    }
                }
            }
        }
    }
}