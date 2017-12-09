using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Services.Admin.Models.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Services.ServiceConstants;

namespace MusicStore.Services.Admin.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly MusicStoreDbContext db;

        public AdminUserService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
        {
            return await this.db.Users
                 .Where(u => u.UserName != AdministratingRole)
                 .ProjectTo<AdminUserListingServiceModel>()
                 .ToListAsync();
        }
    }
}
