using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BookShop.Data.DataConstants;

namespace BookShop.Api.Models.Categories
{
    public class CategoryRequestModel
    {
        [Required]
        [MaxLength(CategoryNameMaxLenght)]
        public string Name { get; set; }
    }
}
