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
    public class ParadaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParadaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parada>>> GetUnidades()
        {
            return await _context.Parada.ToListAsync();
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parada>> GetUnidad(int id)
        {
            var parada = await _context.Parada.FindAsync(id);

            if (parada == null)
            {
                return NotFound();
            }

            return parada;
        }

        // PUT: api/Unidades/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidad(int id, Parada parada)
        {
            if (id != parada.CP)
            {
                return BadRequest();
            }

            _context.Entry(parada).State = EntityState.Modified;

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
        public async Task<ActionResult<Parada>> PostUnidad(Parada parada)
        {
            _context.Parada.Add(parada);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UnidadExists(parada.CP))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUnidad", new { id = parada.CP }, parada);
        }

        // DELETE: api/Unidades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Parada>> DeleteUnidad(int id)
        {
            var parada = await _context.Parada.FindAsync(id);
            if (parada == null)
            {
                return NotFound();
            }

            _context.Parada.Remove(parada);
            await _context.SaveChangesAsync();

            return parada;
        }

        private bool UnidadExists(decimal id)
        {
            return _context.Parada.Any(e => e.CP == id);
        }
    }
}
