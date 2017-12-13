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

        public async Task<bool> AddSongToAlbumAsync(int id, int songId, int artistId)
        {
            var album = await this.db.Albums.FindAsync(id);
            if (album == null)
            {
                return false;
            }

            if (album.Songs.Count() >= album.AmountOfSongs)
            {
                return false;
            }

            var song = await this.db.Songs.FindAsync(songId);
            if (song == null)
            {
                return false;
            }

            if (album.ArtistId != artistId || song.ArtistId != artistId)
            {
                return false;
            }

            this.db.SongsAlbums.Add(new SongAlbum
            {
                AlbumId = album.Id,
                SongId = song.Id
            });

           await this.db.SaveChangesAsync();

            return true;
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

        public async Task<bool> DeleteAsync(int id)
        {
            var album = await this.db.Albums.FindAsync(id);

            if (album == null)
            {
                return false;
            }

            this.db.Remove(album);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<AdminAlbumDetailsServiceModel> DetailsAsync(int id)
        {
            return await this.db.Albums
                .Where(a => a.Id == id)
                .ProjectTo<AdminAlbumDetailsServiceModel>()
                .FirstOrDefaultAsync();
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

        public async Task<int> GetArtistIdByAlbumId(int Id)
        {
            var album = await this.db.Albums.FindAsync(Id);

            return album.ArtistId;
        }

        public async Task<AdminAlbumEditServiceModel> GetByIdAsync(int id)
        {
            return await this.db.Albums
                .Where(a => a.Id == id)
                .ProjectTo<AdminAlbumEditServiceModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsSongAddedAsync(int id, int songId)
        {
            return await this.db.SongsAlbums.AnyAsync(sa => sa.AlbumId == id && sa.SongId == songId);
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
