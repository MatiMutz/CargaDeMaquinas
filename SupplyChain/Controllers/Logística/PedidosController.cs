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
            string xSQL = string.Format("" +
                "SELECT Pedidos.REGISTRO, Pedidos.PEDIDO, Pedidos.REMITO, Pedidos.FLAG, Pedidos.CG_ORDF, Pedidos.TIPOO, Pedidos.CG_TIRE, Pedidos.DES_CLI, Pedidos.CG_ART, Pedidos.DES_ART, Pedidos.DESPACHO, Pedidos.LOTE, Pedidos.FE_MOV, Pedidos.AVISO " +
                "FROM((Pedcli INNER JOIN Programa ON Pedcli.PEDIDO = Programa.PEDIDO) " +
                "INNER JOIN Pedidos ON pedcli.PEDIDO = Pedidos.PEDIDO) " +
                "where(pedidos.FLAG = 0 AND Programa.CG_ESTADO = 3 AND Pedidos.CG_ORDF != 0 AND(Pedidos.TIPOO = 1)) " +
                "UNION SELECT Pedidos.REGISTRO, Pedidos.PEDIDO, Pedidos.REMITO, Pedidos.FLAG, Pedidos.CG_ORDF, Pedidos.TIPOO, Pedidos.CG_TIRE, Pedidos.DES_CLI, Pedidos.CG_ART, Pedidos.DES_ART, Pedidos.DESPACHO, Pedidos.LOTE, Pedidos.FE_MOV, Pedidos.AVISO " +
                "FROM((Pedcli INNER JOIN Programa ON Pedcli.PEDIDO = Programa.PEDIDO) " +
                "INNER JOIN Pedidos ON pedcli.PEDIDO = Pedidos.PEDIDO) " +
                "where Pedcli.PEDIDO NOT IN(select PEDIDO from Pedidos where TIPOO = 1) AND Programa.CG_ESTADO = 3  AND Pedcli.CANTPED > 0 AND Pedidos.TIPOO != 28");
            return _context.Pedidos.FromSqlRaw(xSQL).ToList<Pedidos>();
        }

        // GET: api/Pedidos/BuscarPorPedido/{Pedido}
        [HttpGet("BuscarPorPedido/{Pedido}")]
        public async Task<ActionResult<List<Pedidos>>> BuscarPorPedido(string Pedido)
        {
            List<Pedidos> lPedidos = new List<Pedidos>();
            if (_context.Pedidos.Any())
            {
                lPedidos = await _context.Pedidos.Where(p => p.PEDIDO.ToString() == Pedido).ToListAsync();
            }
            if (lPedidos == null)
            {
                return NotFound();
            }
            return lPedidos;
        }

        // GET: api/Pedidos/BuscarPorCliente/{Cliente}
        [HttpGet("BuscarPorCliente/{Cliente}")]
        public async Task<ActionResult<List<Pedidos>>> BuscarPorCliente(string Cliente)
        {
            List<Pedidos> lPedidos = new List<Pedidos>();
            if (_context.Pedidos.Any())
            {
                lPedidos = await _context.Pedidos.Where(p => p.DES_CLI == Cliente).ToListAsync();
            }
            if (lPedidos == null)
            {
                return NotFound();
            }
            return lPedidos;
        }

        // GET: api/Pedidos/BuscarPorCodigo/{Codigo}
        [HttpGet("BuscarPorCodigo/{Codigo}")]
        public async Task<ActionResult<List<Pedidos>>> BuscarPorCodigo(string Codigo)
        {
            List<Pedidos> lPedidos = new List<Pedidos>();
            if (_context.Pedidos.Any())
            {
                lPedidos = await _context.Pedidos.Where(p => p.CG_ART == Codigo).ToListAsync();
            }
            if (lPedidos == null)
            {
                return NotFound();
            }
            return lPedidos;
        }

        // GET: api/Pedidos/BuscarPorOF/{OF}
        [HttpGet("BuscarPorOF/{OF}")]
        public async Task<ActionResult<List<Pedidos>>> BuscarPorOF(string OF)
        {
            List<Pedidos> lPedidos = new List<Pedidos>();
            if (_context.Pedidos.Any())
            {
                lPedidos = await _context.Pedidos.Where(p => p.CG_ORDF.ToString() == OF).ToListAsync();
            }
            if (lPedidos == null)
            {
                return NotFound();
            }
            return lPedidos;
        }

        // GET:
        [HttpGet("BuscarTrazabilidad/{Pedido}/{Cliente}/{Codigo}/{Busqueda}")]
        public async Task<ActionResult<List<Pedidos>>> BuscarTrazabilidad(string Pedido, string Cliente, string Codigo, int Busqueda)
        {
            List<Pedidos> lContiene = new List<Pedidos>();
            if (Pedido != "Vacio")
            {
                if (_context.Pedidos.Any())
                {
                    lContiene = await _context.Pedidos.Where(p => p.PEDIDO.ToString().Contains(Pedido) && p.AVISO == "ALTA DE PRODUCTO FABRICADO").OrderByDescending(s => s.PEDIDO).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            else if (Cliente != "Vacio")
            {
                if (_context.Pedidos.Any())
                {
                    lContiene = await _context.Pedidos.Where(p => p.DES_CLI.Contains(Cliente) && p.AVISO == "ALTA DE PRODUCTO FABRICADO").OrderByDescending(s => s.PEDIDO).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            else if (Codigo != "Vacio")
            {
                if (_context.Pedidos.Any())
                {
                    lContiene = await _context.Pedidos.Where(p => p.CG_ART.Contains(Codigo) && p.AVISO == "ALTA DE PRODUCTO FABRICADO").OrderByDescending(s => s.PEDIDO).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            else
            {
                if (_context.Pedidos.Any())
                {
                    lContiene = await _context.Pedidos.Where(p => p.PEDIDO.ToString().Contains(Pedido) && p.DES_CLI.Contains(Cliente) && p.CG_ART.Contains(Codigo) && p.AVISO == "ALTA DE PRODUCTO FABRICADO").OrderByDescending(s => s.PEDIDO).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            return lContiene;
        }

        // GET: api/Pedidos/MostrarTrazabilidad/{Pedido}
        [HttpGet("MostrarTrazabilidad/{Pedido}")]
        public async Task<ActionResult<List<Pedidos>>> MostrarTrazabilidad(string Pedido)
        {
            List<Pedidos> lPedidos = new List<Pedidos>();
            if (_context.Pedidos.Any())
            {
                lPedidos = await _context.Pedidos.Where(p => p.PEDIDO.ToString() == Pedido).ToListAsync();
            }
            if (lPedidos == null)
            {
                return NotFound();
            }
            return lPedidos;
        }
    }
}