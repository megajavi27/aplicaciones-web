using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public PerfilController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: api/perfil
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerfilResponseDto>>> GetPerfiles()
        {
            var perfiles = await _context.Perfiles
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .Select(p => new PerfilResponseDto
                {
                    IdPerfil = p.IdPerfil,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Activo = p.Activo
                })
                .ToListAsync();

            return Ok(perfiles);
        }

        // GET: api/perfil/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PerfilResponseDto>> GetPerfil(int id)
        {
            var perfil = await _context.Perfiles
                .Where(p => p.Activo && p.IdPerfil == id)
                .Select(p => new PerfilResponseDto
                {
                    IdPerfil = p.IdPerfil,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Activo = p.Activo
                })
                .FirstOrDefaultAsync();

            if (perfil == null)
            {
                return NotFound(new { mensaje = "Perfil no encontrado" });
            }

            return Ok(perfil);
        }
    }
}
