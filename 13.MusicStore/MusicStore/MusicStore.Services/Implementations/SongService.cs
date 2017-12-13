using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Services.Contracts;
using MusicStore.Services.Models.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MusicStore.Services.ServiceConstants;


namespace MusicStore.Services.Implementations
{
    public class SongService : ISongService
    {
        private readonly MusicStoreDbContext db;

        public SongService(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<SongListingServiceModel>> AllAsync(int page = 1)
        {
            return await this.db.Songs
                .OrderByDescending(a => a.Id)
                .Skip((page - 1) * SongListingPageSize)
                .Take(SongListingPageSize)
                .ProjectTo<SongListingServiceModel>()
                .ToListAsync();
        }

        public async Task<SongDetailsServiceModel> DetailsAsync(int id)
        {
            return await this.db.Songs
                .Where(s => s.Id == id)
                .ProjectTo<SongDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> TotalAsync()
        {
            return await this.db.Songs.CountAsync();
        }
    }
}
