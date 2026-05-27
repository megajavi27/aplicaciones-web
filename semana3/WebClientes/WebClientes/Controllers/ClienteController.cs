using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebClientes.Data;
using WebClientes.Models;

namespace WebClientes.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Genero)
                .Include(c => c.EstadoCivil)
                .ToListAsync();
            return View(clientes);
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Genero)
                .Include(c => c.EstadoCivil)
                .FirstOrDefaultAsync(m => m.ccliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.TiposIdentificacion = new SelectList(await _context.TiposIdentificacion.ToListAsync(), "ctipoidentificacion", "Descripcion");
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "cgenero", "Descripcion");
            ViewBag.EstadosCiviles = new SelectList(await _context.EstadosCiviles.ToListAsync(), "cestadocivil", "Descripcion");
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ccliente,Nombre,Apellidos,Identificacion,ctipoidentificacion,cgenero,FechaNacimiento,cestadocivil,Direccion,Telefono,Correo")] ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TiposIdentificacion = new SelectList(await _context.TiposIdentificacion.ToListAsync(), "ctipoidentificacion", "Descripcion", cliente.ctipoidentificacion);
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "cgenero", "Descripcion", cliente.cgenero);
            ViewBag.EstadosCiviles = new SelectList(await _context.EstadosCiviles.ToListAsync(), "cestadocivil", "Descripcion", cliente.cestadocivil);
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.TiposIdentificacion = new SelectList(await _context.TiposIdentificacion.ToListAsync(), "ctipoidentificacion", "Descripcion", cliente.ctipoidentificacion);
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "cgenero", "Descripcion", cliente.cgenero);
            ViewBag.EstadosCiviles = new SelectList(await _context.EstadosCiviles.ToListAsync(), "cestadocivil", "Descripcion", cliente.cestadocivil);
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ccliente,Nombre,Apellidos,Identificacion,ctipoidentificacion,cgenero,FechaNacimiento,cestadocivil,Direccion,Telefono,Correo")] ClienteModel cliente)
        {
            if (id != cliente.ccliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ccliente))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TiposIdentificacion = new SelectList(await _context.TiposIdentificacion.ToListAsync(), "ctipoidentificacion", "Descripcion", cliente.ctipoidentificacion);
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "cgenero", "Descripcion", cliente.cgenero);
            ViewBag.EstadosCiviles = new SelectList(await _context.EstadosCiviles.ToListAsync(), "cestadocivil", "Descripcion", cliente.cestadocivil);
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Genero)
                .Include(c => c.EstadoCivil)
                .FirstOrDefaultAsync(m => m.ccliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ccliente == id);
        }
    }
}
