using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluacionParcial1.Models
{
    [Table("EstudianteClases")]
    public class EstudianteClase
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("estudiante_id")]
        public int EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        [Column("clase_id")]
        public int ClaseId { get; set; }
        public Clase? Clase { get; set; }
    }
}
