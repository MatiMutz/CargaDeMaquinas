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
    public class OrdenesFabricacionSEController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdenesFabricacionSEController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{idOrden}")]
        public IEnumerable<ModeloOrdenFabricacionSE> Get(int idOrden)
        {
            try
            {
                string xSQL = String.Format("SELECT PE.REGISTRO, " +
                                            "PE.CG_ART, PE.DES_ART, PE.DESPACHO, " +
                                            "(PE.STOCK * - 1) AS STOCK, PE.LOTE, PE.VALE, PE.UBICACION, PR.CG_LINEA " +
                                            "FROM Pedidos PE, Prod PR WHERE PE.CG_ART = PR.CG_PROD AND PE.TIPOO IN (10, 11, 28) " +
                                            "AND PE.CG_ORDEN = 3 AND PE.CG_ORDF = {0} " +
                                            "ORDER BY PE.REGISTRO",
                                            idOrden);
                return _context.OrdenesFabricacionSE.FromSqlRaw(xSQL).ToList<ModeloOrdenFabricacionSE>();
            }
            catch
            {
                return new List<ModeloOrdenFabricacionSE>();
            }
        }
    }
}
