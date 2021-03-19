using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SupplyChain;
using SupplyChain.Shared.Models;

namespace SupplyChain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FabricacionController : ControllerBase
    {
        private string CadenaConexionSQL = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
        private readonly AppDbContext _context;
        private DataTable dbFabricacion;

        public FabricacionController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/Fabricacion
        [HttpGet]
        public List<Fabricacion> Get()
        {

            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);

                                     
                string xSQLCommandString = ("SELECT B.CG_ORDEN, A.CG_ORDF, (select max(cg_ordf) from programa where  CG_ORDFASOC = A.CG_ORDFASOC) ULTIMAORDENASOCIADA" +
                                                     ", (CASE WHEN B.CG_ORDEN=1 THEN 'Producto' ELSE (CASE WHEN B.CG_ORDEN=2 THEN 'Semi-Elaborado de Proceso' ELSE (CASE WHEN B.CG_ORDEN=3 THEN 'Semi-Elaborado' ELSE (CASE WHEN B.CG_ORDEN=4 THEN 'Materia Prima' ELSE (CASE WHEN B.CG_ORDEN=10 THEN 'Insumo No Productivo / Articulo de Reventa' ELSE (CASE WHEN B.CG_ORDEN=11 THEN 'Herramental e Insumos Inventariables' ELSE (CASE WHEN B.CG_ORDEN=12 THEN 'Repuestos' ELSE (CASE WHEN B.CG_ORDEN=13 THEN 'Servicios' ELSE '' END) END) END) END) END) END) END) END) AS CLASE" +
                                                     ", (CASE WHEN A.CG_R='' THEN 'Fabricación' ELSE (CASE WHEN A.CG_R='R' THEN 'Reproceso' ELSE (CASE WHEN A.CG_R='T' THEN 'Retrabajo' ELSE (CASE WHEN A.CG_R='S' THEN 'Seleccion' ELSE (CASE WHEN A.CG_R='A' THEN 'Armado' ELSE '' END) END) END) END) END) AS CG_R" +
                                                     ", A.CG_ESTADOCARGA, A.CG_PROD, A.DES_PROD, A.CANT, A.CANTFAB, B.UNID, A.PROCESO, A.INSUMOS_ENTREGADOS_A_PLANTA, A.PEDIDO" +
                                                     ", A.DIASFAB, A.CG_CELDA, A.CG_FORM, A.FE_ENTREGA, A.FE_EMIT, A.FE_PLAN, A.FE_FIRME, A.FE_CURSO, A.FECHA_PREVISTA_FABRICACION, A.ORDEN" +
                                                     ", A.FE_ANUL, A.FE_CIERRE FROM Programa A, Prod B WHERE CG_REG>=2 AND" +
                                                     "  A.Cg_prod = B.Cg_prod AND (A.CG_ESTADOCARGA = 2 OR A.CG_ESTADOCARGA=3)");
                dbFabricacion = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<Fabricacion> xLista = dbFabricacion.AsEnumerable().Select(m => new Fabricacion()
                {
                    CG_PROD = m.Field<string>("CG_PROD"),
                    DES_PROD = m.Field<string>("DES_PROD"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CG_ORDF = m.Field<int>("CG_ORDF"),
                    ULTIMAORDENASOCIADA = m.Field<int>("ULTIMAORDENASOCIADA"),
                    CLASE = m.Field<string>("CLASE"),
                    CG_R = m.Field<string>("CG_R"),
                    CG_ESTADOCARGA = m.Field<int>("CG_ESTADOCARGA"),
                    CANT = m.Field<decimal>("CANT"),
                    CANTFAB = m.Field<decimal>("CANTFAB"),
                    UNID = m.Field<string>("UNID"),
                    PROCESO = m.Field<string>("PROCESO"),
                    INSUMOS_ENTREGADOS_A_PLANTA = m.Field<bool>("INSUMOS_ENTREGADOS_A_PLANTA"),
                    PEDIDO = m.Field<int>("PEDIDO"),
                    DIASFAB = m.Field<decimal>("DIASFAB"),
                    CG_CELDA = m.Field<string>("CG_CELDA"),
                    CG_FORM = m.Field<int>("CG_FORM"),
                    FE_ENTREGA = m.Field<DateTime?>("FE_ENTREGA"),
                    FE_EMIT = m.Field<DateTime?>("FE_EMIT"),
                    FE_PLAN = m.Field<DateTime?>("FE_PLAN"),
                    FE_FIRME = m.Field<DateTime?>("FE_FIRME"),
                    FE_CURSO = m.Field<DateTime?>("FE_CURSO"),
                    FECHA_PREVISTA_FABRICACION = m.Field<DateTime?>("FECHA_PREVISTA_FABRICACION"),
                    ORDEN = m.Field<int>("ORDEN"),
                    FE_ANUL = m.Field<DateTime?>("FE_ANUL"),
                    FE_CIERRE = m.Field<DateTime?>("FE_CIERRE"),
                }).ToList<Fabricacion>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<Fabricacion>();
            }
        }
    }
}
