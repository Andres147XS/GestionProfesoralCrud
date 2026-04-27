using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.DTOs;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    /// <summary>CRUD de Usuarios — solo accesible por Administrador</summary>
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .OrderBy(u => u.NombreUsuario)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Correo = u.Correo,
                    RolId = u.RolId,
                    RolNombre = u.Rol!.Nombre,
                    Activo = u.Activo
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var u = await _context.Usuarios.Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (u == null) return NotFound();
            return new UsuarioDto
            {
                Id = u.Id, NombreUsuario = u.NombreUsuario, Correo = u.Correo,
                RolId = u.RolId, RolNombre = u.Rol!.Nombre, Activo = u.Activo
            };
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] UsuarioCreateDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == dto.NombreUsuario))
                return Conflict(new { mensaje = "El nombre de usuario ya existe" });

            if (!await _context.Roles.AnyAsync(r => r.Id == dto.RolId))
                return BadRequest(new { mensaje = "El rol especificado no existe" });

            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Correo = dto.Correo.Trim(),
                RolId = dto.RolId,
                Activo = true
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, new UsuarioDto
            {
                Id = usuario.Id, NombreUsuario = usuario.NombreUsuario, Correo = usuario.Correo,
                RolId = usuario.RolId, RolNombre = usuario.Rol!.Nombre, Activo = usuario.Activo
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioUpdateDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return NotFound();

            // Actualizar solo los campos que vienen en el DTO
            if (dto.NombreUsuario != null)
            {
                var trimmed = dto.NombreUsuario.Trim();
                if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == trimmed && u.Id != id))
                    return Conflict(new { mensaje = "El nombre de usuario ya existe" });
                usuario.NombreUsuario = trimmed;
            }

            if (dto.Password != null)
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            if (dto.Correo != null)
                usuario.Correo = dto.Correo.Trim();

            if (dto.RolId.HasValue)
            {
                if (!await _context.Roles.AnyAsync(r => r.Id == dto.RolId))
                    return BadRequest(new { mensaje = "El rol especificado no existe" });
                usuario.RolId = dto.RolId.Value;
            }

            if (dto.Activo.HasValue)
            {
                // Proteger: no desactivar al único administrador activo
                if (!dto.Activo.Value && usuario.Rol!.Nombre == "Administrador")
                {
                    var countAdmins = await _context.Usuarios
                        .Where(u => u.Rol!.Nombre == "Administrador" && u.Activo)
                        .CountAsync();
                    if (countAdmins <= 1)
                        return BadRequest(new { mensaje = "No se puede desactivar el único administrador activo" });
                }
                usuario.Activo = dto.Activo.Value;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return NotFound();

            if (usuario.Rol?.Nombre == "Administrador")
            {
                var countAdmins = await _context.Usuarios
                    .Where(u => u.Rol!.Nombre == "Administrador" && u.Activo)
                    .CountAsync();
                if (countAdmins <= 1)
                    return BadRequest(new { mensaje = "No se puede eliminar el único administrador activo" });
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}