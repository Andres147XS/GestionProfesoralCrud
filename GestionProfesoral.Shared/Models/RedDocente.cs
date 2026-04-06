using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProfesoral.Shared.Models
{
    public class RedDocente
    {
        [Required]
        public int RedId { get; set; }

        [Required]
        public int DocenteCedula { get; set; }

        public DateTime? FechaInicio { get; set; }

        [MaxLength(45)]
        public string? FechaFin { get; set; }

        public string? ActDestacadas { get; set; }

        // Navigation properties
        [ForeignKey("RedId")]
        public virtual Red? Red { get; set; }

        [ForeignKey("DocenteCedula")]
        public virtual Docente? Docente { get; set; }
    }
}
