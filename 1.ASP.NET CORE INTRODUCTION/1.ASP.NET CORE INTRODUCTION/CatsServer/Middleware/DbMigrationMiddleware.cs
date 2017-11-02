using CatsServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsServer.Middleware
{
    public class DbMigrationMiddleware
    {
        private readonly RequestDelegate next;

        public DbMigrationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.RequestServices.GetRequiredService<CatsDbContext>().Database.Migrate();

            return this.next(context);
        }
    }
}
