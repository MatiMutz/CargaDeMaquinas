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
    public class CatOpeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CatOpeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CateOperarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatOpe>>> GetCateOperarios()
        {
            return await _context.CateOperarios.ToListAsync();
        }

        // GET: api/CateOperarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CatOpe>> GetCatOpe(string id)
        {
            var CatOpe = await _context.CateOperarios.FindAsync(id);

            if (CatOpe == null)
            {
                return NotFound();
            }

            return CatOpe;
        }

        // PUT: api/CateOperarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatOpe(string id, CatOpe CatOpe)
        {
            if (id != CatOpe.CG_CATEOP)
            {
                return BadRequest();
            }

            _context.Entry(CatOpe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatOpeExists(id))
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

        // POST: api/CateOperarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CatOpe>> PostCatOpe(CatOpe CatOpe)
        {
            _context.CateOperarios.Add(CatOpe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CatOpeExists(CatOpe.CG_CATEOP))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCatOpe", new { id = CatOpe.CG_CATEOP }, CatOpe);
        }

        // DELETE: api/CateOperarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CatOpe>> DeleteCatOpe(string id)
        {
            var CatOpe = await _context.CateOperarios.FindAsync(id);
            if (CatOpe == null)
            {
                return NotFound();
            }

            _context.CateOperarios.Remove(CatOpe);
            await _context.SaveChangesAsync();

            return CatOpe;
        }

        private bool CatOpeExists(string id)
        {
            return _context.CateOperarios.Any(e => e.CG_CATEOP == id);
        }
    }
}