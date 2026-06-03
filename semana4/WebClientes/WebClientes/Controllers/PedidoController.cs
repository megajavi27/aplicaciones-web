using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClientes.Data;
using WebClientes.Models.Ventas;
using System.Linq;
using System.Threading.Tasks;

namespace WebClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .ToListAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.cPedido == id);

            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpGet("{id}/detalles")]
        public async Task<IActionResult> GetPedidoDetalles(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido!)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.cPedido == id);

            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePedido([FromBody] PedidoModel pedido)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.cPedido }, pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, [FromBody] PedidoModel pedido)
        {
            if (id != pedido.cPedido) return BadRequest();
            _context.Entry(pedido).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pedidos.Any(e => e.cPedido == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
