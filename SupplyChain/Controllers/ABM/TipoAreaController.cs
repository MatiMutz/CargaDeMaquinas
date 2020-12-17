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
    public class TipoAreaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoAreaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TipoArea
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoArea>>> GetTipoArea()
        {
            return await _context.TipoArea.ToListAsync();
        }

        // GET: api/TipoArea/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoArea>> GetTipoArea(int id)
        {
            var TipoArea = await _context.TipoArea.FindAsync(id);

            if (TipoArea == null)
            {
                return NotFound();
            }

            return TipoArea;
        }

        // PUT: api/TipoArea/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoArea(int id, TipoArea TipoArea)
        {
            if (id != TipoArea.CG_TIPOAREA)
            {
                return BadRequest();
            }

            _context.Entry(TipoArea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoAreaExists(id))
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

        // POST: api/TipoArea
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TipoArea>> PostTipoArea(TipoArea TipoArea)
        {
            _context.TipoArea.Add(TipoArea);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TipoAreaExists(TipoArea.CG_TIPOAREA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTipoArea", new { id = TipoArea.CG_TIPOAREA }, TipoArea);
        }

        // DELETE: api/TipoArea/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoArea>> DeleteTipoArea(int id)
        {
            var TipoArea = await _context.TipoArea.FindAsync(id);
            if (TipoArea == null)
            {
                return NotFound();
            }

            _context.TipoArea.Remove(TipoArea);
            await _context.SaveChangesAsync();

            return TipoArea;
        }

        private bool TipoAreaExists(int id)
        {
            return _context.TipoArea.Any(e => e.CG_TIPOAREA == id);
        }
    }
}