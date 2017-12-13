using AutoMapper;
using MusicStore.Core.Mapping;
using MusicStore.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusicStore.Services.Models.Albums
{
    public class AlbumDetailsServiceModel : IMapFrom<Album>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public IEnumerable<string> Songs { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Album, AlbumDetailsServiceModel>()
                .ForMember(a => a.Artist, cfg => cfg.MapFrom(a => a.Artist.Name))
                .ForMember(a => a.Songs, cfg => cfg.MapFrom(a => a.Songs.Select(s => s.Song.Name)));
        }
    }
}
