using Microsoft.EntityFrameworkCore;
using ClientsDtosWebService.Models;

namespace ClientsDtosWebService.Data
{
    public class AppClientDbContext: DbContext
    {
        public AppClientDbContext(DbContextOptions<AppClientDbContext> options) : base(options)
        {
        }
        public DbSet<Client> Clients => Set<Client>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ігноруємо всі властивості, які потрібні лише для Azure Table Storage
            modelBuilder.Entity<Client>().Ignore(c => c.PartitionKey);
            modelBuilder.Entity<Client>().Ignore(c => c.RowKey);
            modelBuilder.Entity<Client>().Ignore(c => c.Timestamp);
            modelBuilder.Entity<Client>().Ignore(c => c.ETag);
        }
    }
}
