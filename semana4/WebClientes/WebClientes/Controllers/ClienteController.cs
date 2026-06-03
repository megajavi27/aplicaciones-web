using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClientes.Data;
using WebClientes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WebClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cliente
        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Genero)
                .Include(c => c.EstadoCivil)
                .ToListAsync();
            return Ok(clientes);
        }

        // GET: api/cliente/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Genero)
                .Include(c => c.EstadoCivil)
                .FirstOrDefaultAsync(m => m.ccliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        // POST: api/cliente
        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ccliente }, cliente);
        }

        // PUT: api/cliente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteModel cliente)
        {
            if (id != cliente.ccliente)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la petición.");
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/cliente/catalogos
        [HttpGet("catalogos")]
        public async Task<IActionResult> GetCatalogos()
        {
            var tiposIdentificacion = await _context.TiposIdentificacion.ToListAsync();
            var generos = await _context.Generos.ToListAsync();
            var estadosCiviles = await _context.EstadosCiviles.ToListAsync();

            return Ok(new
            {
                tiposIdentificacion,
                generos,
                estadosCiviles
            });
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ccliente == id);
        }
    }
}
