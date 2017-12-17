using MusicStore.Services.Models.ShoppingCarts;
using System.Collections.Generic;

namespace MusicStore.Services.Contracts
{
    public interface IShoppingCartManager
    {
        void AddToCart(string id,  int songId);

        void RemoveFromCart(string id, int songId);

        IEnumerable<CartItem> GetItems(string id);

        void Clear(string id);
    }
}
