using Microsoft.EntityFrameworkCore;
using Villarin_SYSARCH.Models;

namespace Villarin_SYSARCH.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<CurrentSitIn> CurrentSitIns { get; set; }
    }
}
