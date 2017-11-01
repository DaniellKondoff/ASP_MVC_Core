using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CatsServer.Data
{
    public class Cat
    {
        private const int StringMaxLength = 50;
        private const int StringMinLenght = 1;

        public int Id { get; set; }

        [Required]
        [MinLength(StringMinLenght)]
        [MaxLength(StringMaxLength)]
        public string Name { get; set; }

        [Range(0,20)]
        public int Age { get; set; }

        [Required]
        [MaxLength(StringMaxLength)]
        [MinLength(StringMinLenght)]
        public string Breed { get; set; }

        [Required]
        [MaxLength(2000)]
        [MinLength(10)]
        public string ImageUrl { get; set; }
    }
}
