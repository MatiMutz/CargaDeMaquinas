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
    public class ProgramaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProgramaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Programa> Get(string PEDIDO)
        {
            string xSQL = string.Format("SELECT Programa.REGISTRO, Programa.PEDIDO, Programa.CG_ESTADO, Programa.CG_ESTADOCARGA, Programa.FE_CIERRE FROM((Pedcli INNER JOIN Programa ON Pedcli.PEDIDO = Programa.PEDIDO) INNER JOIN Pedidos ON pedcli.PEDIDO = Pedidos.PEDIDO) where(pedidos.FLAG = 0 AND Programa.CG_ESTADO = 3 AND Pedidos.CG_ORDF != 0 AND(Pedidos.TIPOO = 1)) UNION SELECT Programa.REGISTRO, Programa.PEDIDO, Programa.CG_ESTADO, Programa.CG_ESTADOCARGA, Programa.FE_CIERRE FROM((Pedcli INNER JOIN Programa ON Pedcli.PEDIDO = Programa.PEDIDO) INNER JOIN Pedidos ON pedcli.PEDIDO = Pedidos.PEDIDO) where Pedcli.PEDIDO NOT IN(select PEDIDO from Pedidos where TIPOO = 1) AND Programa.CG_ESTADO = 3  AND Pedcli.CANTPED > 0 AND Pedidos.TIPOO != 28");
            return _context.Programa.FromSqlRaw(xSQL).ToList<Programa>();
        }


        [HttpGet("GetPedidos")]
        public IEnumerable<Programa> Gets(string PEDIDO)
        {
            string xSQL = string.Format("SELECT * FROM Programa ");
            return _context.Programa.FromSqlRaw(xSQL).ToList<Programa>();
        }


    }
}