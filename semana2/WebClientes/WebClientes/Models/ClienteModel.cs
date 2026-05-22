using System.ComponentModel.DataAnnotations;

namespace WebClientes.Models
{
    public class ClienteModel
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "Nombres del Cliente")]
        [MaxLength(100, ErrorMessage = "El maximo numero de caracteres es 100")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Nombres_Cliente { get; set; }

        [MaxLength(100, ErrorMessage = "El maximo numero de caracteres es 100")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(17, ErrorMessage = "El máximo número de caracteres es 17")]
        [MinLength(9, ErrorMessage = "El minimo de caracteres es 9")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Correo { get; set; }
    }
}
