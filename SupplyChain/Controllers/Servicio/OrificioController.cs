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
    public class OrificioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrificioController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Orificio
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Orificio>>> GetOrificio()
        //{
        //    return await _context.Orificio.ToListAsync();
        //}

        [HttpGet]
        public IEnumerable<Orificio> Get()
        {
            var xitem = _context.Orificio.ToList();
            return xitem;
        }

        // GET: api/Orificio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orificio>> GetOrificio(int id)
        {
            var Orificio = await _context.Orificio.FindAsync(id);

            if (Orificio == null)
            {
                return NotFound();
            }

            return Orificio;
        }

        // PUT: api/Orificio/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrificio(int id, Orificio Orificio)
        {
            if (id != Orificio.Id)
            {
                return BadRequest();
            }

            _context.Entry(Orificio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrificioExists(id))
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

        // POST: api/Orificio
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Orificio>> PostOrificio(Orificio Orificio)
        {
            _context.Orificio.Add(Orificio);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrificioExists(Orificio.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrificio", new { id = Orificio.Id }, Orificio);
        }

        // DELETE: api/Orificio/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Orificio>> DeleteOrificio(int id)
        {
            var Orificio = await _context.Orificio.FindAsync(id);
            if (Orificio == null)
            {
                return NotFound();
            }

            _context.Orificio.Remove(Orificio);
            await _context.SaveChangesAsync();

            return Orificio;
        }

        private bool OrificioExists(int id)
        {
            return _context.Orificio.Any(e => e.Id == id);
        }
    }
}