using MusicStore.Services.Admin.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStore.Services.Admin.Contracts
{
    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserListingServiceModel>> AllAsync();
    }
}
