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
    public class TiposNoConfController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposNoConfController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TiposNoConf
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiposNoConf>>> GetTiposNoConf()
        {
            return await _context.TiposNoConf.ToListAsync();
        }

        // GET: api/TiposNoConf/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TiposNoConf>> GetTiposNoConf(int id)
        {
            var TiposNoConf = await _context.TiposNoConf.FindAsync(id);

            if (TiposNoConf == null)
            {
                return NotFound();
            }

            return TiposNoConf;
        }

        // PUT: api/TiposNoConf/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiposNoConf(int id, TiposNoConf TiposNoConf)
        {
            if (id != TiposNoConf.Cg_TipoNc)
            {
                return BadRequest();
            }

            _context.Entry(TiposNoConf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiposNoConfExists(id))
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

        // POST: api/TiposNoConf
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TiposNoConf>> PostTiposNoConf(TiposNoConf TiposNoConf)
        {
            _context.TiposNoConf.Add(TiposNoConf);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TiposNoConfExists(TiposNoConf.Cg_TipoNc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTiposNoConf", new { id = TiposNoConf.Cg_TipoNc }, TiposNoConf);
        }

        // DELETE: api/TiposNoConf/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TiposNoConf>> DeleteTiposNoConf(int id)
        {
            var TiposNoConf = await _context.TiposNoConf.FindAsync(id);
            if (TiposNoConf == null)
            {
                return NotFound();
            }

            _context.TiposNoConf.Remove(TiposNoConf);
            await _context.SaveChangesAsync();

            return TiposNoConf;
        }

        private bool TiposNoConfExists(int id)
        {
            return _context.TiposNoConf.Any(e => e.Cg_TipoNc == id);
        }
    }
}