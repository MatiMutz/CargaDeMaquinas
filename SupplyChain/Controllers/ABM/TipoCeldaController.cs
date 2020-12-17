using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace SupplyChain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCeldaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoCeldaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TipoCelda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCelda>>> GetTipoCelda()
        {
            return await _context.TipoCelda.ToListAsync();
        }

        // GET: api/TipoCelda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCelda>> GetTipoCelda(int id)
        {
            var TipoCelda = await _context.TipoCelda.FindAsync(id);

            if (TipoCelda == null)
            {
                return NotFound();
            }

            return TipoCelda;
        }

        // PUT: api/TipoCelda/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoCelda(int id, TipoCelda TipoCelda)
        {
            if (id != TipoCelda.CG_TIPOCELDA)
            {
                return BadRequest();
            }

            _context.Entry(TipoCelda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCeldaExists(id))
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

        // POST: api/TipoCelda
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TipoCelda>> PostTipoCelda(TipoCelda TipoCelda)
        {
            _context.TipoCelda.Add(TipoCelda);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TipoCeldaExists(TipoCelda.CG_TIPOCELDA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTipoCelda", new { id = TipoCelda.CG_TIPOCELDA }, TipoCelda);
        }

        // DELETE: api/TipoCelda/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoCelda>> DeleteTipoCelda(int id)
        {
            var TipoCelda = await _context.TipoCelda.FindAsync(id);
            if (TipoCelda == null)
            {
                return NotFound();
            }

            _context.TipoCelda.Remove(TipoCelda);
            await _context.SaveChangesAsync();

            return TipoCelda;
        }

        private bool TipoCeldaExists(int id)
        {
            return _context.TipoCelda.Any(e => e.CG_TIPOCELDA == id);
        }
    }
}