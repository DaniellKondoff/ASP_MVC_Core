using MusicStore.Core.Mapping;
using MusicStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace MusicStore.Services.Models.Albums
{
    public class AlbumShoppingDetailsServiceModels : IMapFrom<Album>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Album, AlbumShoppingDetailsServiceModels>()
                .ForMember(a => a.Name, cfg => cfg.MapFrom(a => a.Title));
        }
    }
}
