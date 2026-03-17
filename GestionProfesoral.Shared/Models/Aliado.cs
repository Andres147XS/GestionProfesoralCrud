using System.ComponentModel.DataAnnotations;

namespace GestionProfesoral.Shared.Models
{
    public class Aliado
    {
        [Key]
        public long Nit { get; set; }

        [Required]
        [MaxLength(60)]
        public string RazonSocial { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string NombreContacto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(60)]
        public string Correo { get; set; } = string.Empty;

        [MaxLength(45)]
        public string Telefono { get; set; } = string.Empty;

        [MaxLength(45)]
        public string Ciudad { get; set; } = string.Empty;
    }
}
