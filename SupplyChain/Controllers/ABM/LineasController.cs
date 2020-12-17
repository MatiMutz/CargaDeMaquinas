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
    public class LineasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LineasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Lineas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lineas>>> GetLineas()
        {
            return await _context.Lineas.ToListAsync();
        }

        // GET: api/Lineas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lineas>> GetLineas(int id)
        {
            var Lineas = await _context.Lineas.FindAsync(id);

            if (Lineas == null)
            {
                return NotFound();
            }

            return Lineas;
        }

        // PUT: api/Lineas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineas(int id, Lineas Lineas)
        {
            if (id != Lineas.CG_LINEA)
            {
                return BadRequest();
            }

            _context.Entry(Lineas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineasExists(id))
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

        // POST: api/Lineas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Lineas>> PostLineas(Lineas Lineas)
        {
            _context.Lineas.Add(Lineas);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LineasExists(Lineas.CG_LINEA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLineas", new { id = Lineas.CG_LINEA }, Lineas);
        }

        // DELETE: api/Lineas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lineas>> DeleteLineas(int id)
        {
            var Lineas = await _context.Lineas.FindAsync(id);
            if (Lineas == null)
            {
                return NotFound();
            }

            _context.Lineas.Remove(Lineas);
            await _context.SaveChangesAsync();

            return Lineas;
        }

        private bool LineasExists(int id)
        {
            return _context.Lineas.Any(e => e.CG_LINEA == id);
        }
    }
}