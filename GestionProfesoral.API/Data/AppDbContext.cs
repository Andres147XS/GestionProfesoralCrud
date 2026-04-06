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
        public DbSet<Docente> Docentes { get; set; } = null!;
        public DbSet<RedDocente> RedesDocentes { get; set; } = null!;
        public DbSet<Reconocimiento> Reconocimientos { get; set; } = null!;
        public DbSet<Experiencia> Experiencias { get; set; } = null!;
        public DbSet<EvaluacionDocente> EvaluacionesDocentes { get; set; } = null!;
        public DbSet<DocenteDepartamento> DocentesDepartamentos { get; set; } = null!;
        public DbSet<Alianza> Alianzas { get; set; } = null!;
        public DbSet<EstudiosRealizados> EstudiosRealizados { get; set; } = null!;
        public DbSet<EstudioAC> EstudiosAC { get; set; } = null!;
        public DbSet<InteresesFuturos> InteresesFuturos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beca>().HasKey(b => b.Estudios);
            modelBuilder.Entity<Beca>().Property(b => b.Estudios).ValueGeneratedNever();

            modelBuilder.Entity<ApoyoProfesoral>().HasKey(a => a.Estudios);
            modelBuilder.Entity<ApoyoProfesoral>().Property(a => a.Estudios).ValueGeneratedNever();
            modelBuilder.Entity<Aliado>().HasKey(al => al.Nit);
            modelBuilder.Entity<Aliado>().Property(al => al.Nit).ValueGeneratedNever();
            modelBuilder.Entity<Docente>().HasKey(d => d.Cedula);

            // Forzar nombres de tablas en plural para evitar discrepancias
            modelBuilder.Entity<Docente>().ToTable("Docentes");
            modelBuilder.Entity<Red>().ToTable("Redes");
            modelBuilder.Entity<Aliado>().ToTable("Aliados");
            modelBuilder.Entity<Beca>().ToTable("Becas");
            modelBuilder.Entity<ApoyoProfesoral>().ToTable("Apoyos");
            modelBuilder.Entity<Reconocimiento>().ToTable("Reconocimientos");
            modelBuilder.Entity<Experiencia>().ToTable("Experiencias");
            modelBuilder.Entity<EvaluacionDocente>().ToTable("EvaluacionesDocentes");
            modelBuilder.Entity<Alianza>().ToTable("Alianzas");
            modelBuilder.Entity<EstudiosRealizados>().ToTable("EstudiosRealizados");
            modelBuilder.Entity<RedDocente>().ToTable("RedesDocentes");
            modelBuilder.Entity<DocenteDepartamento>().ToTable("DocentesDepartamentos");
            modelBuilder.Entity<EstudioAC>().ToTable("EstudiosAC");
            modelBuilder.Entity<InteresesFuturos>().ToTable("InteresesFuturos");

            modelBuilder.Entity<RedDocente>()
                .HasKey(rd => new { rd.RedId, rd.DocenteCedula });

            modelBuilder.Entity<DocenteDepartamento>()
                .HasKey(dd => new { dd.DocenteCedula, dd.DepartamentoId });

            modelBuilder.Entity<EstudioAC>()
                .HasKey(eac => new { eac.EstudioId, eac.AreaConocimientoId });

            modelBuilder.Entity<InteresesFuturos>()
                .HasKey(ifut => new { ifut.DocenteCedula, ifut.TerminoClave });

            base.OnModelCreating(modelBuilder);
        }
    }
}
