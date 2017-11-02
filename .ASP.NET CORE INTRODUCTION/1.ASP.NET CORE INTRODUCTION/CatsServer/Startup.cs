namespace CatsServer
{
    using Data;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;


    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatsDbContext>(options => options.UseSqlServer(@"Server=(localdb)\.;Database=CatsServerDb;Integrated Security=True"));
            //services.AddDbContext<CatsDbContext>(options => options.UseSqlServer(AppSetings.DatabaseConnectionString));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDatabaseMigration();
            app.UseStaticFiles();
            app.UseHtmlContentType();
            app.UseRequestHandlers();
            app.UseNotFoundHandler();
        }
    }
}
