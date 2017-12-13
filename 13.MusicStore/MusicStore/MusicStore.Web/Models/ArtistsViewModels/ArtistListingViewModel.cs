using MusicStore.Services;
using MusicStore.Services.Models.Artists;
using System;
using System.Collections.Generic;

namespace MusicStore.Web.Models.ArtistsViewModels
{
    public class ArtistListingViewModel
    {
        public IEnumerable<ArtistListingServiceModel> AllArtists { get; set; }

        public int TotalArtists { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalArtists / ServiceConstants.ArtistsListingPageSize);

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
