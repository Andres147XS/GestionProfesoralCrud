using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionProfesoral.API.Controllers
{
    // CRUD de la entidad Beca
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BecaController : ControllerBase
    {

        private readonly AppDbContext _context;

        public BecaController(AppDbContext context)
        {
            _context = context;
        }

        // GET  (lista todas las becas)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beca>>> GetBecas()
        {
            return await _context.Becas.ToListAsync();
        }

        // GET (busca por Estudios)
        [HttpGet("{id}")]
        public async Task<ActionResult<Beca>> GetBeca(int id)
        {
            var beca = await _context.Becas.FindAsync(id);
            if (beca == null) return NotFound();
            return beca;
        }

        // POST (crea una beca)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<Beca>> PostBeca(Beca beca)
        {
            _context.Becas.Add(beca);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBeca), new { id = beca.Estudios }, beca);
        }

        // PUT (actualiza una beca (id = Estudios))
        [Authorize(Roles = "Administrador,Docente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeca(int id, Beca beca)
        {
            if (id != beca.Estudios) return BadRequest();
            _context.Entry(beca).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!BecaExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE  (elimina una beca por Estudios)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeca(int id)
        {
            var beca = await _context.Becas.FindAsync(id);
            if (beca == null) return NotFound();
            _context.Becas.Remove(beca);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método auxiliar para validar existencia (usado en PUT)
        private bool BecaExists(int id) => _context.Becas.Any(e => e.Estudios == id);
    }
}
