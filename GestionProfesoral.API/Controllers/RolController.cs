using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    /// <summary>CRUD de Roles — solo accesible por Administrador</summary>
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RolController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
            => await _context.Roles.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            return rol == null ? NotFound() : rol;
        }

        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol([FromBody] Rol rol)
        {
            if (await _context.Roles.AnyAsync(r => r.Nombre == rol.Nombre))
                return Conflict(new { mensaje = "Ya existe un rol con ese nombre" });
            rol.Id = 0;
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, rol);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, [FromBody] Rol rol)
        {
            if (id != rol.Id) return BadRequest();
            var rolExiste = await _context.Roles.FindAsync(id);
            if (rolExiste == null) return NotFound();
            rolExiste.Nombre = rol.Nombre;
            rolExiste.Descripcion = rol.Descripcion;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return NotFound();
            if (await _context.Usuarios.AnyAsync(u => u.RolId == id))
                return BadRequest(new { mensaje = "No se puede eliminar un rol que tiene usuarios asignados" });
            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
