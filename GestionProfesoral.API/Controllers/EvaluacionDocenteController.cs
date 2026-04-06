using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionDocenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EvaluacionDocenteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionDocente>>> GetEvaluaciones()
        {
            return await _context.EvaluacionesDocentes.Include(e => e.Docente).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionDocente>> GetEvaluacion(int id)
        {
            var evaluacion = await _context.EvaluacionesDocentes.Include(e => e.Docente).FirstOrDefaultAsync(e => e.Id == id);
            if (evaluacion == null) return NotFound();
            return evaluacion;
        }

        [HttpPost]
        public async Task<ActionResult<EvaluacionDocente>> PostEvaluacion(EvaluacionDocente evaluacion)
        {
            _context.EvaluacionesDocentes.Add(evaluacion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvaluacion), new { id = evaluacion.Id }, evaluacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacion(int id, EvaluacionDocente evaluacion)
        {
            if (id != evaluacion.Id) return BadRequest();
            _context.Entry(evaluacion).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacion(int id)
        {
            var evaluacion = await _context.EvaluacionesDocentes.FindAsync(id);
            if (evaluacion == null) return NotFound();
            _context.EvaluacionesDocentes.Remove(evaluacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EvaluacionExists(int id) => _context.EvaluacionesDocentes.Any(e => e.Id == id);
    }
}
