﻿using AutoMapper;
using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Models.Artists
{
    public class ArtistListingServiceModel : IMapFrom<Artist>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Songs { get; set; }

        public int Albums { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Artist, ArtistListingServiceModel>()
                 .ForMember(a => a.Songs, cfg => cfg.MapFrom(ar => ar.Songs.Count))
                 .ForMember(a => a.Albums, cfg => cfg.MapFrom(ar => ar.Albums.Count));
        }
    }
}
