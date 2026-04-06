using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExperienciaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Experiencia>>> GetExperiencias()
        {
            return await _context.Experiencias.Include(e => e.Docente).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Experiencia>> GetExperiencia(int id)
        {
            var experiencia = await _context.Experiencias.Include(e => e.Docente).FirstOrDefaultAsync(e => e.Id == id);
            if (experiencia == null) return NotFound();
            return experiencia;
        }

        [HttpPost]
        public async Task<ActionResult<Experiencia>> PostExperiencia(Experiencia experiencia)
        {
            _context.Experiencias.Add(experiencia);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExperiencia), new { id = experiencia.Id }, experiencia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExperiencia(int id, Experiencia experiencia)
        {
            if (id != experiencia.Id) return BadRequest();
            _context.Entry(experiencia).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperienciaExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExperiencia(int id)
        {
            var experiencia = await _context.Experiencias.FindAsync(id);
            if (experiencia == null) return NotFound();
            _context.Experiencias.Remove(experiencia);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ExperienciaExists(int id) => _context.Experiencias.Any(e => e.Id == id);
    }
}
