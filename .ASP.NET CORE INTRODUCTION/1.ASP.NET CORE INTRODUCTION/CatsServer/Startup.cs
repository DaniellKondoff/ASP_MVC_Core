namespace CatsServer
{
    using CatsServer.Infrastructure;
    using Data;
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
            services.AddDbContext<CatsDbContext>(options => options.UseSqlServer(@"Server=(localdb)\.;Database=CatsServerDb;Integrated Security=True"));
            //services.AddDbContext<CatsDbContext>(options => options.UseSqlServer(@"Server=.;Database=CatsServerDb;Integrated Security=True"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.Use((context, next) =>
            {
                context.RequestServices.GetRequiredService<CatsDbContext>().Database.Migrate();
                return next();
            });

            app.UseStaticFiles();

            app.Use((context, next)=>
            {
                context.Response.Headers.Add("Content-Type", "text/html");
                return next();
            });

            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/"
                    && ctx.Request.Method== HttpMethod.Get, 
                home =>
             {
                 home.Run(async(context) => 
                 {
                     await context.Response.WriteAsync($"<h1>{env.ApplicationName}</h1>");

                     var db = context.RequestServices.GetService<CatsDbContext>();

                     var catData = db.Cats.Select(c => new
                     {
                         c.Id,
                         c.Name
                     })
                     .ToList();

                     await context.Response.WriteAsync("<ul>");

                     foreach (var cat in catData)
                     {
                         await context.Response.WriteAsync($@"<li><a href=""/cat/{cat.Id}"">{cat.Name}<a/></li>");
                     }

                     await context.Response.WriteAsync("</ul>");

                     await context.Response.WriteAsync($@"
                        <form action = ""/cat/add"">
                        <input type=""submit"" value=""Add Cat"" />
                        </form>");
                 });
             });


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
                        else if(context.Request.Method == HttpMethod.Post)
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
                                    throw new InvalidOperationException("Invalid Cat Data");
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

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 Page was not Found :/");
            });
        }
    }
}
