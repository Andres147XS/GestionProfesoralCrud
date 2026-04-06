using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProfesoral.Shared.Models
{
    public class Docente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Cedula { get; set; }

        [Required]
        [MaxLength(60)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string Apellidos { get; set; } = string.Empty;

        [MaxLength(12)]
        public string? Genero { get; set; }

        [MaxLength(30)]
        public string? Cargo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [Required]
        [MaxLength(70)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [MaxLength(128)]
        public string? UrlCvlac { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        [MaxLength(45)]
        public string? Escalafon { get; set; }

        public string? Perfil { get; set; }

        [MaxLength(45)]
        public string? CatMinciencia { get; set; }

        [MaxLength(45)]
        public string? ConvMinciencia { get; set; }

        [MaxLength(45)]
        public string? Nacionalidad { get; set; }

        public int? LineaInvestigacionPrincipal { get; set; }
    }
}