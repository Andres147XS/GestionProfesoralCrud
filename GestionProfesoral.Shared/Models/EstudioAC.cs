using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProfesoral.Shared.Models
{
    public class EstudioAC
    {
        [Required]
        public int EstudioId { get; set; }

        [Required]
        public int AreaConocimientoId { get; set; }

        [ForeignKey("EstudioId")]
        public virtual EstudiosRealizados? Estudio { get; set; }
    }
}