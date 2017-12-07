using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Data.Models;

namespace MusicStore.Data.ModelConfiguration
{
    public class SongAlbumConfiguration : IEntityTypeConfiguration<SongAlbum>
    {
        public void Configure(EntityTypeBuilder<SongAlbum> builder)
        {
            builder
                .HasKey(sa => new { sa.SongId, sa.AlbumId });

            builder
                .HasOne(sa => sa.Song)
                .WithMany(s => s.Albums)
                .HasForeignKey(sa => sa.SongId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(sa => sa.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(sa => sa.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
