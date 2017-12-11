using MusicStore.Services;
using MusicStore.Services.Admin.Models.Albums;
using System;
using System.Collections.Generic;

namespace MusicStore.Web.Areas.Admin.Models.Albums
{
    public class AlbumListingViewModel
    {
        public IEnumerable<AdminAlbumsListingServiceModel> AllAlbums { get; set; }

        public int TotalAlbums { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalAlbums / ServiceConstants.AdminAlbumsListingPageSize);

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
