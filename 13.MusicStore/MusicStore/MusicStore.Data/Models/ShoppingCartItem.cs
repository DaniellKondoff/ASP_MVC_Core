using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStore.Data.Models
{
    public class ShoppingCartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? SongId { get; set; }

        public Song Song { get; set; }

        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
