using ApiCevicheria.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCevicheria.Data
{
    public class DataContext : DbContext
    {
        // Constructor para configurar la cadena de conexión
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Platillo> Platillos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Estado> Estados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la clave primaria compuesta para la clase Orden
            modelBuilder.Entity<Orden>().HasKey(o => new { o.ID, o.RegistroID });

            // Otros ajustes y configuraciones de modelos si los tienes

            base.OnModelCreating(modelBuilder);
        }
    }
}
