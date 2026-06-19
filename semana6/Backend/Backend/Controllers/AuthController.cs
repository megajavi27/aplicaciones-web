using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        private readonly IWebHostEnvironment _env;

        public AuthController(ProyectoContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hashed = PasswordHelper.Hash(loginDto.Password);
            var usuario = await _context.Usuarios
                .Include(u => u.Perfil)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == hashed && u.Activo);

            if (usuario == null)
                return Unauthorized(new { message = "Credenciales inválidas: usuario o contraseña incorrectos." });

            var jwtKey = (Environment.GetEnvironmentVariable("JWT_KEY") ?? "").PadRight(32, '.');
            var key = Encoding.UTF8.GetBytes(jwtKey);
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            var expiresMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                    new Claim("nombre", usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Perfil.Nombre),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiresMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenString = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            // Almacenar JWT en cookie HttpOnly (no accesible desde JavaScript)
            // SameSite=None + Secure=true necesario para que el browser envíe la cookie
            // en peticiones cross-scheme (Angular HTTP → backend HTTPS en localhost)
            Response.Cookies.Append("auth_token", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes)
            });

            // Cookie de XSRF legible por Angular (no HttpOnly)
            var antiforgery = HttpContext.RequestServices.GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>();
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            // Solo devuelve el nombre para guardarlo en localStorage (la sesión vive en la cookie)
            return Ok(new
            {
                nombre = usuario.Nombre,
                perfil = usuario.Perfil.Nombre
            });
        }

        // POST api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            Response.Cookies.Delete("XSRF-TOKEN");
            return Ok(new { message = "Sesión cerrada correctamente." });
        }

        // GET api/auth/me  — valida sesión desde el backend
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var nombre = User.FindFirstValue("nombre");
            var email = User.FindFirstValue(ClaimTypes.Email);
            var perfil = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new { nombre, email, perfil });
        }
    }
}
