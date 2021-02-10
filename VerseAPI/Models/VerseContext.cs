using Microsoft.EntityFrameworkCore;
using VerseAPI.Models;

namespace VerseAPI.Models
{
    public class VerseContext : DbContext
    {
        public VerseContext(DbContextOptions<VerseContext> options)
            : base(options)
        {
        }

        public DbSet<Ship> Ship { get; set; }

        public DbSet<Planet> Planet { get; set; }
    }
}
