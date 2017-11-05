using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealer.Data.Models.ModelConfigurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder
               .HasOne(s => s.Car)
               .WithMany(c => c.Sales)
               .HasForeignKey(s => s.CarId);


            builder
               .HasOne(s => s.Customer)
               .WithMany(c => c.Sales)
               .HasForeignKey(s => s.CustomerId);
        }
    }
}
