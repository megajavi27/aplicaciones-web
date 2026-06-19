using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("perfiles")]
    public class Perfil
    {
        [Key]
        [Column("id_perfil")]
        public int IdPerfil { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;


        // Propiedad de navegación
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
