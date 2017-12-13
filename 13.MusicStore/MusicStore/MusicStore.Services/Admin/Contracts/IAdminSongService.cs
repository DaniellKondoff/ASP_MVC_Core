using MusicStore.Data.Models;
using MusicStore.Services.Admin.Models.Songs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStore.Services.Admin.Contracts
{
    public interface IAdminSongService
    {
        Task CreateAsync(string name, decimal price, double duration, int artistId, Ganre ganre);

        Task<IEnumerable<AdminSongListingServiceModel>> AllAsync(int page = 1);

        Task<int> TotalAsync();

        Task<bool> ExistAsync(int id);

        Task<AdminSongEditServiceModel> GetByIdAsync(int id);

        Task EditAsync(int id, string name, decimal price, double duration, int artistId, Ganre ganre);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<AdminSongBaseServiceModel>> AllBasicAsync(int id);

        Task<AdminSongDetailsServiceModel> DetailsAsync(int id);
    }
}
