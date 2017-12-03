using BookShop.Common.Mapping;
using BookShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Services.Models.Categories
{
    public class CategoryListingServiceModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
