using BookShop.Common.Mapping;
using BookShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Services.Models.Books
{
    public class BookListingServiceModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

    }
}
