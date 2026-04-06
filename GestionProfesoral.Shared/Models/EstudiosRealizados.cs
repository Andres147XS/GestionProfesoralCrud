using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionProfesoral.Shared.Models
{
    public class EstudiosRealizados
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Universidad { get; set; } = string.Empty;

        public DateTime? Fecha { get; set; }

        [Required]
        [MaxLength(45)]
        public string Tipo { get; set; } = string.Empty;

        [MaxLength(45)]
        public string? Ciudad { get; set; }

        [Required]
        public int DocenteCedula { get; set; }

        public bool InsAcreditada { get; set; }

        [MaxLength(45)]
        public string? Metodologia { get; set; }

        public string? PerfilEgresado { get; set; }

        [MaxLength(45)]
        public string? Pais { get; set; }

        [ForeignKey("DocenteCedula")]
        [JsonIgnore]
        public virtual Docente? Docente { get; set; }
    }
}
