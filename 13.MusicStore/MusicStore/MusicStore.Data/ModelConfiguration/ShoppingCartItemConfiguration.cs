using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Data.Models;

namespace MusicStore.Data.ModelConfiguration
{
    class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder
                .HasOne(sp => sp.Song)
                .WithMany(s => s.ShoppingCartItems)
                .HasForeignKey(sp => sp.SongId);
        }
    }
}
