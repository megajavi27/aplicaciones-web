using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ProyectoContext _context;

        public AuthController(ProyectoContext context)
        {
            _context = context;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var hashed = PasswordHelper.Hash(loginDto.Password);
            var usuario = await _context.Usuarios
                .Include(u => u.Perfil)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == hashed && u.Activo);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Credenciales invalidas usuario o contraseña." });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = (Environment.GetEnvironmentVariable("JWT_KEY") ?? "").PadRight(32, '.');
            var key = Encoding.UTF8.GetBytes(jwtKey);
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            var expiresMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Perfil.Nombre),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiresMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                usuario = new
                {
                    idUsuario = usuario.IdUsuario,
                    nombre = usuario.Nombre,
                    email = usuario.Email,
                    perfil = usuario.Perfil.Nombre
                }
            });
        }
    }
}
