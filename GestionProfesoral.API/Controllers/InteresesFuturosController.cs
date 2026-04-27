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
    public class InteresesFuturosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InteresesFuturosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InteresesFuturos>>> GetInteresesFuturos()
        {
            return await _context.InteresesFuturos.Include(i => i.Docente).ToListAsync();
        }

        [HttpGet("{docenteCedula}/{terminoClave}")]
        public async Task<ActionResult<InteresesFuturos>> GetInteresFuturo(int docenteCedula, string terminoClave)
        {
            var i = await _context.InteresesFuturos
                .Include(i => i.Docente)
                .FirstOrDefaultAsync(x => x.DocenteCedula == docenteCedula && x.TerminoClave == terminoClave);

            if (i == null) return NotFound();
            return i;
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<InteresesFuturos>> PostInteresFuturo(InteresesFuturos i)
        {
            _context.InteresesFuturos.Add(i);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException)
            {
                if (InteresFuturoExists(i.DocenteCedula, i.TerminoClave)) return Conflict();
                else throw;
            }
            return CreatedAtAction(nameof(GetInteresFuturo), new { docenteCedula = i.DocenteCedula, terminoClave = i.TerminoClave }, i);
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{docenteCedula}/{terminoClave}")]
        public async Task<IActionResult> DeleteInteresFuturo(int docenteCedula, string terminoClave)
        {
            var i = await _context.InteresesFuturos.FindAsync(docenteCedula, terminoClave);
            if (i == null) return NotFound();
            _context.InteresesFuturos.Remove(i);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool InteresFuturoExists(int docenteCedula, string terminoClave) =>
            _context.InteresesFuturos.Any(e => e.DocenteCedula == docenteCedula && e.TerminoClave == terminoClave);
    }
}
