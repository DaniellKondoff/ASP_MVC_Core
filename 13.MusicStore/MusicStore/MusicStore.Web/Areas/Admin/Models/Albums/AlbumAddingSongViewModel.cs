using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Areas.Admin.Models.Albums
{
    public class AlbumAddingSongViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Song")]
        public int SongId { get; set; }

        public IEnumerable<SelectListItem> Songs { get; set; }
    }
}
