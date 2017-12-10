using MusicStore.Core.Mapping;
using MusicStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace MusicStore.Services.Admin.Models.Artists
{
    public class AdminArtistListingServiceModel : IMapFrom<Artist>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Songs { get; set; }

        public int Albums { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Artist, AdminArtistListingServiceModel>()
                 .ForMember(a => a.Songs, cfg => cfg.MapFrom(ar => ar.Songs.Count))
                 .ForMember(a => a.Albums, cfg => cfg.MapFrom(ar => ar.Albums.Count));
        }
    }
}
