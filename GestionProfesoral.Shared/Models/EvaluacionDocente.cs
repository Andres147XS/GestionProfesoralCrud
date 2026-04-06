using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionProfesoral.Shared.Models
{
    public class EvaluacionDocente
    {
        [Key]
        public int Id { get; set; }

        public float Calificacion { get; set; }

        [Required]
        [MaxLength(45)]
        public string Semestre { get; set; } = string.Empty;

        [Required]
        public int DocenteCedula { get; set; }

        [ForeignKey("DocenteCedula")]
        [JsonIgnore]
        public virtual Docente? Docente { get; set; }
    }
}

