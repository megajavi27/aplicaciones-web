namespace Backend.DTOs
{
    public class PerfilResponseDto
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
