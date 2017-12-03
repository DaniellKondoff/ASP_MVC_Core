using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static BookShop.Data.DataConstants;

namespace BookShop.Data.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(AuthorNameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(AuthorNameMaxLenght)]
        public string LastName { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
