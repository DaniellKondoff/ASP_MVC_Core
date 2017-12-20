using MusicStore.Services.Contracts;
using MusicStore.Services.Models.ShoppingCarts;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MusicStore.Services.Implementations
{
    public class ShoppingCartManager : IShoppingCartManager
    {
        private readonly ConcurrentDictionary<string, ShoppingCart> cart;

        public ShoppingCartManager()
        {
            this.cart = new ConcurrentDictionary<string, ShoppingCart>();
        }

        public void AddToCart(string id, int productId, string title)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.AddToCart(productId, title);
        }

        public void Clear(string id)
        {
            this.GetShoppingCart(id).Clear();
        }

        public IEnumerable<CartItem> GetItems(string id)
        {
            var shoppingCart = this.GetShoppingCart(id);

            return new List<CartItem>(shoppingCart.Items);
        }

        public void RemoveFromCart(string id, int productId, string title)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.RemoveFromCart(productId, title);
        }

        private ShoppingCart GetShoppingCart(string id)
        {
            return this.cart.GetOrAdd(id, new ShoppingCart());
        }
    }
}
