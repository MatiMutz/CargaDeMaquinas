using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplyChain;

namespace SupplyChain
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolutionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Solution
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Solution>>> GetSolution()
        {
            return await _context.Solution.ToListAsync();
        }

        // GET: api/Solution/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Solution>> GetSolution(int CAMPO)
        {
            var Solution = await _context.Solution.FindAsync(CAMPO);

            if (Solution == null)
            {
                return NotFound();
            }

            return Solution;
        }

        // PUT: api/Solution/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolution(string Id, Solution Solution)
        {
            if (Id != Solution.CAMPO)
            {
                return BadRequest();
            }

            _context.Entry(Solution).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolutionExists(Id))
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

        // POST: api/Solution
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Solution>> PostSolution(Solution Solution)
        {
            _context.Solution.Add(Solution);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SolutionExists(Solution.CAMPO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSolution", new { id = Solution.CAMPO }, Solution);
        }

        // DELETE: api/Solution/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Solution>> DeleteSolution(string CAMPO)
        {
            var Solution = await _context.Solution.FindAsync(CAMPO);
            if (Solution == null)
            {
                return NotFound();
            }

            _context.Solution.Remove(Solution);
            await _context.SaveChangesAsync();

            return Solution;
        }

        private bool SolutionExists(string CAMPO)
        {
            return _context.Solution.Any(e => e.CAMPO == CAMPO);
        }
    }
}