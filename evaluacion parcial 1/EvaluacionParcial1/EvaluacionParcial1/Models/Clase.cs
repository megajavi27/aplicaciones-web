using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluacionParcial1.Models
{
    [Table("Clases")]
    public class Clase
    {
        [Key]
        [Column("clase_id")]
        public int ClaseId { get; set; }

        [Column("nombre_clase")]
        [Required(ErrorMessage = "El nombre de la clase es requerido")]
        [MaxLength(150)]
        [Display(Name = "Nombre de la Clase")]
        public string NombreClase { get; set; } = string.Empty;

        [Column("profesor_id")]
        [Required(ErrorMessage = "Debe asignar un profesor")]
        [Display(Name = "Profesor")]
        public int ProfesorId { get; set; }

        public Profesor? Profesor { get; set; }

        public ICollection<EstudianteClase> EstudianteClases { get; set; } = new List<EstudianteClase>();
    }
}
