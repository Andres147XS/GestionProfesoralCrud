using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlianzaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlianzaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alianza>>> GetAlianzas()
        {
            return await _context.Alianzas.Include(a => a.Aliado).Include(a => a.Docente).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alianza>> GetAlianza(int id)
        {
            var alianza = await _context.Alianzas.Include(a => a.Aliado).Include(a => a.Docente).FirstOrDefaultAsync(a => a.Id == id);
            if (alianza == null) return NotFound();
            return alianza;
        }

        [HttpPost]
        public async Task<ActionResult<Alianza>> PostAlianza(Alianza alianza)
        {
            _context.Alianzas.Add(alianza);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAlianza), new { id = alianza.Id }, alianza);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlianza(int id, Alianza alianza)
        {
            if (id != alianza.Id) return BadRequest();
            _context.Entry(alianza).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlianzaExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlianza(int id)
        {
            var alianza = await _context.Alianzas.FindAsync(id);
            if (alianza == null) return NotFound();
            _context.Alianzas.Remove(alianza);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AlianzaExists(int id) => _context.Alianzas.Any(e => e.Id == id);
    }
}
