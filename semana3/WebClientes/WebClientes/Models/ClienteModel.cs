using System.ComponentModel.DataAnnotations;

namespace WebClientes.Models
{
    public class ClienteModel
    {
        [Key]
        public int ccliente { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El máximo número de caracteres es 100")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Nombre { get; set; }

        [MaxLength(100, ErrorMessage = "El máximo número de caracteres es 100")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(10, ErrorMessage = "El máximo número de caracteres es 10")]
        [Display(Name = "N° Identificación")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Tipo de Identificación")]
        public char ctipoidentificacion { get; set; }
        public TipoIdentificacionModel TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Género")]
        public char cgenero { get; set; }
        public GeneroModel Genero { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Estado Civil")]
        public char cestadocivil { get; set; }
        public EstadoCivilModel EstadoCivil { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(17, ErrorMessage = "El máximo número de caracteres es 17")]
        [MinLength(9, ErrorMessage = "El mínimo de caracteres es 9")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Correo { get; set; }
    }
}
