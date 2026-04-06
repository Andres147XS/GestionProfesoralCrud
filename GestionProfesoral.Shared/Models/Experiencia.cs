using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionProfesoral.Shared.Models
{
    public class Experiencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string NombreCargo { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        public string Institucion { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        public string Tipo { get; set; } = string.Empty;

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        public int DocenteCedula { get; set; }

        [ForeignKey("DocenteCedula")]
        [JsonIgnore]
        public virtual Docente? Docente { get; set; }
    }
}