using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;

namespace GestionProfesoral.API.Controllers
{
    // CRUD de la entidad ApoyoProfesoral
    [Route("api/[controller]")]
    [ApiController]
    public class ApoyoProfesoralController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ApoyoProfesoralController(AppDbContext context)
        {
            _context = context;
        }

        // GET (lista todos los apoyos)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApoyoProfesoral>>> GetApoyos()
        {
            return await _context.Apoyos.ToListAsync();
        }

        // GET (busca por Estudios)
        [HttpGet("{id}")]
        public async Task<ActionResult<ApoyoProfesoral>> GetApoyo(int id)
        {
            var apoyo = await _context.Apoyos.FindAsync(id);
            if (apoyo == null) return NotFound();
            return apoyo;
        }

        // POST (crea un apoyo)
        [HttpPost]
        public async Task<ActionResult<ApoyoProfesoral>> PostApoyo(ApoyoProfesoral apoyo)
        {
            _context.Apoyos.Add(apoyo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetApoyo), new { id = apoyo.Estudios }, apoyo);
        }

        // PUT actualiza (id = Estudios)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApoyo(int id, ApoyoProfesoral apoyo)
        {
            if (id != apoyo.Estudios) return BadRequest();
            _context.Entry(apoyo).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApoyoExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE (elimina por Estudios)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApoyo(int id)
        {
            var apoyo = await _context.Apoyos.FindAsync(id);
            if (apoyo == null) return NotFound();
            _context.Apoyos.Remove(apoyo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método auxiliar para validar existencia (usado en PUT)
        private bool ApoyoExists(int id) => _context.Apoyos.Any(e => e.Estudios == id);
    }
}
