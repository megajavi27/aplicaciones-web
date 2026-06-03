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
    public class DetallePedidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DetallePedidoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetalles()
        {
            var detalles = await _context.DetallesPedido
                .Include(d => d.Producto)
                .ToListAsync();
            return Ok(detalles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetalle(int id)
        {
            var detalle = await _context.DetallesPedido
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.cDetalle == id);

            if (detalle == null) return NotFound();
            return Ok(detalle);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDetalle([FromBody] DetallePedidoModel detalle)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            // Calculamos el precio unitario basado en el producto si no viene
            if (detalle.PrecioUnitario == 0)
            {
                var producto = await _context.Productos.FindAsync(detalle.cProducto);
                if (producto != null) detalle.PrecioUnitario = producto.Precio;
            }

            _context.DetallesPedido.Add(detalle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDetalle), new { id = detalle.cDetalle }, detalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetalle(int id, [FromBody] DetallePedidoModel detalle)
        {
            if (id != detalle.cDetalle) return BadRequest();
            _context.Entry(detalle).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DetallesPedido.Any(e => e.cDetalle == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id);
            if (detalle == null) return NotFound();
            _context.DetallesPedido.Remove(detalle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
