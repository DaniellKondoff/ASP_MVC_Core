using MusicStore.Core.Mapping;
using MusicStore.Data.Models;

namespace MusicStore.Services.Admin.Models.Songs
{
    public class AdminSongBaseServiceModel : IMapFrom<Song>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
