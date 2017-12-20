using MusicStore.Services.Models.Albums;
using MusicStore.Services.Models.Songs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStore.Services.Contracts
{
    public interface IShoppingService
    {
        Task CreateOrderAsync(string userId, IEnumerable<SongShoppingDetailsServiceModel> itemsWithDetails, IEnumerable<AlbumShoppingDetailsServiceModels> itemsAlbumsWithDetails);
    }
}
