namespace CatsServer
{
    using CatsServer.Infrastructure;
    using Data;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<CatsDbContext>(options => options.UseSqlServer(@"Server=(localdb)\.;Database=CatsServerDb;Integrated Security=True"));
            services.AddDbContext<CatsDbContext>(options => options.UseSqlServer(@"Server=.;Database=CatsServerDb;Integrated Security=True"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseDatabaseMigration();
            app.UseStaticFiles();
            app.UseHtmlContentType();
            app.UseRequestHandlers();

            app.MapWhen(
                req => req.Request.Path.Value == "/cat/add",
                catAdd =>
                {
                    catAdd.Run(async (context) =>
                    {
                        if (context.Request.Method == HttpMethod.Get)
                        {
                            context.Response.Redirect("/cat-add-form.html");
                        }
                        else if (context.Request.Method == HttpMethod.Post)
                        {

                            var db = context.RequestServices.GetService<CatsDbContext>();
                            var formData = context.Request.Form;

                            var age = 0;
                            int.TryParse(formData["Age"], out age);
                            var cat = new Cat
                            {
                                Name = formData["Name"],
                                Age = age,
                                Breed = formData["Breed"],
                                ImageUrl = formData["ImageUrl"]
                            };


                            db.Cats.Add(cat);
                            try
                            {
                                if (string.IsNullOrWhiteSpace(cat.Name)
                                    || string.IsNullOrWhiteSpace(cat.Breed)
                                    || string.IsNullOrWhiteSpace(cat.ImageUrl))
                                {
                                    throw new InvalidOperationException("Invalid Cat Data.");
                                }
                                await db.SaveChangesAsync();
                                context.Response.Redirect("/");
                            }
                            catch
                            {
                                await context.Response.WriteAsync("<h2> Invalid cat data! </h2>");
                                await context.Response.WriteAsync(@"<a href=""/cat/add"">Back To The Form</a>");
                            }
                        }
                    });
                });

            app.MapWhen(
                ctx => ctx.Request.Path.Value.StartsWith("/cat")
                && ctx.Request.Method == HttpMethod.Get,
                catDetails =>
                {
                    catDetails.Run(async (context) =>
                    {
                        var urlPart = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
                        if (urlPart.Length < 2)
                        {
                            context.Response.Redirect("/");
                            return;
                        }
                        else
                        {
                            var catId = 0;
                            int.TryParse(urlPart[1], out catId);
                            if (catId == 0)
                            {
                                context.Response.Redirect("/");
                                return;
                            }

                            var db = context.RequestServices.GetService<CatsDbContext>();

                            using (db)
                            {
                                var cat = await db.Cats.FindAsync(catId);

                                if (cat == null)
                                {
                                    context.Response.Redirect("/");
                                    return;
                                }

                                await context.Response.WriteAsync($@"<h1>{cat.Name}</h1>");
                                await context.Response.WriteAsync($@"<img src=""{cat.ImageUrl}"" alt=""{cat.Name}"" width=""300"" />");
                                await context.Response.WriteAsync($@"<p>Age: {cat.Age}</p>");
                                await context.Response.WriteAsync($@"<p>Breed: {cat.Breed}</p>");

                            }
                        }

                    });
                });

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 Page was not Found :/");
            });
        }
    }
}
