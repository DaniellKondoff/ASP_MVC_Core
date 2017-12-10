using MusicStore.Services.Admin.Models.Artists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Admin.Contracts
{
    public interface IAdminArtistService
    {
        Task<IEnumerable<AdminArtistListingServiceModel>> AllAsync(int page = 1);

        Task<int> TotalAsync();

        Task CreateAsync(string name);

        Task<AdminBaseArtistServiceModel> GetByIdAsync(int id);

        Task<bool> EditAsync(int id, string name);

        Task<bool> DeleteAsync(int id);
    }
}
