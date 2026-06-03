using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClientes.Models.Ventas
{
    public class ProveedorModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cProveedor { get; set; }

        [Required]
        [StringLength(100)]
        public string RazonSocial { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        public string Ruc { get; set; } = string.Empty;

        [StringLength(17)]
        public string? Telefono { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Correo { get; set; }
    }
}
