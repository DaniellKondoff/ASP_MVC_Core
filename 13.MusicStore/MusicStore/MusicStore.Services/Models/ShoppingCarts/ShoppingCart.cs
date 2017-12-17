using System.Collections.Generic;
using System.Linq;

namespace MusicStore.Services.Models.ShoppingCarts
{  
    public class ShoppingCart
    {
        private readonly IList<CartItem> items;

        public ShoppingCart()
        {
            this.items = new List<CartItem>();
        }

        public void AddToCart(int songId)
        {
            var cartItem = this.items.FirstOrDefault(s => s.SongId == songId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    SongId = songId,
                    Quantity = 1
                };

                this.items.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

        }

        public void RemoveFromCart(int songId)
        {
            var cartItem = this.items
                .FirstOrDefault(i => i.SongId == songId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    this.items.Remove(cartItem);
                }
            }
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public IEnumerable<CartItem> Items => new List<CartItem>(this.items);
    }
}
