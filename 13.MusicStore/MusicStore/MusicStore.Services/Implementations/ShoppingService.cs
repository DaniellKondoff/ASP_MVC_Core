using MusicStore.Data;
using MusicStore.Data.Models;
using MusicStore.Services.Contracts;
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

        public async Task CreateOrderAsync(string userId, IEnumerable<SongShoppingDetailsServiceModel> itemsWithDetails)
        {
            var order = new Order
            {
                UserId = userId,
                TotalPrice = itemsWithDetails.Sum(i => i.Price * i.Quantity)
            };

            foreach (var item in itemsWithDetails)
            {
                order.Items.Add(new OrderItem
                {
                    SongId = item.Id,
                    SongPrice = item.Price,
                    Quantity = item.Quantity
                });
            }

            this.db.Orders.Add(order);
            await this.db.SaveChangesAsync();
        }
    }
}
