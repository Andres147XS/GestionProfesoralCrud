using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionProfesoral.Shared.Models
{
    public class Alianza
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public long AliadoNit { get; set; }

        [Required]
        public int DepartamentoId { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        public int DocenteCedula { get; set; }

        [ForeignKey("AliadoNit")]
        [JsonIgnore]
        public virtual Aliado? Aliado { get; set; }

        [ForeignKey("DocenteCedula")]
        [JsonIgnore]
        public virtual Docente? Docente { get; set; }
    }
}
