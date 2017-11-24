using AutoMapper.QueryableExtensions;
using LearningSystem.Data;
using LearningSystem.Services.Admin.Contracts;
using LearningSystem.Services.Admin.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Services.Admin.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly LearningSystemDbContext db;

        public AdminUserService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
        {
           return await this.db.Users
                 .ProjectTo<AdminUserListingServiceModel>()
                 .ToListAsync();
        }
    }
}
