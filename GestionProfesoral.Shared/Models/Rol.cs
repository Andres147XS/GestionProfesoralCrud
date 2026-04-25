using System.ComponentModel.DataAnnotations;

namespace GestionProfesoral.Shared.Models
{
    public class Rol
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descripcion { get; set; }

        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
