using Microsoft.EntityFrameworkCore;
namespace Models
{
    public class ApresentacaoContext : DbContext
    {
        public ApresentacaoContext(DbContextOptions<ApresentacaoContext> options) : base(options)
        {
        }
        public DbSet<Apresentacao> Apresentacao { get; set; }
        public DbSet<ApresentacaoStatus> ApresentacaoStatus { get; set; }
    }
}