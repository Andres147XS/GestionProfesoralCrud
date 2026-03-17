using System.ComponentModel.DataAnnotations;

namespace GestionProfesoral.Shared.Models
{
    // Entidad Red 
    public class Red
    {
        // Llave primaria (PK) (autoincremental).
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(45)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(45)]
        public string Pais { get; set; } = string.Empty;
    }
}
