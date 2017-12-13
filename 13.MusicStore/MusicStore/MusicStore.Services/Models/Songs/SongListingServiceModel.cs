using AutoMapper;
using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Models.Songs
{
    public class SongListingServiceModel : IMapFrom<Song>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Artist { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Song, SongListingServiceModel>()
                .ForMember(s => s.Artist, cfg => cfg.MapFrom(s => s.Artist.Name));
        }
    }
}
