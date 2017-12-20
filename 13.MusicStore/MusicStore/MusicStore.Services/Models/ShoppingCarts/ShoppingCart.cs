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

        public void AddToCart(int productId, string title)
        {
            var cartItem = this.items.FirstOrDefault(i => i.ProductId == productId && i.Title == title);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Title = title,
                    Quantity = 1
                };

                this.items.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

        }

        public void RemoveFromCart(int productId, string title)
        {
            var cartItem = this.items
                .FirstOrDefault(i => i.ProductId == productId && i.Title.ToLower().Contains(title.ToLower()));

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
