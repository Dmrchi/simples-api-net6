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


    }
}
