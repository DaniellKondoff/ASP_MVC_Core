using AutoMapper;
using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Models.Songs
{
    public class SongDetailsServiceModel : IMapFrom<Song>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Duration { get; set; }

        public decimal Price { get; set; }

        public string Artist { get; set; }

        public Ganre Ganre { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Song, SongDetailsServiceModel>()
                .ForMember(s => s.Artist, cfg => cfg.MapFrom(s => s.Artist.Name));
        }
    }
}
