using Developing.API.Infrastructure.Database.DataModel.Brands;
using Developing.API.Infrastructure.Database.DataModel.Models;
using Developing.API.Infrastructure.Database.DataModel.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Developing.API.Infrastructure.Database.DataModel
{
    public sealed class ApiDbContext : DbContext
    {
        public const string Schema = "dev_netcore";

        public ApiDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<Brand>().Configure();
            modelBuilder.Entity<Model>().Configure();
            modelBuilder.Entity<Vehicle>().Configure();
        }
    }
}
