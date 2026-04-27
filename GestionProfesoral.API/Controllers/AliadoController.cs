using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionProfesoral.API.Data;
using GestionProfesoral.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionProfesoral.API.Controllers
{
    // CRUD de la entidad Aliado.

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AliadoController : ControllerBase
    {

        private readonly AppDbContext _context;


        public AliadoController(AppDbContext context)
        {
            _context = context;
        }

        // GET (lista todos los aliados)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aliado>>> GetAliados()
        {

            return await _context.Aliados.ToListAsync();
        }

        // GET por id (busca por id)
        [HttpGet("{id}")]
        public async Task<ActionResult<Aliado>> GetAliado(long id)
        {

            var aliado = await _context.Aliados.FindAsync(id);
            if (aliado == null) return NotFound();
            return aliado;
        }

        // POST (crea un aliado)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<Aliado>> PostAliado(Aliado aliado)
        {

            _context.Aliados.Add(aliado);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAliado), new { id = aliado.Nit }, aliado);
        }

        // PUT (actualiza)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAliado(long id, Aliado aliado)
        {

            if (id != aliado.Nit) return BadRequest();

            _context.Entry(aliado).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!AliadoExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE  (elimina por Id)
        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAliado(long id)
        {
            var aliado = await _context.Aliados.FindAsync(id);
            if (aliado == null) return NotFound();
            _context.Aliados.Remove(aliado);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método auxiliar para validar existencia (usado en PUT)
        private bool AliadoExists(long id) => _context.Aliados.Any(e => e.Nit == id);
    }
}
