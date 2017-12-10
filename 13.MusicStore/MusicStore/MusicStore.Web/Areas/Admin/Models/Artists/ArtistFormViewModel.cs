using System.ComponentModel.DataAnnotations;
using static MusicStore.Data.DataConstants;

namespace MusicStore.Web.Areas.Admin.Models.Artists
{
    public class ArtistFormViewModel
    {
        [Required]
        [MinLength(ArtistNameMinLenght)]
        [MaxLength(ArtistNameMaxLenght)]
        public string Name { get; set; }
    }
}
