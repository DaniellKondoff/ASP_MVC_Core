using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Services.Admin.Models.Albums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Services.ServiceConstants;

namespace MusicStore.Services.Admin.Implementations
{
    public class AdminAlbumService : IAdminAlbumService
    {
        private readonly MusicStoreDbContext db;

        public AdminAlbumService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task CreateAsync(string title, decimal price, int amountOfSongs, int artistId)
        {
            var album = new Album
            {
                Title = title,
                Price = price,
                AmountOfSongs = amountOfSongs,
                ArtistId = artistId
            };

            await this.db.Albums.AddAsync(album);
            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string title, decimal price, int amountOfSongs, int artistId)
        {
            var album = await this.db.Albums.FindAsync(id);

            album.Title = title;
            album.Price = price;
            album.AmountOfSongs = amountOfSongs;
            album.ArtistId = artistId;

            this.db.Albums.Update(album);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await this.db.Albums.AnyAsync(a => a.Id == id);
        }

        public async Task<AdminAlbumEditServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Albums
                .Where(a => a.Id == id)
                .ProjectTo<AdminAlbumEditServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AdminAlbumsListingServiceModel>> ListAllAsync(int page = 1)
        {
            return await this.db
                .Albums
                .OrderByDescending(a => a.Id)
                .Skip((page - 1) * AdminAlbumsListingPageSize)
                .Take(AdminAlbumsListingPageSize)
                .ProjectTo<AdminAlbumsListingServiceModel>()
                .ToListAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db.Albums.CountAsync();
        }
    }
}
