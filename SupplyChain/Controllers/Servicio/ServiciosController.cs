using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SupplyChain
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        //private IHostingEnvironment hostingEnv;
        private readonly AppDbContext _context;

        public ServiciosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Service> Get()
        {
            var xitem = _context.Servicios.ToList();
            return xitem;
        }

        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetServicios(string id)
        {
            var Servicios = await _context.Servicios.FindAsync(id);

            if (Servicios == null)
            {
                return NotFound();
            }

            return Servicios;
        }

        // PUT: api/Servicios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicios(string id, Service Servicios)
        {
            if (id != Servicios.PEDIDO)
            {
                return BadRequest();
            }

            _context.Entry(Servicios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!ServiciosExists(id))
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


        // POST: api/Servicios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Service>> PostServicios(Service Servicios)
        {
            _context.Servicios.Add(Servicios);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServiciosExists(Servicios.PEDIDO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServicios", new { id = Servicios.PEDIDO }, Servicios);
        }

        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Service>> DeleteServicios(string id)
        {
            var Servicios = await _context.Servicios.FindAsync(id);
            if (Servicios == null)
            {
                return NotFound();
            }

            _context.Servicios.Remove(Servicios);
            await _context.SaveChangesAsync();

            return Servicios;
        }

        private bool ServiciosExists(string id)
        {
            return _context.Servicios.Any(e => e.PEDIDO == id);
        }
    }
}