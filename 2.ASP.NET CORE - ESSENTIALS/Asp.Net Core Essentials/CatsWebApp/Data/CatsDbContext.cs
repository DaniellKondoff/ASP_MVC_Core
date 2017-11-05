using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CatsWebApp.Models;

namespace CatsWebApp.Data
{
    public class CatsDbContext : IdentityDbContext<User>
    {
        public CatsDbContext(DbContextOptions<CatsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
    }
}
