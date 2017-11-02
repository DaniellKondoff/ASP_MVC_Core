using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CatsServer.Infrastructure;
using CatsServer.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CatsServer.Handlers
{
    public class AddCatHandler : IHandler
    {
        public int Order => 2;

        public Func<HttpContext, bool> Condition => 
            req => req.Request.Path.Value == "/cat/add";

        public RequestDelegate RequestHandler =>
            async (context) =>
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
            };
    }
}
