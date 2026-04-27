using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionProfesoral.API.Controllers
{
    // CRUD de la entidad Red.
    [Authorize]
    [Route("api/[controller]")]

    [ApiController]
    public class RedController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RedController(AppDbContext context)
        {
            _context = context;
        }

        // GET (lista todos los registros)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Red>>> GetRedes()
        {
            return await _context.Redes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Red>> GetRed(int id)
        {
            var red = await _context.Redes.FindAsync(id);
            if (red == null) return NotFound();
            return red;
        }

        // POST (crea un registro)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<Red>> PostRed(Red red)
        {
            _context.Redes.Add(red);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRed), new { id = red.Id }, red);
        }

        // PUT (actualiza un registro)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRed(int id, Red red)
        {
            if (id != red.Id) return BadRequest();
            _context.Entry(red).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                // Si alguien borra el registro mientras se edita, se lanza una concurrencia
                if (!RedExists(id)) return NotFound();
                else throw;
            }
            // 204 No Content: actualización exitosa 
            return NoContent();
        }

        // DELETE (elimina un registro)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRed(int id)
        {
            var red = await _context.Redes.FindAsync(id);
            if (red == null) return NotFound();
            _context.Redes.Remove(red);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método auxiliar usado en el PUT: verifica existencia del registro
        private bool RedExists(int id) => _context.Redes.Any(e => e.Id == id);
    }
}
