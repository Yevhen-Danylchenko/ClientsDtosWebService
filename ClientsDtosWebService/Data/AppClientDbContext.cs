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
    }
}
