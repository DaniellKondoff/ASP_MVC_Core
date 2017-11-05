using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarDealer.Data.Models.ModelConfigurations
{
    public class PartCarsConfiguration : IEntityTypeConfiguration<PartCars>
    {
        public void Configure(EntityTypeBuilder<PartCars> builder)
        {
            builder
                .HasKey(pc => new { pc.PartId, pc.CarId });

            builder
                .HasOne(pc => pc.Car)
                .WithMany(c => c.Parts)
                .HasForeignKey(pc => pc.CarId);

            builder
               .HasOne(pc => pc.Part)
               .WithMany(p => p.Cars)
               .HasForeignKey(pc => pc.PartId);
        }
    }
}
