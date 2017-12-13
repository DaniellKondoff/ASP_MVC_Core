using System;
using System.Collections.Generic;
using System.Text;

namespace MusicStore.Data.Models
{
    public class ShoppingCart
    {
        public string Id { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
