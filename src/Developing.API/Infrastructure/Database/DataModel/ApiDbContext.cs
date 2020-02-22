using Microsoft.EntityFrameworkCore;

namespace Developing.API.Infrastructure.Database.DataModel
{
    public sealed class ApiDbContext : DbContext
    {
        public const string Schema = "dev_netcore";

        public ApiDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
        }
    }
}
