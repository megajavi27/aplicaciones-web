using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluacionParcial1.Models
{
    [Table("Profesores")]
    public class Profesor
    {
        [Key]
        [Column("profesor_id")]
        public int ProfesorId { get; set; }

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

        [Column("especialidad")]
        [Required(ErrorMessage = "La especialidad es requerida")]
        [MaxLength(150)]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; } = string.Empty;

        [Column("email")]
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        public ICollection<Clase> Clases { get; set; } = new List<Clase>();
    }
}
