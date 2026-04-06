using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedDocenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RedDocenteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RedDocente>>> GetRedesDocentes()
        {
            return await _context.RedesDocentes.Include(rd => rd.Red).Include(rd => rd.Docente).ToListAsync();
        }

        [HttpGet("{redId}/{docenteCedula}")]
        public async Task<ActionResult<RedDocente>> GetRedDocente(int redId, int docenteCedula)
        {
            var redDocente = await _context.RedesDocentes
                .Include(rd => rd.Red)
                .Include(rd => rd.Docente)
                .FirstOrDefaultAsync(rd => rd.RedId == redId && rd.DocenteCedula == docenteCedula);

            if (redDocente == null) return NotFound();
            return redDocente;
        }

        [HttpPost]
        public async Task<ActionResult<RedDocente>> PostRedDocente(RedDocente redDocente)
        {
            _context.RedesDocentes.Add(redDocente);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException)
            {
                if (RedDocenteExists(redDocente.RedId, redDocente.DocenteCedula)) return Conflict();
                else throw;
            }
            return CreatedAtAction(nameof(GetRedDocente), new { redId = redDocente.RedId, docenteCedula = redDocente.DocenteCedula }, redDocente);
        }

        [HttpDelete("{redId}/{docenteCedula}")]
        public async Task<IActionResult> DeleteRedDocente(int redId, int docenteCedula)
        {
            var redDocente = await _context.RedesDocentes.FindAsync(redId, docenteCedula);
            if (redDocente == null) return NotFound();
            _context.RedesDocentes.Remove(redDocente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RedDocenteExists(int redId, int docenteCedula) =>
            _context.RedesDocentes.Any(e => e.RedId == redId && e.DocenteCedula == docenteCedula);
    }
}