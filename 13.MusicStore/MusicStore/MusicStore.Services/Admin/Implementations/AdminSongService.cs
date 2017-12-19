using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Services.Admin.Models.Songs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Services.ServiceConstants;

namespace MusicStore.Services.Admin.Implementations
{
    public class AdminSongService : IAdminSongService
    {
        private readonly MusicStoreDbContext db;

        public AdminSongService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AdminSongListingServiceModel>> AllAsync(int page = 1)
        {
            return await this.db.Songs
                .OrderByDescending(a => a.Id)
                .Skip((page - 1) * AdminSongListingPageSize)
                .Take(AdminSongListingPageSize)
                .ProjectTo<AdminSongListingServiceModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminSongBaseServiceModel>> AllBasicAsync(int id)
        {
            return await this.db.Songs
                .OrderByDescending(s => s.Id)
                .Where(s => s.ArtistId == id)
                .ProjectTo<AdminSongBaseServiceModel>()
                .ToListAsync();
        }

        public async Task CreateAsync(string name, decimal price, double duration, int artistId, Ganre ganre)
        {
            var song = new Song
            {
                Name = name,
                Price = price,
                Duration = duration,
                ArtistId = artistId,
                Ganre = ganre
            };

            await this.db.Songs.AddAsync(song);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var song = await this.db.Songs.FindAsync(id);

            if (song == null)
            {
                return false;
            }

            this.db.Songs.Remove(song);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<AdminSongDetailsServiceModel> DetailsAsync(int id)
        {
            return await this.db.Songs
                .Where(s => s.Id == id)
                .ProjectTo<AdminSongDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task EditAsync(int id, string name, decimal price, double duration, int artistId, Ganre ganre)
        {
            var song = await this.db.Songs.FindAsync(id);

            if (song == null)
            {
                return;
            }

            song.Name = name;
            song.Price = price;
            song.Duration = duration;
            song.ArtistId = artistId;
            song.Ganre = ganre;

            this.db.Songs.Update(song);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await this.db.Songs
                .AnyAsync(s => s.Id == id);
        }

        public async Task<AdminSongEditServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Songs
                .Where(s => s.Id == id)
                .ProjectTo<AdminSongEditServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db.Songs.CountAsync();
        }
    }
}
