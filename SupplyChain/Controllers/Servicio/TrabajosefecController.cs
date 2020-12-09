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
    public class TrabajosefecController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrabajosefecController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Trabajosefec
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Trabajosefec>>> GetTrabajosefec()
        //{
        //    return await _context.Trabajosefec.ToListAsync();
        //}

        [HttpGet]
        public IEnumerable<Trabajosefec> Get()
        {
            var xitem = _context.Trabajosefec.ToList();
            return xitem;
        }

        // GET: api/Trabajosefec/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trabajosefec>> GetTrabajosefec(int id)
        {
            var Trabajosefec = await _context.Trabajosefec.FindAsync(id);

            if (Trabajosefec == null)
            {
                return NotFound();
            }

            return Trabajosefec;
        }

        // PUT: api/Trabajosefec/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrabajosefec(int id, Trabajosefec Trabajosefec)
        {
            if (id != Trabajosefec.Id)
            {
                return BadRequest();
            }

            _context.Entry(Trabajosefec).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrabajosefecExists(id))
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

        // POST: api/Trabajosefec
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Trabajosefec>> PostTrabajosefec(Trabajosefec Trabajosefec)
        {
            _context.Trabajosefec.Add(Trabajosefec);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TrabajosefecExists(Trabajosefec.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTrabajosefec", new { id = Trabajosefec.Id }, Trabajosefec);
        }

        // DELETE: api/Trabajosefec/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trabajosefec>> DeleteTrabajosefec(int id)
        {
            var Trabajosefec = await _context.Trabajosefec.FindAsync(id);
            if (Trabajosefec == null)
            {
                return NotFound();
            }

            _context.Trabajosefec.Remove(Trabajosefec);
            await _context.SaveChangesAsync();

            return Trabajosefec;
        }

        private bool TrabajosefecExists(int id)
        {
            return _context.Trabajosefec.Any(e => e.Id == id);
        }
    }
}