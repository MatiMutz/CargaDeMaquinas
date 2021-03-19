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
    public class PendientesFabricarController : ControllerBase
    {
        private string CadenaConexionSQL = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
        private DataTable dbPendFab;

        private readonly AppDbContext _context;

        public PendientesFabricarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PendientesFabricar
        [HttpGet]
        public List<ModeloPendientesFabricar> Get()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQL("EXEC NET_PCP_Pedidos");

                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = "SELECT REGISTRO, PEDIDO, CG_ART, DES_ART, PREVISION, CANTPED, CALCULADO, CANTEMITIR, LOPTIMO, STOCK, STOCKMIN, ";
                xSQLCommandString += "ISNULL((SELECT TOP 1 CG_FORM FROM FORM2 WHERE ACTIVO != 'N' AND CG_PROD = A.CG_ART), 0) AS CG_FORM, ";
                xSQLCommandString += "(case when (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=1) IS NULL then 0 else (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=1) end) AS STOCKENT, ";
                xSQLCommandString += "(case when (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=0) IS NULL then 0 else (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=0) end) AS COMP_EMITIDAS, ";
                xSQLCommandString += "(CASE WHEN A.EXIGEOA = 1 THEN 'Armado' ELSE 'Fabricación' END) AS EXIGEOA ";
                xSQLCommandString += "FROM Net_temp_pedidos A ORDER BY CG_ART";
                dbPendFab = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloPendientesFabricar> xLista = dbPendFab.AsEnumerable().Select(m => new ModeloPendientesFabricar()
                {
                    REGISTRO = m.Field<decimal>("REGISTRO"),
                    PEDIDO = m.Field<decimal>("PEDIDO"),
                    EXIGEOA = m.Field<string>("EXIGEOA"),
                    CG_ART = m.Field<string>("CG_ART"),
                    DES_ART = m.Field<string>("DES_ART"),
                    CG_FORM = m.Field<int?>("CG_FORM"),
                    PREVISION = m.Field<decimal>("PREVISION"),
                    CANTPED = m.Field<decimal>("CANTPED"),
                    CALCULADO = m.Field<decimal>("CALCULADO"),
                    CANTEMITIR = m.Field<decimal>("CANTEMITIR"),
                    LOPTIMO = m.Field<decimal>("LOPTIMO"),
                    STOCK = m.Field<decimal>("STOCK"),
                    STOCKMIN = m.Field<decimal>("STOCKMIN"),
                    STOCKENT = m.Field<decimal>("STOCKENT"),
                    COMP_EMITIDAS = m.Field<decimal>("COMP_EMITIDAS"),
                }).ToList();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloPendientesFabricar>();
            }
        }

        // PUT: api/PendientesFabricar/PutPenFab/{id}
        [HttpPut("PutPenFab/{id}")]
        public async Task<ActionResult<List<ModeloPendientesFabricar>>> PutPenFab(int id, ModeloPendientesFabricar prev)
        {
            if (id != prev.REGISTRO)
            {
                return BadRequest();
            }
            string xCg_form = prev.CG_FORM.ToString();
            if (xCg_form == "0")
            {
                return BadRequest();
            }
            else
            {
                string xRegistro = prev.REGISTRO.ToString();
                string xValor = prev.CANTEMITIR.ToString();
                // Reemplaza "," por "." para grabar en el SQL
                xValor = Convert.ToDouble(xValor.Replace(",", ".")).ToString();
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLcommandString = "UPDATE NET_TEMP_PEDIDOS SET CantEmitir = " + xValor + " WHERE Registro='" + xRegistro + "'";
                xConexionSQL.EjecutarSQLNonQuery(xSQLcommandString);
            }
            return NoContent();
        }

        private bool PenFabExists(int id)
        {
            return _context.ModeloPendientesFabricar.Any(e => e.REGISTRO == id);
        }

        // GET: api/EmitirOrdenes
        [HttpGet("EmitirOrdenes")]
        public List<ModeloPendientesFabricar> EmitirOrdenes()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);

                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQL("NET_PCP_EmitirOrdenes '" + "User" + "'");

                string xSQLCommandString = "SELECT REGISTRO, PEDIDO, CG_ART, DES_ART, PREVISION, CANTPED, CALCULADO, CANTEMITIR, LOPTIMO, STOCK, STOCKMIN, ";
                xSQLCommandString += "ISNULL((SELECT TOP 1 CG_FORM FROM FORM2 WHERE ACTIVO != 'N' AND CG_PROD = A.CG_ART), 0) AS CG_FORM, ";
                xSQLCommandString += "(case when (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=1) IS NULL then 0 else (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=1) end) AS STOCKENT, ";
                xSQLCommandString += "(case when (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=0) IS NULL then 0 else (SELECT SUM(CANT) FROM Programa B WHERE A.CG_ART=B.CG_PROD AND CG_ESTADOCARGA=0) end) AS COMP_EMITIDAS, ";
                xSQLCommandString += "(CASE WHEN A.EXIGEOA = 1 THEN 'Armado' ELSE 'Fabricación' END) AS EXIGEOA ";
                xSQLCommandString += "FROM Net_temp_pedidos A ORDER BY CG_ART";
                dbPendFab = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloPendientesFabricar> xLista = dbPendFab.AsEnumerable().Select(m => new ModeloPendientesFabricar()
                {
                    REGISTRO = m.Field<decimal>("REGISTRO"),
                    PEDIDO = m.Field<decimal>("PEDIDO"),
                    EXIGEOA = m.Field<string>("EXIGEOA"),
                    CG_ART = m.Field<string>("CG_ART"),
                    DES_ART = m.Field<string>("DES_ART"),
                    CG_FORM = m.Field<int?>("CG_FORM"),
                    PREVISION = m.Field<decimal>("PREVISION"),
                    CANTPED = m.Field<decimal>("CANTPED"),
                    CALCULADO = m.Field<decimal>("CALCULADO"),
                    CANTEMITIR = m.Field<decimal>("CANTEMITIR"),
                    LOPTIMO = m.Field<decimal>("LOPTIMO"),
                    STOCK = m.Field<decimal>("STOCK"),
                    STOCKMIN = m.Field<decimal>("STOCKMIN"),
                    STOCKENT = m.Field<decimal>("STOCKENT"),
                    COMP_EMITIDAS = m.Field<decimal>("COMP_EMITIDAS"),
                }).ToList<ModeloPendientesFabricar>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloPendientesFabricar>();
            }
        }
    }
}
