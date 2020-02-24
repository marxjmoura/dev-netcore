using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Developing.API.Infrastructure.Database.DataModel.Vehicles
{
    public static class VehicleMap
    {
        public static void Configure(this EntityTypeBuilder<Vehicle> vehicle)
        {
            vehicle.ToTable("vehicle");

            vehicle.HasKey(p => p.Id).HasName("pk_vehicle");

            vehicle.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            vehicle.Property(p => p.ModelId)
                .HasColumnName("model_id")
                .IsRequired();

            vehicle.Property(p => p.ModelYear)
                .HasColumnName("model_year")
                .HasMaxLength(4)
                .IsRequired();

            vehicle.Property(p => p.Fuel)
                .HasColumnName("fuel")
                .HasMaxLength(20)
                .IsRequired();

            vehicle.Property(p => p.Value)
                .HasColumnName("value")
                .HasColumnType("decimal(8,2)")
                .IsRequired();
        }
    }
}
