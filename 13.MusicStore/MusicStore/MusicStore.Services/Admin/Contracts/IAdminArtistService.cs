using MusicStore.Services.Admin.Models.Artists;
using System.Collections.Generic;
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

        Task<IEnumerable<AdminBaseArtistServiceModel>> AllBasicAsync();

        Task<bool> ExistAsync(int artistId);
    }
}
