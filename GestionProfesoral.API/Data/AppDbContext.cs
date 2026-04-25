using Microsoft.EntityFrameworkCore;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beca>().HasKey(b => b.Estudios);
            modelBuilder.Entity<Beca>().Property(b => b.Estudios).ValueGeneratedNever();
            modelBuilder.Entity<ApoyoProfesoral>().HasKey(a => a.Estudios);
            modelBuilder.Entity<ApoyoProfesoral>().Property(a => a.Estudios).ValueGeneratedNever();
            modelBuilder.Entity<Aliado>().HasKey(al => al.Nit);
            modelBuilder.Entity<Aliado>().Property(al => al.Nit).ValueGeneratedNever();
            modelBuilder.Entity<Docente>().HasKey(d => d.Cedula);

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
            modelBuilder.Entity<Rol>().ToTable("Roles");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

            modelBuilder.Entity<RedDocente>().HasKey(rd => new { rd.RedId, rd.DocenteCedula });
            modelBuilder.Entity<DocenteDepartamento>().HasKey(dd => new { dd.DocenteCedula, dd.DepartamentoId });
            modelBuilder.Entity<EstudioAC>().HasKey(eac => new { eac.EstudioId, eac.AreaConocimientoId });
            modelBuilder.Entity<InteresesFuturos>().HasKey(ifut => new { ifut.DocenteCedula, ifut.TerminoClave });

            modelBuilder.Entity<Usuario>().HasIndex(u => u.NombreUsuario).IsUnique();
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total al sistema incluyendo CRUD de usuarios y roles" },
                new Rol { Id = 2, Nombre = "Docente", Descripcion = "Acceso a modulos de gestion profesoral" },
                new Rol { Id = 3, Nombre = "Consultor", Descripcion = "Solo lectura de informacion" }
            );

            // admin user - password: Admin123!
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    NombreUsuario = "admin",
                    PasswordHash = "$2b$10$/pEsxDsbMoUzQsc9FSC9l.S0DqMgRBH81tArmW76arHE9GovZtP1q",
                    Correo = "admin@gestionprofesoral.edu.co",
                    RolId = 1,
                    Activo = true
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}