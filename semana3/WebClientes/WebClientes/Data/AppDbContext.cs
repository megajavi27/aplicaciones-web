using Microsoft.EntityFrameworkCore;
using WebClientes.Models;

namespace WebClientes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<TipoIdentificacionModel> TiposIdentificacion { get; set; }
        public DbSet<GeneroModel> Generos { get; set; }
        public DbSet<EstadoCivilModel> EstadosCiviles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar PKs de tipo char como char(1) para MySQL
            modelBuilder.Entity<TipoIdentificacionModel>()
                .Property(e => e.ctipoidentificacion)
                .HasColumnType("char(1)");

            modelBuilder.Entity<GeneroModel>()
                .Property(e => e.cgenero)
                .HasColumnType("char(1)");

            modelBuilder.Entity<EstadoCivilModel>()
                .Property(e => e.cestadocivil)
                .HasColumnType("char(1)");

            // También configurar las FKs char(1) en ClienteModel
            modelBuilder.Entity<ClienteModel>()
                .Property(e => e.ctipoidentificacion)
                .HasColumnType("char(1)");

            modelBuilder.Entity<ClienteModel>()
                .Property(e => e.cgenero)
                .HasColumnType("char(1)");

            modelBuilder.Entity<ClienteModel>()
                .Property(e => e.cestadocivil)
                .HasColumnType("char(1)");

            // --- Relaciones explícitas (HasOne / WithMany / HasForeignKey) ---
            modelBuilder.Entity<ClienteModel>()
                .HasOne(c => c.TipoIdentificacion)
                .WithMany()
                .HasForeignKey(c => c.ctipoidentificacion)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClienteModel>()
                .HasOne(c => c.Genero)
                .WithMany()
                .HasForeignKey(c => c.cgenero)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClienteModel>()
                .HasOne(c => c.EstadoCivil)
                .WithMany()
                .HasForeignKey(c => c.cestadocivil)
                .OnDelete(DeleteBehavior.Restrict);

            // Datos iniciales - TiposIdentificacion
            modelBuilder.Entity<TipoIdentificacionModel>().HasData(
                new TipoIdentificacionModel { ctipoidentificacion = 'c', Descripcion = "Cédula" },
                new TipoIdentificacionModel { ctipoidentificacion = 'p', Descripcion = "Pasaporte" }
            );

            // Datos iniciales - Generos
            modelBuilder.Entity<GeneroModel>().HasData(
                new GeneroModel { cgenero = 'h', Descripcion = "Hombre" },
                new GeneroModel { cgenero = 'm', Descripcion = "Mujer" }
            );

            // Datos iniciales - EstadosCiviles
            modelBuilder.Entity<EstadoCivilModel>().HasData(
                new EstadoCivilModel { cestadocivil = 'c', Descripcion = "Casado" },
                new EstadoCivilModel { cestadocivil = 's', Descripcion = "Soltero" },
                new EstadoCivilModel { cestadocivil = 'v', Descripcion = "Viudo" },
                new EstadoCivilModel { cestadocivil = 'd', Descripcion = "Divorciado" }
            );
        }
    }
}
