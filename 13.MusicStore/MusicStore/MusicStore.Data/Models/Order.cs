using System.Collections.Generic;

namespace MusicStore.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public ICollection<OrderItemAlbum> ItemsAlbums { get; set; } = new List<OrderItemAlbum>();
    }
}
