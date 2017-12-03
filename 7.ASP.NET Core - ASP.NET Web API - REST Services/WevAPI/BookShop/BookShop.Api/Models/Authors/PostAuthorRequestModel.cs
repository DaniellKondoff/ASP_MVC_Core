using System.ComponentModel.DataAnnotations;
using static BookShop.Data.DataConstants;

namespace BookShop.Api.Models.Authors
{
    public class PostAuthorRequestModel
    {
        [Required]
        [MaxLength(AuthorNameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(AuthorNameMaxLenght)]
        public string LastName { get; set; }
    }
}
