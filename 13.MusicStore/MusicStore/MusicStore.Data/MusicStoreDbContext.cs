using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data.ModelConfiguration;
using MusicStore.Data.Models;

namespace MusicStore.Data
{
    public class MusicStoreDbContext : IdentityDbContext<User>
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<SongAlbum> SongsAlbums { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SongAlbumConfiguration());
            builder.ApplyConfiguration(new ArtistConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
