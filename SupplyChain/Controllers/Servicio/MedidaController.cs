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
    public class MedidaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedidaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Medida
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medida>>> GetMedida()
        {
            return await _context.Medida.ToListAsync();
        }

        // GET: api/Medida/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medida>> GetMedida(int id)
        {
            var Medida = await _context.Medida.FindAsync(id);

            if (Medida == null)
            {
                return NotFound();
            }

            return Medida;
        }

        // PUT: api/Medida/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedida(int id, Medida Medida)
        {
            if (id != Medida.Id)
            {
                return BadRequest();
            }

            _context.Entry(Medida).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedidaExists(id))
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

        // POST: api/Medida
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Medida>> PostMedida(Medida Medida)
        {
            _context.Medida.Add(Medida);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MedidaExists(Medida.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMedida", new { id = Medida.Id }, Medida);
        }

        // DELETE: api/Medida/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Medida>> DeleteMedida(int id)
        {
            var Medida = await _context.Medida.FindAsync(id);
            if (Medida == null)
            {
                return NotFound();
            }

            _context.Medida.Remove(Medida);
            await _context.SaveChangesAsync();

            return Medida;
        }

        private bool MedidaExists(int id)
        {
            return _context.Medida.Any(e => e.Id == id);
        }
    }
}