using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
        public bool Activo { get; set; }

    }

    public class UsuarioCreateDto
    {
        [Required]
        public int IdPerfil { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Apellido { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Telefono { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }

    public class UsuarioUpdateDto
    {
        [Required]
        public int IdPerfil { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Apellido { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Telefono { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
