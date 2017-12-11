using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Admin.Models.Albums
{
    public class AdminAlbumEditServiceModel : IMapFrom<Album>
    {
        public string Title { get; set; }

        public int AmountOfSong { get; set; }

        public decimal Price { get; set; }
    }
}
