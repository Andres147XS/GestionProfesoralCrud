using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionProfesoral.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosRealizadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstudiosRealizadosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudiosRealizados>>> GetEstudios()
        {
            return await _context.EstudiosRealizados.Include(e => e.Docente).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstudiosRealizados>> GetEstudio(int id)
        {
            var estudio = await _context.EstudiosRealizados.Include(e => e.Docente).FirstOrDefaultAsync(e => e.Id == id);
            if (estudio == null) return NotFound();
            return estudio;
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<EstudiosRealizados>> PostEstudio(EstudiosRealizados estudio)
        {
            _context.EstudiosRealizados.Add(estudio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEstudio), new { id = estudio.Id }, estudio);
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudio(int id, EstudiosRealizados estudio)
        {
            if (id != estudio.Id) return BadRequest();
            _context.Entry(estudio).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudioExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudio(int id)
        {
            var estudio = await _context.EstudiosRealizados.FindAsync(id);
            if (estudio == null) return NotFound();
            _context.EstudiosRealizados.Remove(estudio);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EstudioExists(int id) => _context.EstudiosRealizados.Any(e => e.Id == id);
    }
}
