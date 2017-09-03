using BeamLab.MediaPortal.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeamLab.MediaPortal.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BeamLab.MediaPortal.Web
{
    public static class UsersRolesExtensions
    {
        private static readonly string[] Roles = new string[] { "Administrator" };
        private static readonly string AdminUserEmail = "admin@mediaportal.com";
        private static readonly string AdminUserPassword = "Admin2017!";
        private static readonly string AdminRole = "Administrator";

        public static async Task AddUsersRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (db.Database.GetPendingMigrations().Any())
                {
                    await db.Database.MigrateAsync();

                    var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                        foreach (var role in Roles)
                        {
                            if (!await roleManager.RoleExistsAsync(role))
                            {
                                await roleManager.CreateAsync(new IdentityRole(role));
                            }
                        }

                        if ((await userManager.FindByEmailAsync(AdminUserEmail)) == null)
                        {
                            var user = new ApplicationUser() { Email = AdminUserEmail, UserName = AdminUserEmail };
                            var creation = await userManager.CreateAsync(user, AdminUserPassword);

                            if (creation.Succeeded)
                            {
                                await userManager.AddToRoleAsync(user, AdminRole);
                            }
                        }
                    }
                }
            }
        }
    }

}
