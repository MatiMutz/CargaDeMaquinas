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

namespace SupplyChain
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesFabricacionMPController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdenesFabricacionMPController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{idOrden}")]
        public IEnumerable<ModeloOrdenFabricacionMP> Get(int idOrden)
        {
            try
            {
                string xSQL = string.Format("SELECT REGISTRO, CG_ART, DES_ART, " +
                                            "convert(numeric(10, 4), (STOCK * - 1)) AS STOCK, LOTE, DESPACHO, SERIE " +
                                            "FROM Pedidos WHERE TIPOO IN (10, 11, 28) AND CG_ORDEN = 4 AND CG_ORDF = {0} ORDER BY REGISTRO"
                                            ,idOrden);
                return _context.OrdenesFabricacionMP.FromSqlRaw(xSQL).ToList<ModeloOrdenFabricacionMP>();
            }
            catch
            {
                return new List<ModeloOrdenFabricacionMP>();
            }
        }
    }
}
