using Microsoft.EntityFrameworkCore;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Data
{
    // AppDbContext representa la base de datos para Entity Framework Core
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tablas
        public DbSet<Red> Redes { get; set; } = null!;
        public DbSet<Beca> Becas { get; set; } = null!;
        public DbSet<ApoyoProfesoral> Apoyos { get; set; } = null!;
        public DbSet<Aliado> Aliados { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beca>().HasKey(b => b.Estudios);
            modelBuilder.Entity<ApoyoProfesoral>().HasKey(a => a.Estudios);
            modelBuilder.Entity<Aliado>().HasKey(al => al.Nit);

            base.OnModelCreating(modelBuilder);
        }
    }
}
