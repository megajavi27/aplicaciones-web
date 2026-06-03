using System.ComponentModel.DataAnnotations;

namespace WebClientes.Models
{
    public class GeneroModel
    {
        [Key]
        public char cgenero { get; set; }  // 'h' = Hombre, 'm' = Mujer

        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(20, ErrorMessage = "El máximo número de caracteres es 20")]
        [Display(Name = "Género")]
        public string Descripcion { get; set; }
    }
}
