using Microsoft.EntityFrameworkCore;
using News.Data.Models;

namespace News.Data
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
        {

        }

        public DbSet<NewsEnt> News { get; set; }
    }
}
