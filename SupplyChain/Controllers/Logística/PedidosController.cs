using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SupplyChain;
using SupplyChain.Shared.Models;
namespace SupplyChain
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Pedidos> Get(string PEDIDO)
        {
            string xSQL = string.Format("SELECT Pedidos.REGISTRO, Pedidos.PEDIDO, Pedidos.REMITO, Pedidos.FLAG, Pedidos.CG_ORDF, Pedidos.TIPOO, Pedidos.CG_TIRE, Pedido.DES_CLI FROM((Pedcli INNER JOIN Programa ON Pedcli.PEDIDO = Programa.PEDIDO) INNER JOIN Pedidos ON pedcli.PEDIDO = Pedidos.PEDIDO) where(pedidos.FLAG = 0 AND Programa.CG_ESTADO = 3 AND Pedidos.CG_ORDF != 0 AND(Pedidos.TIPOO = 1)) UNION SELECT Pedidos.REGISTRO, Pedidos.PEDIDO, Pedidos.REMITO, Pedidos.FLAG, Pedidos.CG_ORDF, Pedidos.TIPOO, Pedidos.CG_TIRE, Pedidos.DES_CLI FROM((Pedcli INNER JOIN Programa ON Pedcli.PEDIDO = Programa.PEDIDO) INNER JOIN Pedidos ON pedcli.PEDIDO = Pedidos.PEDIDO) where Pedcli.PEDIDO NOT IN(select PEDIDO from Pedidos where TIPOO = 1) AND Programa.CG_ESTADO = 3  AND Pedcli.CANTPED > 0 AND Pedidos.TIPOO != 28");
            return _context.Pedidos.FromSqlRaw(xSQL).ToList<Pedidos>();
        }

        // GET:
        [HttpGet("BuscarTrazabilidad/{Pedido}/{Cliente}/{Busqueda}")]
        public async Task<ActionResult<List<Pedidos>>> BuscarTrazabilidad(string Pedido, string Cliente, int Busqueda)
        {
            List<Pedidos> lContiene = new List<Pedidos>();
            if (Cliente == "Vacio")
            {
                if (_context.Pedidos.Any())
                {
                    lContiene = await _context.Pedidos.Where(p => p.PEDIDO.ToString().Contains(Pedido)).OrderByDescending(s => s.PEDIDO).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            else if (Pedido == "Vacio")
            {
                if (_context.Pedidos.Any())
                {
                    lContiene = await _context.Pedidos.Where(p => p.DES_CLI.Contains(Cliente)).OrderByDescending(s => s.PEDIDO).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            else if (Pedido != "Vacio" && Cliente != "Vacio")
            {
                if (_context.Pedidos.Any())
                {
                    lContiene = await _context.Pedidos.Where(p => p.PEDIDO.ToString().Contains(Pedido) && p.DES_CLI.Contains(Cliente)).OrderByDescending(s => s.PEDIDO).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            return lContiene;
        }

    }
}