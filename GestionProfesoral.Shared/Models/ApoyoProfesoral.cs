using System.ComponentModel.DataAnnotations;

namespace GestionProfesoral.Shared.Models
{
    // Entidad ApoyoProfesoral
    public class ApoyoProfesoral
    {
        [Key]
        public int Estudios { get; set; }

        [Required]
        public bool ConApoyo { get; set; }

        [Required]
        [MaxLength(45)]
        public string Institucion { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        public string Tipo { get; set; } = string.Empty;
    }
}
