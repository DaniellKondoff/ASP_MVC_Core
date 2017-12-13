using MusicStore.Services.Models.Artists;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStore.Services.Contracts
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistListingServiceModel>> AllAsync(int page = 1);

        Task<int> TotalAsync();
    }
}
