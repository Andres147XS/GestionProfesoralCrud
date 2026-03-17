using System.ComponentModel.DataAnnotations;

namespace GestionProfesoral.Shared.Models
{
    // Entidad Beca 
    public class Beca
    {
        [Key]
        public int Estudios { get; set; }

        [Required]
        [MaxLength(45)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        public string Institucion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        [Required]
        public DateTime FechaFin { get; set; } = DateTime.Now;
    }
}
