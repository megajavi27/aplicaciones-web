using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvaluacionParcial1.Models;

namespace EvaluacionParcial1.Controllers
{
    public class ClaseController : Controller
    {
        private readonly EscuelaDBContext _context;

        public ClaseController(EscuelaDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clases = _context.Clases.Include(c => c.Profesor);
            return View(await clases.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var clase = await _context.Clases
                .Include(c => c.Profesor)
                .FirstOrDefaultAsync(m => m.ClaseId == id);
            
            if (clase == null) return NotFound();

            return View(clase);
        }

        public IActionResult Create()
        {
            var profesores = _context.Profesores.Select(p => new {
                p.ProfesorId,
                NombreCompleto = p.Nombre + " " + p.Apellido
            }).ToList();
            ViewBag.ProfesorId = new SelectList(profesores, "ProfesorId", "NombreCompleto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaseId,NombreClase,ProfesorId")] Clase clase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var profesores = _context.Profesores.Select(p => new {
                p.ProfesorId,
                NombreCompleto = p.Nombre + " " + p.Apellido
            }).ToList();
            ViewBag.ProfesorId = new SelectList(profesores, "ProfesorId", "NombreCompleto", clase.ProfesorId);
            return View(clase);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var clase = await _context.Clases.FindAsync(id);
            if (clase == null) return NotFound();
            
            var profesores = _context.Profesores.Select(p => new {
                p.ProfesorId,
                NombreCompleto = p.Nombre + " " + p.Apellido
            }).ToList();
            ViewBag.ProfesorId = new SelectList(profesores, "ProfesorId", "NombreCompleto", clase.ProfesorId);
            return View(clase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaseId,NombreClase,ProfesorId")] Clase clase)
        {
            if (id != clase.ClaseId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaseExists(clase.ClaseId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var profesores = _context.Profesores.Select(p => new {
                p.ProfesorId,
                NombreCompleto = p.Nombre + " " + p.Apellido
            }).ToList();
            ViewBag.ProfesorId = new SelectList(profesores, "ProfesorId", "NombreCompleto", clase.ProfesorId);
            return View(clase);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var clase = await _context.Clases
                .Include(c => c.Profesor)
                .FirstOrDefaultAsync(m => m.ClaseId == id);
                
            if (clase == null) return NotFound();

            return View(clase);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clase = await _context.Clases.FindAsync(id);
            if (clase != null)
            {
                _context.Clases.Remove(clase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaseExists(int id)
        {
            return _context.Clases.Any(e => e.ClaseId == id);
        }
    }
}
