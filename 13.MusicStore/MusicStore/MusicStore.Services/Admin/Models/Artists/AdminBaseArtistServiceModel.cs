using MusicStore.Core.Mapping;
using MusicStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicStore.Services.Admin.Models.Artists
{
    public class AdminBaseArtistServiceModel : IMapFrom<Artist>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
