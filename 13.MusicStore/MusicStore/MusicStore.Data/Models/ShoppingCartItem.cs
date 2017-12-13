namespace MusicStore.Data.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public int? SongId { get; set; }

        public Song Song { get; set; }

        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
