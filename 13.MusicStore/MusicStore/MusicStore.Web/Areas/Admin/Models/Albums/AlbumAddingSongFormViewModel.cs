using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Web.Areas.Admin.Models.Albums
{
    public class AlbumAddingSongFormViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int SongId { get; set; }
    }
}
