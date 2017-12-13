using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Services.Contracts;
using MusicStore.Services.Models.Albums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Services.ServiceConstants;

namespace MusicStore.Services.Implementations
{
    public class AlbumService : IAlbumService
    {
        private readonly MusicStoreDbContext db;

        public AlbumService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<AlbumDetailsServiceModel> DetailsAsync(int id)
        {
            return await this.db.Albums
                .Where(a => a.Id == id)
                .ProjectTo<AlbumDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AlbumsListingServiceModel>> ListAllAsync(int page = 1)
        {
            return await this.db
               .Albums
               .OrderByDescending(a => a.Id)
               .Skip((page - 1) * AlbumsListingPageSize)
               .Take(AlbumsListingPageSize)
               .ProjectTo<AlbumsListingServiceModel>()
               .ToListAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db.Albums.CountAsync();
        }
    }
}
