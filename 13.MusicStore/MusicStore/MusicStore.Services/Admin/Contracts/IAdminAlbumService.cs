using MusicStore.Services.Admin.Models.Albums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStore.Services.Admin.Contracts
{
    public interface IAdminAlbumService
    {
        Task CreateAsync(string title, decimal price, int amountOfSongs, int artistId);

        Task<IEnumerable<AdminAlbumsListingServiceModel>> ListAllAsync(int page = 1);

        Task<int> TotalAsync();

        Task<AdminAlbumEditServiceModel> GetByIdAsync(int id);

        Task<bool> ExistAsync(int id);

        Task EditAsync(int id, string title, decimal price, int amountOfSongs, int artistId);

        Task<bool> DeleteAsync(int id);

        Task<AdminAlbumDetailsServiceModel> DetailsAsync(int id);

        Task<int> GetArtistIdByAlbumId(int Id);

        Task<bool> AddSongToAlbumAsync(int id, int songId, int artistId);

        Task<bool> IsSongAddedAsync(int id, int songId);
    }
}
