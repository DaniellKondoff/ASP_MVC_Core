using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicStore.Data.DataConstants;

namespace MusicStore.Web.Areas.Admin.Models.Albums
{
    public class AlbumFormViewModel
    {
        [Required]
        [MinLength(AlbumTitleMinLenght)]
        [MaxLength(AlbumTitleMaxLenght)]
        public string Title { get; set; }

        [Range(AlbumMinAmountOfSongs, AlbumMaxAmountOfSongs)]
        public int AmountOfSongs { get; set; }

        [Required]
        [Display(Name = "Artist")]
        public int ArtistId { get; set; }

        public IEnumerable<SelectListItem> Artists { get; set; }

        [Range(AlbumMinPrice, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
