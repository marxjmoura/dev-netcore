using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Developing.API.Infrastructure.Database.DataModel.Brands
{
    public static class BrandMap
    {
        public static void Configure(this EntityTypeBuilder<Brand> brand)
        {
            brand.ToTable("brand");

            brand.HasKey(p => p.Id)
                .HasName("pk_brand");

            brand.HasIndex(p => p.Name)
                .IsUnique()
                .HasName("idx_brand_name");

            brand.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            brand.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(30)
                .IsRequired();

            brand.HasMany(p => p.Models)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId)
                .HasConstraintName("fk_model__brand");
        }
    }
}
