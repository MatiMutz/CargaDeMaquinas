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
    public class IndicController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IndicController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Indic>>> GetUnidades()
        {
            return await _context.Indic.ToListAsync();
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Indic>> GetUnidad(int id)
        {
            var indic = await _context.Indic.FindAsync(id);

            if (indic == null)
            {
                return NotFound();
            }

            return indic;
        }

        // PUT: api/Unidades/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidad(int id, Indic indic)
        {
            if (id != indic.REGISTRO)
            {
                return BadRequest();
            }

            _context.Entry(indic).State = EntityState.Modified;

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
        public async Task<ActionResult<Indic>> PostUnidad(Indic indic)
        {
            _context.Indic.Add(indic);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UnidadExists(indic.REGISTRO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUnidad", new { id = indic.REGISTRO }, indic);
        }

        // DELETE: api/Unidades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Indic>> DeleteUnidad(int id)
        {
            var indic = await _context.Indic.FindAsync(id);
            if (indic == null)
            {
                return NotFound();
            }

            _context.Indic.Remove(indic);
            await _context.SaveChangesAsync();

            return indic;
        }

        private bool UnidadExists(int id)
        {
            return _context.Indic.Any(e => e.REGISTRO == id);
        }
    }
}
