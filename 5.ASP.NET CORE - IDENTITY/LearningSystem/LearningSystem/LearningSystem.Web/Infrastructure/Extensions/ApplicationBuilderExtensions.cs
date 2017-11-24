using LearningSystem.Data;
using LearningSystem.Data.Models;
using LearningSystem.Web.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LearningSystem.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<LearningSystemDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();


                Task.Run(async () =>
                {
                    var adminName = WebConstants.AdministratingRole;

                    var roles = new[]
                    {
                        adminName,
                        WebConstants.BlogAuthorRole,
                        WebConstants.TrainerRole
                    };

                    foreach (var role in roles)
                    {
                        var roleExists = await roleManager.RoleExistsAsync(role);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role
                            });
                        }
                    }
                    

                    var adminEmail = "admin@admin.com";
                    var adminExist = await userManager.FindByEmailAsync(adminEmail);

                    if (adminExist == null)
                    {
                        var adminUser = new User
                        {
                            Email = adminEmail,
                            UserName = adminName,
                            Name = adminName,
                            BirthDate = DateTime.UtcNow
                        };

                        await userManager.CreateAsync(adminUser, "admin123");

                        await userManager.AddToRoleAsync(adminUser, adminName);
                    }
                })
                .Wait();

            }

            return app;
        }
    }
}
