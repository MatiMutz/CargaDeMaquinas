using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace SupplyChain
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UnidadesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Unidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unidades>>> GetUnidades()
        {
            return await _context.Unidades.ToListAsync();
        }

        // GET: api/Unidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Unidades>> GetUnidad(string id)
        {
            var unidad = await _context.Unidades.FindAsync(id);

            if (unidad == null)
            {
                return NotFound();
            }

            return unidad;
        }

        // PUT: api/Unidades/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidad(string id, Unidades unidad)
        {
            if (id != unidad.UNID)
            {
                return BadRequest();
            }

            _context.Entry(unidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnidadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Unidades
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Unidades>> PostUnidad(Unidades unidad)
        {
            _context.Unidades.Add(unidad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UnidadExists(unidad.UNID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUnidad", new { id = unidad.UNID }, unidad);
        }

        // DELETE: api/Unidades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unidades>> DeleteUnidad(string id)
        {
            var unidad = await _context.Unidades.FindAsync(id);
            if (unidad == null)
            {
                return NotFound();
            }

            _context.Unidades.Remove(unidad);
            await _context.SaveChangesAsync();

            return unidad;
        }

        private bool UnidadExists(string id)
        {
            return _context.Unidades.Any(e => e.UNID == id);
        }
    }
}
