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

        public async Task<IEnumerable<AlbumShoppingDetailsServiceModels>> AlbumsShoppingDetails(IEnumerable<int> itemAlbumsIds)
        {
            return await this.db
               .Albums
               .Where(s => itemAlbumsIds.Contains(s.Id))
               .ProjectTo<AlbumShoppingDetailsServiceModels>()
               .ToListAsync();
        }

        public async Task<AlbumDetailsServiceModel> DetailsAsync(int id)
        {
            return await this.db.Albums
                .Where(a => a.Id == id)
                .ProjectTo<AlbumDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AlbumsListingServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;

            return await this.db
                .Albums
                .OrderByDescending(s => s.Id)
                .Where(s => s.Title.ToLower().Contains(searchText.ToLower()))
                .ProjectTo<AlbumsListingServiceModel>()
                .ToListAsync();
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
