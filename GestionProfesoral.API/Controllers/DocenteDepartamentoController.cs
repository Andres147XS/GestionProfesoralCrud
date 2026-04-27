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
    public class DocenteDepartamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocenteDepartamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocenteDepartamento>>> GetDocentesDepartamentos()
        {
            return await _context.DocentesDepartamentos.Include(dd => dd.Docente).ToListAsync();
        }

        [HttpGet("{docenteCedula}/{departamentoId}")]
        public async Task<ActionResult<DocenteDepartamento>> GetDocenteDepartamento(int docenteCedula, int departamentoId)
        {
            var dd = await _context.DocentesDepartamentos
                .Include(dd => dd.Docente)
                .FirstOrDefaultAsync(x => x.DocenteCedula == docenteCedula && x.DepartamentoId == departamentoId);

            if (dd == null) return NotFound();
            return dd;
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<DocenteDepartamento>> PostDocenteDepartamento(DocenteDepartamento dd)
        {
            _context.DocentesDepartamentos.Add(dd);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException)
            {
                if (DocenteDepartamentoExists(dd.DocenteCedula, dd.DepartamentoId)) return Conflict();
                else throw;
            }
            return CreatedAtAction(nameof(GetDocenteDepartamento), new { docenteCedula = dd.DocenteCedula, departamentoId = dd.DepartamentoId }, dd);
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{docenteCedula}/{departamentoId}")]
        public async Task<IActionResult> DeleteDocenteDepartamento(int docenteCedula, int departamentoId)
        {
            var dd = await _context.DocentesDepartamentos.FindAsync(docenteCedula, departamentoId);
            if (dd == null) return NotFound();
            _context.DocentesDepartamentos.Remove(dd);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DocenteDepartamentoExists(int docenteCedula, int departamentoId) =>
            _context.DocentesDepartamentos.Any(e => e.DocenteCedula == docenteCedula && e.DepartamentoId == departamentoId);
    }
}