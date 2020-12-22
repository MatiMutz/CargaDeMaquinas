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
    public class ProTareaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProTareaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProTarea>>> GetUnidades()
        {
            return await _context.ProTarea.ToListAsync();
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProTarea>> GetUnidad(string id)
        {
            var protarea = await _context.ProTarea.FindAsync(id);

            if (protarea == null)
            {
                return NotFound();
            }

            return protarea;
        }

        // PUT: api/Unidades/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidad(string id, ProTarea protarea)
        {
            if (id != protarea.TAREAPROC)
            {
                return BadRequest();
            }

            _context.Entry(protarea).State = EntityState.Modified;

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
        public async Task<ActionResult<ProTarea>> PostUnidad(ProTarea protarea)
        {
            _context.ProTarea.Add(protarea);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UnidadExists(protarea.TAREAPROC))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUnidad", new { id = protarea.TAREAPROC }, protarea);
        }

        // DELETE: api/Unidades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProTarea>> DeleteUnidad(string id)
        {
            var protarea = await _context.ProTarea.FindAsync(id);
            if (protarea == null)
            {
                return NotFound();
            }

            _context.ProTarea.Remove(protarea);
            await _context.SaveChangesAsync();

            return protarea;
        }

        private bool UnidadExists(string id)
        {
            return _context.ProTarea.Any(e => e.TAREAPROC == id);
        }
    }
}
