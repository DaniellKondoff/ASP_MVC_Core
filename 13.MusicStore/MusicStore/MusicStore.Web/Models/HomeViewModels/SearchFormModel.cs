using System.ComponentModel.DataAnnotations;

namespace MusicStore.Web.Models.HomeViewModels
{
    public class SearchFormModel
    {
        public string SearchText { get; set; }

        [Display(Name = "Search In Artists")]
        public bool SearchInArtists { get; set; } = true;

        [Display(Name = "Search In Songs")]
        public bool SearchInSongs { get; set; } = true;

        [Display(Name = "Search In Albums")]
        public bool SearchInAlbums { get; set; } = true;
    }
}
