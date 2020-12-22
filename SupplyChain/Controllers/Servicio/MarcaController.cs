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
    public class MarcaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MarcaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Marca
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marca>>> GetMarca()
        {
            return await _context.Marca.ToListAsync();
        }

        // GET: api/Marca/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Marca>> GetMarca(string id)
        {
            var Marca = await _context.Marca.FindAsync(id);

            if (Marca == null)
            {
                return NotFound();
            }

            return Marca;
        }

        // PUT: api/Marca/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarca(string id, Marca Marca)
        {
            if (id != Marca.MARCA)
            {
                return BadRequest();
            }

            _context.Entry(Marca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarcaExists(id))
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

        // POST: api/Marca
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Marca>> PostMarca(Marca Marca)
        {
            _context.Marca.Add(Marca);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MarcaExists(Marca.MARCA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMarca", new { id = Marca.MARCA }, Marca);
        }

        // DELETE: api/Marca/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Marca>> DeleteMarca(string id)
        {
            var Marca = await _context.Marca.FindAsync(id);
            if (Marca == null)
            {
                return NotFound();
            }

            _context.Marca.Remove(Marca);
            await _context.SaveChangesAsync();

            return Marca;
        }

        private bool MarcaExists(string id)
        {
            return _context.Marca.Any(e => e.MARCA == id);
        }
    }
}