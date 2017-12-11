using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicStore.Data.DataConstants;

namespace MusicStore.Data.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        [MinLength(AlbumTitleMinLenght)]
        [MaxLength(AlbumTitleMaxLenght)]
        public string Title { get; set; }

        [Range(AlbumMinAmountOfSongs, AlbumMaxAmountOfSongs)]
        public int AmountOfSongs { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        [Range(AlbumMinPrice, double.MaxValue)]
        public decimal Price { get; set; }

        public List<SongAlbum> Songs { get; set; } = new List<SongAlbum>();
    }
}
