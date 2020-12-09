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
    public class OperarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OperarioController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Operario
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Operario>>> GetOperario()
        //{
        //    return await _context.Operario.ToListAsync();
        //}
        [HttpGet]
        public IEnumerable<Operario> Get()
        {
            var xitem = _context.Operario.ToList();
            return xitem;
        }

        // GET: api/Operario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operario>> GetOperario(int id)
        {
            var Operario = await _context.Operario.FindAsync(id);

            if (Operario == null)
            {
                return NotFound();
            }

            return Operario;
        }

        // PUT: api/Operario/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperario(int id, Operario Operario)
        {
            if (id != Operario.CG_OPER)
            {
                return BadRequest();
            }

            _context.Entry(Operario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperarioExists(id))
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

        // POST: api/Operario
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Operario>> PostOperario(Operario Operario)
        {
            _context.Operario.Add(Operario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OperarioExists(Operario.CG_OPER))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOperario", new { id = Operario.CG_OPER }, Operario);
        }

        // DELETE: api/Operario/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Operario>> DeleteOperario(int id)
        {
            var Operario = await _context.Operario.FindAsync(id);
            if (Operario == null)
            {
                return NotFound();
            }

            _context.Operario.Remove(Operario);
            await _context.SaveChangesAsync();

            return Operario;
        }

        private bool OperarioExists(int id)
        {
            return _context.Operario.Any(e => e.CG_OPER == id);
        }
    }
}