using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.DTOs;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>Login: valida credenciales y devuelve JWT</summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NombreUsuario) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { mensaje = "Usuario y contraseña son obligatorios" });

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == dto.NombreUsuario && u.Activo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

            var expiracion = DateTime.UtcNow.AddHours(
                double.Parse(_config["Jwt:ExpirationHours"] ?? "8"));

            return Ok(new LoginResponseDto
            {
                Token = GenerarToken(usuario, expiracion),
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol!.Nombre,
                Expiracion = expiracion
            });
        }

        /// <summary>Registro público: crea usuario con rol Docente y devuelve JWT</summary>
        [HttpPost("registro")]
        public async Task<ActionResult<LoginResponseDto>> Registro([FromBody] RegistroRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos de registro inválidos" });

            // Verificar nombre de usuario único
            if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == dto.NombreUsuario))
                return Conflict(new { mensaje = "El nombre de usuario ya está en uso" });

            // Verificar correo único
            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                return Conflict(new { mensaje = "El correo electrónico ya está registrado" });

            // Obtener rol Docente (por nombre, no por Id fijo)
            var rolDocente = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Docente");
            if (rolDocente == null)
            {
                // Si no existe el rol Docente, usar el primero disponible
                rolDocente = await _context.Roles.FirstOrDefaultAsync();
                if (rolDocente == null)
                    return StatusCode(500, new { mensaje = "No hay roles configurados en el sistema. Contacte al administrador." });
            }

            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario.Trim(),
                Correo = dto.Correo.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = rolDocente.Id,
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Recargar con relación para el token
            usuario.Rol = rolDocente;

            var expiracion = DateTime.UtcNow.AddHours(
                double.Parse(_config["Jwt:ExpirationHours"] ?? "8"));

            return Ok(new LoginResponseDto
            {
                Token = GenerarToken(usuario, expiracion),
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol.Nombre,
                Expiracion = expiracion
            });
        }

        private string GenerarToken(Usuario usuario, DateTime expiracion)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol!.Nombre),
                new Claim("RolId", usuario.RolId.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiracion,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
