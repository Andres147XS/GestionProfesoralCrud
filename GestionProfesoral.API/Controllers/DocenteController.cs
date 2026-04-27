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
    public class DocenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocenteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Docente>>> GetDocentes()
        {
            return await _context.Docentes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Docente>> GetDocente(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null) return NotFound();
            return docente;
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost]
        public async Task<ActionResult<Docente>> PostDocente(Docente docente)
        {
            // Uso de Procedimiento Almacenado para insertar un docente
            var parameters = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@Cedula", docente.Cedula),
                new Microsoft.Data.SqlClient.SqlParameter("@Nombres", docente.Nombres),
                new Microsoft.Data.SqlClient.SqlParameter("@Apellidos", docente.Apellidos),
                new Microsoft.Data.SqlClient.SqlParameter("@Correo", docente.Correo),
                new Microsoft.Data.SqlClient.SqlParameter("@Cargo", docente.Cargo ?? (object)DBNull.Value)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_InsertarDocente @Cedula, @Nombres, @Apellidos, @Correo, @Cargo", parameters);
            }
            catch (Exception)
            {
                if (DocenteExists(docente.Cedula)) return Conflict();
                else throw;
            }

            return CreatedAtAction(nameof(GetDocente), new { id = docente.Cedula }, docente);
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocente(int id, Docente docente)
        {
            if (id != docente.Cedula) return BadRequest();
            _context.Entry(docente).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocenteExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocente(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null) return NotFound();
            _context.Docentes.Remove(docente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DocenteExists(int id) => _context.Docentes.Any(e => e.Cedula == id);
    }
}