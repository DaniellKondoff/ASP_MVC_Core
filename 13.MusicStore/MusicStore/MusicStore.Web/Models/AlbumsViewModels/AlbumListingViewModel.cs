using MusicStore.Services;
using MusicStore.Services.Models.Albums;
using System;
using System.Collections.Generic;

namespace MusicStore.Web.Models.AlbumsViewModels
{
    public class AlbumListingViewModel
    {
        public IEnumerable<AlbumsListingServiceModel> AllAlbums { get; set; }

        public int TotalAlbums { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalAlbums / ServiceConstants.AlbumsListingPageSize);

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
