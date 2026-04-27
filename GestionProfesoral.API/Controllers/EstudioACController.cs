using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;
<<<<<<< HEAD

namespace GestionProfesoral.API.Controllers
{
=======
using Microsoft.AspNetCore.Authorization;

namespace GestionProfesoral.API.Controllers
{
    [Authorize]
>>>>>>> Eloisa
    [Route("api/[controller]")]
    [ApiController]
    public class EstudioACController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstudioACController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudioAC>>> GetEstudiosAC()
        {
            return await _context.EstudiosAC.Include(e => e.Estudio).ToListAsync();
        }

        [HttpGet("{estudioId}/{areaConocimientoId}")]
        public async Task<ActionResult<EstudioAC>> GetEstudioAC(int estudioId, int areaConocimientoId)
        {
            var eac = await _context.EstudiosAC
                .Include(e => e.Estudio)
                .FirstOrDefaultAsync(x => x.EstudioId == estudioId && x.AreaConocimientoId == areaConocimientoId);

            if (eac == null) return NotFound();
            return eac;
        }

<<<<<<< HEAD
=======
        [Authorize(Roles = "Administrador,Docente")]
>>>>>>> Eloisa
        [HttpPost]
        public async Task<ActionResult<EstudioAC>> PostEstudioAC(EstudioAC eac)
        {
            _context.EstudiosAC.Add(eac);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException)
            {
                if (EstudioACExists(eac.EstudioId, eac.AreaConocimientoId)) return Conflict();
                else throw;
            }
            return CreatedAtAction(nameof(GetEstudioAC), new { estudioId = eac.EstudioId, areaConocimientoId = eac.AreaConocimientoId }, eac);
        }

<<<<<<< HEAD
=======
        [Authorize(Roles = "Administrador,Docente")]
>>>>>>> Eloisa
        [HttpDelete("{estudioId}/{areaConocimientoId}")]
        public async Task<IActionResult> DeleteEstudioAC(int estudioId, int areaConocimientoId)
        {
            var eac = await _context.EstudiosAC.FindAsync(estudioId, areaConocimientoId);
            if (eac == null) return NotFound();
            _context.EstudiosAC.Remove(eac);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EstudioACExists(int estudioId, int areaConocimientoId) =>
            _context.EstudiosAC.Any(e => e.EstudioId == estudioId && e.AreaConocimientoId == areaConocimientoId);
    }
}