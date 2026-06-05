using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluacionParcial1.Models
{
    [Table("Estudiantes")]
    public class Estudiante
    {
        [Key]
        [Column("estudiante_id")]
        public int EstudianteId { get; set; }

        [Column("nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("apellido")]
        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(100)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Column("fecha_nacimiento")]
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Column("grado")]
        [Required(ErrorMessage = "El grado es requerido")]
        [MaxLength(50)]
        [Display(Name = "Grado")]
        public string Grado { get; set; } = string.Empty;

        public ICollection<EstudianteClase> EstudianteClases { get; set; } = new List<EstudianteClase>();
    }
}
