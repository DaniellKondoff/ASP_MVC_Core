namespace MusicStore.Data.Models
{
    public class SongAlbum
    {
        public int SongId { get; set; }

        public Song Song { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }
    }
}
