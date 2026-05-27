using System.ComponentModel.DataAnnotations;

namespace WebClientes.Models
{
    public class TipoIdentificacionModel
    {
        [Key]
        public char ctipoidentificacion { get; set; }  // 'c' = Cédula, 'p' = Pasaporte

        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(50, ErrorMessage = "El máximo número de caracteres es 50")]
        [Display(Name = "Tipo de Identificación")]
        public string Descripcion { get; set; }
    }
}
