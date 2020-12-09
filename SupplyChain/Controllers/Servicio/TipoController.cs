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
    public class TipoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Tipo
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Tipo>>> GetTipo()
        //{
        //    return await _context.Tipo.ToListAsync();
        //}

        [HttpGet]
        public IEnumerable<Tipo> Get()
        {
            var xitem = _context.Tipo.ToList();
            return xitem;
        }

        // GET: api/Tipo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipo>> GetTipo(int id)
        {
            var Tipo = await _context.Tipo.FindAsync(id);

            if (Tipo == null)
            {
                return NotFound();
            }

            return Tipo;
        }

        // PUT: api/Tipo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipo(int id, Tipo Tipo)
        {
            if (id != Tipo.Id)
            {
                return BadRequest();
            }

            _context.Entry(Tipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoExists(id))
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

        // POST: api/Tipo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Tipo>> PostTipo(Tipo Tipo)
        {
            _context.Tipo.Add(Tipo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TipoExists(Tipo.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTipo", new { id = Tipo.Id }, Tipo);
        }

        // DELETE: api/Tipo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tipo>> DeleteTipo(int id)
        {
            var Tipo = await _context.Tipo.FindAsync(id);
            if (Tipo == null)
            {
                return NotFound();
            }

            _context.Tipo.Remove(Tipo);
            await _context.SaveChangesAsync();

            return Tipo;
        }

        private bool TipoExists(int id)
        {
            return _context.Tipo.Any(e => e.Id == id);
        }
    }
}