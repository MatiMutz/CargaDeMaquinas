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
    public class IndicadorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IndicadorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Indicadores
        [HttpGet]
        public IEnumerable<Indicadores> Get()
        {
            try
            {
                // Llena FECHA_PREVISTA_FABRICACION en casos que esté en null
                //_context.Database.ExecuteSqlRawAsync("EXEC NET_PCP_Carga_Poner_Fecha_Prevista_Fabricacion 0");

                // Llena tabla de carga
                List<Indicadores> dbCarga;
                dbCarga = _context.Indicadoress.FromSqlRaw("use Solutiion EXEC NET_Listado_PedidosPendientes @xTipo = 'T', @xCliDesde = 1, @xCliHasta = 99999, @xFecDesde = '03-03-2021', @xFecHasta = '12-12-2100', @xDepos = 0, @xTipoFec = 'P', @xCgClas = 0; ").ToList<Indicadores>();
                dbCarga = dbCarga.OrderBy(x => x.Pedido).ToList<Indicadores>();

                return dbCarga;
            }
            catch (Exception ex)
            {
                return new List<Indicadores>();
            }
        }
    }
}
