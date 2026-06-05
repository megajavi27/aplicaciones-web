using Microsoft.EntityFrameworkCore;

namespace EvaluacionParcial1.Models
{
    public class EscuelaDBContext : DbContext
    {
        public EscuelaDBContext(DbContextOptions<EscuelaDBContext> op) : base(op) { }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<EstudianteClase> EstudianteClases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Clase>()
                .HasOne(c => c.Profesor)
                .WithMany(p => p.Clases)
                .HasForeignKey(c => c.ProfesorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EstudianteClase>()
                .HasOne(ec => ec.Estudiante)
                .WithMany(e => e.EstudianteClases)
                .HasForeignKey(ec => ec.EstudianteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EstudianteClase>()
                .HasOne(ec => ec.Clase)
                .WithMany(c => c.EstudianteClases)
                .HasForeignKey(ec => ec.ClaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
