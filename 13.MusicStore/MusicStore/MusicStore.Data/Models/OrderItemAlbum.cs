namespace MusicStore.Data.Models
{
    public class OrderItemAlbum
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public decimal AlbumPrice { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
