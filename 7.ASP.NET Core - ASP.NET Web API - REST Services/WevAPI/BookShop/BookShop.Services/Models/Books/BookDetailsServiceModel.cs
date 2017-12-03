using BookShop.Common.Mapping;
using BookShop.Data.Models;
using BookShop.Services.Models.Authors;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;

namespace BookShop.Services.Models.Books
{
    public class BookDetailsServiceModel : BookByAuthorServiceModel, IMapFrom<Book>, IHaveCustomMapping
    {
        public string Auhtor { get; set; }

        public override void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Book, BookDetailsServiceModel>()
                 .ForMember(b => b.Categories, cfg => cfg.MapFrom(b => b.Categories.Select(c => c.Category.Name)))
                 .ForMember(b => b.Auhtor, cfg => cfg.MapFrom(b => b.Author.FirstName + " " + b.Author.LastName));
           
        }
    }
}
