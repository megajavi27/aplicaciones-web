using System.ComponentModel.DataAnnotations;

namespace WebClientes.Models
{
    public class EstadoCivilModel
    {
        [Key]
        public char cestadocivil { get; set; }  // 'c' = Casado, 's' = Soltero, 'v' = Viudo, 'd' = Divorciado

        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(20, ErrorMessage = "El máximo número de caracteres es 20")]
        [Display(Name = "Estado Civil")]
        public string Descripcion { get; set; }
    }
}
