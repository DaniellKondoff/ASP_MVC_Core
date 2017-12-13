using MusicStore.Services.Models.Songs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Contracts
{
    public interface ISongService
    {
        Task<IEnumerable<SongListingServiceModel>> AllAsync(int page = 1);

        Task<int> TotalAsync();

        Task<SongDetailsServiceModel> DetailsAsync(int id);
    }
}
