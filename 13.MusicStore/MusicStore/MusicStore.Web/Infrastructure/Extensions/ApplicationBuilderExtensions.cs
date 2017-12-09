using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Data;
using MusicStore.Data.Models;
using MusicStore.Web.Infrastructure.Common;
using System;
using System.Threading.Tasks;

namespace MusicStore.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<MusicStoreDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                Task.Run(async () =>
                {
                    var adminName = WebConstants.AdministratingRole;

                    var roleExist = await roleManager.RoleExistsAsync(adminName);

                    if (!roleExist)
                    {
                        var role = new IdentityRole { Name = adminName };

                        await roleManager.CreateAsync(role);
                    }

                    var adminEmail = WebConstants.AdministratingEmail;
                    var adminExist = await userManager.FindByEmailAsync(adminEmail);

                    if (adminExist == null)
                    {
                        var adminUser = new User
                        {
                            Email = adminEmail,
                            UserName = adminName,
                            FirstName = adminName,
                            LastName = adminName,
                            BirthDate = DateTime.Now
                        };

                        await userManager.CreateAsync(adminUser, WebConstants.AdministratingPassword);

                        await userManager.AddToRoleAsync(adminUser, adminName);
                    }
                })
                .Wait();
            }
            
            return app;
        }
    }
}
