using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Developing.API.Infrastructure.Database.DataModel.Models
{
    public static class ModelMap
    {
        public static void Configure(this EntityTypeBuilder<Model> model)
        {
            model.ToTable("model");

            model.HasKey(p => p.Id)
                .HasName("pk_model");

            model.HasIndex(p => p.Name)
                .IsUnique()
                .HasName("idx_model_name");

            model.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            model.Property(p => p.BrandId)
                .HasColumnName("brand_id")
                .IsRequired();

            model.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(80)
                .IsRequired();
        }
    }
}
