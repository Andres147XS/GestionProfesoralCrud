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
    public class ReconocimientoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReconocimientoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reconocimiento>>> GetReconocimientos()
        {
            return await _context.Reconocimientos.Include(r => r.Docente).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reconocimiento>> GetReconocimiento(int id)
        {
            var reconocimiento = await _context.Reconocimientos.Include(r => r.Docente).FirstOrDefaultAsync(r => r.Id == id);
            if (reconocimiento == null) return NotFound();
            return reconocimiento;
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<Reconocimiento>> PostReconocimiento(Reconocimiento reconocimiento)
        {
            _context.Reconocimientos.Add(reconocimiento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReconocimiento), new { id = reconocimiento.Id }, reconocimiento);
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReconocimiento(int id, Reconocimiento reconocimiento)
        {
            if (id != reconocimiento.Id) return BadRequest();
            _context.Entry(reconocimiento).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReconocimientoExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReconocimiento(int id)
        {
            var reconocimiento = await _context.Reconocimientos.FindAsync(id);
            if (reconocimiento == null) return NotFound();
            _context.Reconocimientos.Remove(reconocimiento);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ReconocimientoExists(int id) => _context.Reconocimientos.Any(e => e.Id == id);
    }
}
