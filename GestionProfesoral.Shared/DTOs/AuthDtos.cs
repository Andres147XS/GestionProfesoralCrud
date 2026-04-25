using System.ComponentModel.DataAnnotations;

namespace GestionProfesoral.Shared.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
    }

    public class RegistroRequestDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [MaxLength(60, ErrorMessage = "Máximo 60 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }

    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [MaxLength(60)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol válido")]
        public int RolId { get; set; }
    }

    public class UsuarioUpdateDto
    {
        [MaxLength(60)]
        public string? NombreUsuario { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Correo { get; set; }

        public int? RolId { get; set; }

        public bool? Activo { get; set; }
    }

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}