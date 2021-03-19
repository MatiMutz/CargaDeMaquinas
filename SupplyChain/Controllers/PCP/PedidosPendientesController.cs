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
using SupplyChain.Shared.Models;

namespace SupplyChain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosPendientesController : ControllerBase
    {
        private string CadenaConexionSQL = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
        private DataTable dbPedPend;

        private readonly AppDbContext _context;

        public PedidosPendientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PedidosPendientes
        [HttpGet]
        public IEnumerable<ModeloPedidosPendientes> Get()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);

                // Ejecuta SP y me guarda en dbPedPend los Pedidos Pendientes
                String xSQLSelect = "EXEC NET_PCP_PEDIDOS";
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                dbPedPend = xConexionSQL.EjecutarSQL(xSQLSelect);

                // Retorno
                List<ModeloPedidosPendientes> xLista = dbPedPend.AsEnumerable().Select(m => new ModeloPedidosPendientes()
                {
                    PEDIDO = m.Field<int>("PEDIDO"),
                    FE_MOV = m.Field<DateTime>("FE_MOV"),
                    CG_CLI = m.Field<decimal>("CG_CLI"),
                    DES_CLI = m.Field<string>("DES_CLI"),
                    CG_ART = m.Field<string>("CG_ART"),
                    DES_ART = m.Field<string>("DES_ART"),
                    CANTPED = m.Field<decimal>("CANTPED"),
                    ENTRPREV = m.Field<DateTime>("ENTRPREV"),
                    Obseritem = m.Field<string>("Obseritem"),
                    CG_ESTADOCARGA = m.Field<decimal?>("CG_ESTADOCARGA"),
                    DES_ESTADOCARGA = m.Field<string>("DES_ESTADOCARGA"),
                    CG_ORDF = m.Field<int?>("CG_ORDF"),
                    CAMPOCOM1 = m.Field<string>("CAMPOCOM1"),
                    CAMPOCOM2 = m.Field<string>("CAMPOCOM2"),
                    CAMPOCOM3 = m.Field<string>("CAMPOCOM3"),
                    CAMPOCOM4 = m.Field<string>("CAMPOCOM4"),
                    CAMPOCOM5 = m.Field<string>("CAMPOCOM5"),
                    CAMPOCOM6 = m.Field<string>("CAMPOCOM6"),
                    Semana = m.Field<int>("Semana"),
                    LOTE = m.Field<string>("LOTE"),
                    REGISTRO_PEDCLI = m.Field<decimal>("REGISTRO_PEDCLI"),
                }).ToList<ModeloPedidosPendientes>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloPedidosPendientes>();
            }
        }
    }
}
