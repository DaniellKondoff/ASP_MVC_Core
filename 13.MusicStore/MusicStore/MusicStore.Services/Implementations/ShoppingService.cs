using MusicStore.Data;
using MusicStore.Data.Models;
using MusicStore.Services.Contracts;
using MusicStore.Services.Models.Albums;
using MusicStore.Services.Models.Songs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Services.Implementations
{
    public class ShoppingService : IShoppingService
    {
        private readonly MusicStoreDbContext db;

        public ShoppingService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task CreateOrderAsync(string userId, IEnumerable<SongShoppingDetailsServiceModel> itemsWithDetails, IEnumerable<AlbumShoppingDetailsServiceModels> itemsAlbumsWithDetails)
        {
            var order = new Order
            {
                UserId = userId,
                TotalPrice = (itemsWithDetails.Sum(i => i.Price * i.Quantity) + itemsAlbumsWithDetails.Sum(a=>a.Price * a.Quantity))
            };

            foreach (var songItem in itemsWithDetails)
            {
                order.Items.Add(new OrderItem
                {
                    SongId = songItem.Id,
                    SongPrice = songItem.Price,
                    Quantity = songItem.Quantity
                });
            }

            foreach (var albumItem in itemsAlbumsWithDetails)
            {
                order.ItemsAlbums.Add(new OrderItemAlbum
                {
                    AlbumId = albumItem.Id,
                    AlbumPrice = albumItem.Price,
                    Quantity = albumItem.Quantity
                });
            }

            this.db.Orders.Add(order);
            await this.db.SaveChangesAsync();
        }
    }
}
