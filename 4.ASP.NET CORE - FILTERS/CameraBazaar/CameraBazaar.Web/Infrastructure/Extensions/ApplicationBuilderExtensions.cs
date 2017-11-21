using CameraBazaar.Data;
using CameraBazaar.Data.Models;
using CameraBazaar.Web.Infrastructure.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CameraBazaar.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<CameraBazaarDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                Task.Run(async () =>
                {
                    var adminName = GlobalConstants.AdminInfo.AdministratorName;

                    var roleExist = await roleManager.RoleExistsAsync(adminName);

                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = adminName
                        });
                    }

                    var adminEmail = GlobalConstants.AdminInfo.AdministratorEmail;
                    var adminExist = await userManager.FindByEmailAsync(adminEmail);

                    if (adminExist == null)
                    {
                        var adminUser = new User
                        {
                            Email = adminEmail,
                            UserName = adminName
                        };

                        await userManager.CreateAsync(adminUser, GlobalConstants.AdminInfo.AdministratorPassword);

                        await userManager.AddToRoleAsync(adminUser, adminName);
                    }
                })
                .Wait();

                Task.Run(async () =>
                {
                    var userRoleName = GlobalConstants.GuestInfo.UserRole;

                    var userRoleExist = await roleManager.RoleExistsAsync(userRoleName);

                    if (!userRoleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = userRoleName
                        });
                    }
                })
                .Wait();
            }

            return app;
        }
    }
}
