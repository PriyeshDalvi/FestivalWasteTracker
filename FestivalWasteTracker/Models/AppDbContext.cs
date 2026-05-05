using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FestivalWasteTracker.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<WasteRecord> WasteRecords { get; set; }
    }
}
