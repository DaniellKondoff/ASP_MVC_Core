using BookShop.Common.Mapping;
using System;
using System.Collections.Generic;
using BookShop.Data.Models;
using System.Text;
using AutoMapper;
using System.Linq;

namespace BookShop.Services.Models.Authors
{
    public class AuthorDetailsServiceModel : IMapFrom<Author>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Books { get; set; } = new List<string>();

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Author, AuthorDetailsServiceModel>()
                .ForMember(a => a.Books, cfg => cfg.MapFrom(a => a.Books.Select(b => b.Title)));
        }
    }
}
