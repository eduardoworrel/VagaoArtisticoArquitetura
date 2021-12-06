using Microsoft.EntityFrameworkCore;
namespace Models
{
    public class TransladoContext : DbContext
    {
        public TransladoContext(DbContextOptions<TransladoContext> options) : base(options)
        {
        }
        public DbSet<Translado> Translado { get; set; }
    }
}