using MusicStore.Services.Models.Albums;
using MusicStore.Services.Models.Artists;
using MusicStore.Services.Models.Songs;
using System.Collections.Generic;

namespace MusicStore.Web.Models.HomeViewModels
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }

        public IEnumerable<ArtistListingServiceModel> Artists { get; set; } = new List<ArtistListingServiceModel>();

        public IEnumerable<SongListingServiceModel> Songs { get; set; } = new List<SongListingServiceModel>();

        public IEnumerable<AlbumsListingServiceModel> Albums { get; set; } = new List<AlbumsListingServiceModel>();
    }
}
