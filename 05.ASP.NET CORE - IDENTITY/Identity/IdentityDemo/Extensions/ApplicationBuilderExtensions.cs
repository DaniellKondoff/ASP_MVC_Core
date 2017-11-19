using IdentityDemo.Data;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace IdentityDemo.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();


                Task.Run(async () =>
                {
                    var adminName = GlobalConstants.AdministratorRole;

                    var roleExists = await roleManager.RoleExistsAsync(adminName);

                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = adminName
                        });
                    }

                    var adminEmail = "admin@admin.com";
                    var adminExist = await userManager.FindByNameAsync(adminEmail);

                    if (adminExist == null)
                    {
                        var adminUser = new User
                        {
                            Email = adminEmail,
                            UserName = adminEmail
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
