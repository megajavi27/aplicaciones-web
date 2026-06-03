using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebClientes.Models.Ventas
{
    public class ProductoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cProducto { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; } = 0;

        [Required]
        public int cCategoria { get; set; }

        // Opcional 6to campo: para vincular con Proveedor sin crear tabla extra
        public int? cProveedor { get; set; }

        [ForeignKey("cCategoria")]
        public CategoriaModel? Categoria { get; set; }

        [ForeignKey("cProveedor")]
        public ProveedorModel? Proveedor { get; set; }

        [JsonIgnore]
        public ICollection<DetallePedidoModel>? DetallesPedido { get; set; }
    }
}
