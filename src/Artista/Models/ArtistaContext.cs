using Microsoft.EntityFrameworkCore;
namespace Models
{
    public class ArtistaContext : DbContext
    {
        public ArtistaContext(DbContextOptions<ArtistaContext> options) : base(options)
        {
        }
        public DbSet<Artista> Artista { get; set; }
    }
}