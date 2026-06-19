using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public UsuarioController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Perfil)
                .Where(u => u.Activo)
                .Select(u => new UsuarioResponseDto
                {
                    IdUsuario = u.IdUsuario,
                    IdPerfil = u.IdPerfil,
                    NombrePerfil = u.Perfil.Nombre,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    Activo = u.Activo
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/usuario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Perfil)
                .Where(u => u.Activo && u.IdUsuario == id)
                .Select(u => new UsuarioResponseDto
                {
                    IdUsuario = u.IdUsuario,
                    IdPerfil = u.IdPerfil,
                    NombrePerfil = u.Perfil.Nombre,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    Activo = u.Activo
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> PostUsuario(UsuarioCreateDto dto)
        {
            // Validar email único
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict(new { message = "El email ya está en uso." });
            }

            var usuario = new Usuario
            {
                IdPerfil = dto.IdPerfil,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                PasswordHash = PasswordHelper.Hash(dto.Password),
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // PUT: api/usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioUpdateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null || !usuario.Activo)
            {
                return NotFound();
            }

            // Validar email único excluyendo el usuario actual
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.IdUsuario != id))
            {
                return Conflict(new { message = "El email ya está en uso por otro usuario." });
            }

            usuario.IdPerfil = dto.IdPerfil;
            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Email = dto.Email;
            usuario.Telefono = dto.Telefono;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                usuario.PasswordHash = PasswordHelper.Hash(dto.Password);
            }

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/usuario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null || !usuario.Activo)
            {
                return NotFound();
            }

            // Soft delete
            usuario.Activo = false;

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
