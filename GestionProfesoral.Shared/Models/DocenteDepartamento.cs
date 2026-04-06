using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProfesoral.Shared.Models
{
    public class DocenteDepartamento
    {
        [Required]
        public int DocenteCedula { get; set; }

        [Required]
        public int DepartamentoId { get; set; }

        [MaxLength(15)]
        public string? Dedicacion { get; set; }

        [MaxLength(45)]
        public string? Modalidad { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaSalida { get; set; }

        [ForeignKey("DocenteCedula")]
        public virtual Docente? Docente { get; set; }
    }
}
