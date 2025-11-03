using Devlivery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Devlivery.API.Data
{
    public class VagaContext : DbContext
    {
        public VagaContext(DbContextOptions<VagaContext> opts) : base(opts)
        {
            
        }
        
        public DbSet<Vaga> Vagas { get; set; }
        /* Errado Corrigir : 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Vaga>()
                .HasMany(c => c.Profissional)
                .WithOne(a => a.Vaga!)
                .HasForeignKey(a => a.IdRestaurante);

            modelBuilder
                .Entity<Profissional>()
                .HasOne(a => a.Vaga)
                .WithMany(c => c.Profissional)
                .HasForeignKey(a => a.IdRestaurante);
        }*/

    }
}
