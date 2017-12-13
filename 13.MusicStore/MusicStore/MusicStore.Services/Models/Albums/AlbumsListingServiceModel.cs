using AutoMapper;
using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Models.Albums
{
    public class AlbumsListingServiceModel : IMapFrom<Album>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int AmountOfSongs { get; set; }

        public decimal Price { get; set; }

        public string Artist { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Album, AlbumsListingServiceModel>()
                .ForMember(a => a.Artist, cfg => cfg.MapFrom(a => a.Artist.Name));
        }
    }
}
