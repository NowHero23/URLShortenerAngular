using Microsoft.EntityFrameworkCore;
using URLShortenerAngular.Server.Models;

namespace URLShortenerAngular.Server.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UrlItem> UrlItems { get; set; }
    }
}
