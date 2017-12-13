using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Services.Contracts;
using MusicStore.Services.Models.Artists;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Services.ServiceConstants;

namespace MusicStore.Services.Implementations
{
    public class ArtistService : IArtistService
    {
        private readonly MusicStoreDbContext db;

        public ArtistService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<ArtistListingServiceModel>> AllAsync(int page = 1)
        {
            return await this.db
                .Artists
                .OrderByDescending(a => a.Id)
                .Skip((page - 1) * ArtistsListingPageSize)
                .Take(ArtistsListingPageSize)
                .ProjectTo<ArtistListingServiceModel>()
                .ToListAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db.Artists.CountAsync();
        }
    }
}
