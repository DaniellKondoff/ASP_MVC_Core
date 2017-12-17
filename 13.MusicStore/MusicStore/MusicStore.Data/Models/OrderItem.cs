namespace MusicStore.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int SongId { get; set; }

        public decimal SongPrice { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
