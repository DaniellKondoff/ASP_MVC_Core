using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CameraBazaar.Data.Models;

namespace CameraBazaar.Data
{
    public class CameraBazaarDbContext : IdentityDbContext<User>
    {
        public CameraBazaarDbContext(DbContextOptions<CameraBazaarDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
