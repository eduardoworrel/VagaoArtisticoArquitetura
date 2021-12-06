using Microsoft.EntityFrameworkCore;
namespace Models
{
    public class TremContext : DbContext
    {
        public TremContext(DbContextOptions<TremContext> options) : base(options)
        {
        }
        public DbSet<Trem> Trem { get; set; }
    }
}