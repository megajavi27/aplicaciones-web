using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebClientes.Models.Ventas
{
    public class DetallePedidoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cDetalle { get; set; }

        [Required]
        public int cPedido { get; set; }

        [Required]
        public int cProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey("cPedido")]
        [JsonIgnore]
        public PedidoModel? Pedido { get; set; }

        [ForeignKey("cProducto")]
        public ProductoModel? Producto { get; set; }
    }
}
