using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealer.Data.Models.ModelConfigurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder
                 .HasMany(s => s.Parts)
                 .WithOne(p => p.Supplier)
                 .HasForeignKey(p => p.SupplierId);
        }
    }
}
