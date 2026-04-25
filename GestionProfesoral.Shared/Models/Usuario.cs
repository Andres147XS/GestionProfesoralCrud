using System.ComponentModel.DataAnnotations;

namespace GestionProfesoral.Shared.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public int RolId { get; set; }
        public Rol? Rol { get; set; }

        public bool Activo { get; set; } = true;
    }
}
