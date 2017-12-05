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

        [MaxLength(AlbumMaxAmountOfSongs)]
        public int AmountOfSongs { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        public decimal Price { get; set; }

        public ICollection<SongAlbum> Songs { get; set; } = new HashSet<SongAlbum>();
    }
}
