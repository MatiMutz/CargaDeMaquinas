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
    public class TipoMatController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoMatController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoMat>>> GetUnidades()
        {
            return await _context.TipoMat.ToListAsync();
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoMat>> GetUnidad(string id)
        {
            var tipomat = await _context.TipoMat.FindAsync(id);

            if (tipomat == null)
            {
                return NotFound();
            }

            return tipomat;
        }

        // PUT: api/Unidades/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidad(string id, TipoMat tipomat)
        {
            if (id != tipomat.TIPO)
            {
                return BadRequest();
            }

            _context.Entry(tipomat).State = EntityState.Modified;

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
        public async Task<ActionResult<TipoMat>> PostUnidad(TipoMat tipomat)
        {
            _context.TipoMat.Add(tipomat);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UnidadExists(tipomat.TIPO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUnidad", new { id = tipomat.TIPO }, tipomat);
        }

        // DELETE: api/Unidades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoMat>> DeleteUnidad(string id)
        {
            var tipomat = await _context.TipoMat.FindAsync(id);
            if (tipomat == null)
            {
                return NotFound();
            }

            _context.TipoMat.Remove(tipomat);
            await _context.SaveChangesAsync();

            return tipomat;
        }

        private bool UnidadExists(string id)
        {
            return _context.TipoMat.Any(e => e.TIPO == id);
        }
    }
}
