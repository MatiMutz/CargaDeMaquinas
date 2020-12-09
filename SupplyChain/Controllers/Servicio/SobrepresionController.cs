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
    public class SobrepresionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SobrepresionController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Sobrepresion
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Sobrepresion>>> GetSobrepresion()
        //{
        //    return await _context.Sobrepresion.ToListAsync();
        //}

        [HttpGet]
        public IEnumerable<Sobrepresion> Get()
        {
            var xitem = _context.Sobrepresion.ToList();
            return xitem;
        }

        // GET: api/Sobrepresion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sobrepresion>> GetSobrepresion(int id)
        {
            var Sobrepresion = await _context.Sobrepresion.FindAsync(id);

            if (Sobrepresion == null)
            {
                return NotFound();
            }

            return Sobrepresion;
        }

        // PUT: api/Sobrepresion/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSobrepresion(int id, Sobrepresion Sobrepresion)
        {
            if (id != Sobrepresion.Id)
            {
                return BadRequest();
            }

            _context.Entry(Sobrepresion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SobrepresionExists(id))
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

        // POST: api/Sobrepresion
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Sobrepresion>> PostSobrepresion(Sobrepresion Sobrepresion)
        {
            _context.Sobrepresion.Add(Sobrepresion);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SobrepresionExists(Sobrepresion.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSobrepresion", new { id = Sobrepresion.Id }, Sobrepresion);
        }

        // DELETE: api/Sobrepresion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sobrepresion>> DeleteSobrepresion(int id)
        {
            var Sobrepresion = await _context.Sobrepresion.FindAsync(id);
            if (Sobrepresion == null)
            {
                return NotFound();
            }

            _context.Sobrepresion.Remove(Sobrepresion);
            await _context.SaveChangesAsync();

            return Sobrepresion;
        }

        private bool SobrepresionExists(int id)
        {
            return _context.Sobrepresion.Any(e => e.Id == id);
        }
    }
}