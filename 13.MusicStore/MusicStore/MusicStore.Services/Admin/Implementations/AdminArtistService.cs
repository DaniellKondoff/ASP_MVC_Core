using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Data.Models;
using MusicStore.Services.Admin.Contracts;
using MusicStore.Services.Admin.Models.Artists;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Services.ServiceConstants;

namespace MusicStore.Services.Admin.Implementations
{
    public class AdminArtistService : IAdminArtistService
    {
        private readonly MusicStoreDbContext db;

        public AdminArtistService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AdminArtistListingServiceModel>> AllAsync(int page = 1)
        {
            return await this.db
                .Artists
                .OrderByDescending(a => a.Id)
                .Skip((page -1) * AdminArtistsListingPageSize)
                .Take(AdminArtistsListingPageSize)
                .ProjectTo<AdminArtistListingServiceModel>()
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminBaseArtistServiceModel>> AllBasicAsync()
        {
            return await this.db.Artists
                    .ProjectTo<AdminBaseArtistServiceModel>()
                    .ToListAsync();
        }

        public async Task CreateAsync(string name)
        {
            if (name == null)
            {
                return;
            }

            var artist = new Artist
            {
                Name = name
            };

            await this.db.Artists.AddAsync(artist);
            await this.db.SaveChangesAsync();
            
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var artist = await this.db.Artists.FindAsync(id);

            if (artist == null)
            {
                return false;
            }

            this.db.Artists.Remove(artist);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(int id, string name)
        {
            var artist = await this.db.Artists.FindAsync(id);

            if (artist == null)
            {
                return false;
            }

            artist.Name = name;

            this.db.Artists.Update(artist);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistAsync(int artistId)
        {
            return await this.db.Artists
                .AnyAsync(a => a.Id == artistId);
        }

        public async Task<AdminBaseArtistServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Artists
                .Where(a => a.Id == id)
                .ProjectTo<AdminBaseArtistServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db
                .Artists.CountAsync();
        }
    }
}
