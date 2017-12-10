using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStore.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicStore.Data.DataConstants;

namespace MusicStore.Web.Areas.Admin.Models.Songs
{
    public class SongFormViewModel
    {
        [Required]
        [MinLength(SongTitleMinLenght)]
        [MaxLength(SongTitleMaxLenght)]
        public string Name { get; set; }

        public double Duration { get; set; }

        [Range(SongMinPrice, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Artist")]
        public int ArtistId { get; set; }

        public IEnumerable<SelectListItem> Artists { get; set; }

        public Ganre Ganre { get; set; }
    }
}
