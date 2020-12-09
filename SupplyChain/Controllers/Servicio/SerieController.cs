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
    public class SerieController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SerieController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Serie
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Serie>>> GetSerie()
        //{
        //    return await _context.Serie.ToListAsync();
        //}

        [HttpGet]
        public IEnumerable<Serie> Get()
        {
            var xitem = _context.Serie.ToList();
            return xitem;
        }

        // GET: api/Serie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Serie>> GetSerie(int id)
        {
            var Serie = await _context.Serie.FindAsync(id);

            if (Serie == null)
            {
                return NotFound();
            }

            return Serie;
        }

        // PUT: api/Serie/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSerie(int id, Serie Serie)
        {
            if (id != Serie.Id)
            {
                return BadRequest();
            }

            _context.Entry(Serie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SerieExists(id))
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

        // POST: api/Serie
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Serie>> PostSerie(Serie Serie)
        {
            _context.Serie.Add(Serie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SerieExists(Serie.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSerie", new { id = Serie.Id }, Serie);
        }

        // DELETE: api/Serie/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Serie>> DeleteSerie(int id)
        {
            var Serie = await _context.Serie.FindAsync(id);
            if (Serie == null)
            {
                return NotFound();
            }

            _context.Serie.Remove(Serie);
            await _context.SaveChangesAsync();

            return Serie;
        }

        private bool SerieExists(int id)
        {
            return _context.Serie.Any(e => e.Id == id);
        }
    }
}