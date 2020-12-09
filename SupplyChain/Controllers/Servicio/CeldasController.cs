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
    public class CeldasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CeldasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Celdas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Celdas>>> GetCeldas()
        {
            return await _context.Celdas.ToListAsync();
        }

        // GET: api/Celdas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Celdas>> GetCeldas(string id)
        {
            var Celdas = await _context.Celdas.FindAsync(id);

            if (Celdas == null)
            {
                return NotFound();
            }

            return Celdas;
        }

        // PUT: api/Celdas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCeldas(string id, Celdas Celdas)
        {
            if (id != Celdas.CG_CELDA)
            {
                return BadRequest();
            }

            _context.Entry(Celdas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CeldasExists(id))
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

        // POST: api/Celdas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Celdas>> PostCeldas(Celdas Celdas)
        {
            _context.Celdas.Add(Celdas);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CeldasExists(Celdas.CG_CELDA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCeldas", new { id = Celdas.CG_CELDA }, Celdas);
        }

        // DELETE: api/Celdas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Celdas>> DeleteCeldas(string id)
        {
            var Celdas = await _context.Celdas.FindAsync(id);
            if (Celdas == null)
            {
                return NotFound();
            }

            _context.Celdas.Remove(Celdas);
            await _context.SaveChangesAsync();

            return Celdas;
        }

        private bool CeldasExists(string id)
        {
            return _context.Celdas.Any(e => e.CG_CELDA == id);
        }
    }
}