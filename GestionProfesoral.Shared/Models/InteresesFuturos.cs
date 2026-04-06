using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionProfesoral.Shared.Models
{
    public class InteresesFuturos
    {
        [Required]
        public int DocenteCedula { get; set; }

        [Required]
        [MaxLength(30)]
        public string TerminoClave { get; set; } = string.Empty;

        [ForeignKey("DocenteCedula")]
        [JsonIgnore] // Evita que Swagger pida el objeto Docente completo
        public virtual Docente? Docente { get; set; }
    }
}
