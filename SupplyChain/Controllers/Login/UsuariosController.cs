using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplyChain.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{usuario}/{contras}")]
        public async Task<ActionResult<Usuarios>> Get(string usuario, string contras)
        {
            try
            {
                //string xSQL = $"SELECT Usuario, Contras FROM USUARIOS WHERE Usuario = '{usuario}' AND CONTRAS = '{contras}'";
                //return await _context.Usuarios.FromSqlRaw(xSQL).FirstOrDefaultAsync();
                return await _context.Usuarios
                    .Where(u => u.Usuario == usuario && u.Contras == contras).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
