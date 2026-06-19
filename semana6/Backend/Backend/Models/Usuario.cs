using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_perfil")]
        public int IdPerfil { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Column("apellido")]
        public string Apellido { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; } = null!;

        [Column("activo")]
        public bool Activo { get; set; } = true;


        // Propiedad de navegación
        [ForeignKey("IdPerfil")]
        public Perfil Perfil { get; set; } = null!;
    }
}
