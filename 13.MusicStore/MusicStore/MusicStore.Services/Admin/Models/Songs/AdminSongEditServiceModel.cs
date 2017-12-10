using AutoMapper;
using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Admin.Models.Songs
{
    public class AdminSongEditServiceModel : IMapFrom<Song>, IHaveCustomMapping
    {
        public string Name { get; set; }

        public string Artist { get; set; }

        public double Duration { get; set; }

        public decimal Price { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Song, AdminSongEditServiceModel>()
                .ForMember(s => s.Artist, cfg => cfg.MapFrom(s => s.Artist.Name));
        }
    }
}
