using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClientes.Models.Ventas
{
    public class PedidoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cPedido { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        public decimal Total { get; set; } = 0;

        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Procesado, Enviado, Cancelado

        [Required]
        public int ccliente { get; set; }

        [ForeignKey("ccliente")]
        public ClienteModel? Cliente { get; set; }

        public ICollection<DetallePedidoModel>? DetallesPedido { get; set; }
    }
}
