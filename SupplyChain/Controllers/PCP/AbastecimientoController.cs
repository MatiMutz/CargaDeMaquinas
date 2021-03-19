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
    public class AbastecimientoController : ControllerBase
    {
        private string CadenaConexionSQL = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
        private DataTable dbAbastecimientoMP;
        private DataTable dbAbastecimientoSE;

        private readonly AppDbContext _context;

        public AbastecimientoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Abastecimiento/AbastecimientoMP
        [HttpGet("AbastecimientoMP")]
        public List<ModeloAbastecimiento> AbastecimientoMP()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQL("EXEC NET_PCP_Abastecimiento");

                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = ("SELECT CG_MAT, DES_MAT, CALCULADO, ACOMPRAR, ACOMPRAR_INFORMADO, STOCK, UNIDMED, UNIDCOMER, STOCK_MINIMO, PEND_SIN_OC, COMP_DE_ENTRADA, COMP_DE_SALIDA" +
                                                            ", STOCK_CORREG, EN_PROCESO, REQUERIDO, ENTRPREV, * FROM NET_Temp_Abastecimiento WHERE CG_ORDEN = 4");
                dbAbastecimientoMP = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloAbastecimiento> xLista = dbAbastecimientoMP.AsEnumerable().Select(m => new ModeloAbastecimiento()
                {
                    CG_PROD = m.Field<string>("CG_PROD"),
                    CG_MAT = m.Field<string>("CG_MAT"),
                    DES_MAT = m.Field<string>("DES_MAT"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CALCULADO = m.Field<decimal?>("CALCULADO"),
                    ACOMPRAR = m.Field<decimal?>("ACOMPRAR"),
                    ACOMPRAR_INFORMADO = m.Field<decimal?>("ACOMPRAR_INFORMADO"),
                    STOCK = m.Field<decimal?>("STOCK"),
                    UNIDMED = m.Field<string>("UNIDMED"),
                    UNIDCOMER = m.Field<string>("UNIDCOMER"),
                    STOCK_MINIMO = m.Field<decimal?>("STOCK_MINIMO"),
                    PEND_SIN_OC = m.Field<decimal?>("PEND_SIN_OC"),
                    COMP_DE_ENTRADA = m.Field<decimal?>("COMP_DE_ENTRADA"),
                    COMP_DE_SALIDA = m.Field<decimal?>("COMP_DE_SALIDA"),
                    STOCK_CORREG = m.Field<decimal?>("STOCK_CORREG"),
                    EN_PROCESO = m.Field<decimal?>("EN_PROCESO"),
                    REQUERIDO = m.Field<decimal?>("REQUERIDO"),
                    ENTRPREV = m.Field<DateTime>("ENTRPREV"),
                    CG_CIA = m.Field<int>("CG_CIA"),
                    USUARIO = m.Field<string>("USUARIO"),
                }).ToList<ModeloAbastecimiento>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloAbastecimiento>();
            }
        }

        // GET: api/Abastecimiento/AbastecimientoMPX
        [HttpGet("AbastecimientoMPX")]
        public List<ModeloAbastecimiento> AbastecimientoMPX()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = ("SELECT CG_MAT, DES_MAT, CALCULADO, ACOMPRAR, ACOMPRAR_INFORMADO, STOCK, UNIDMED, UNIDCOMER, STOCK_MINIMO, PEND_SIN_OC, COMP_DE_ENTRADA, COMP_DE_SALIDA" +
                                                            ", STOCK_CORREG, EN_PROCESO, REQUERIDO, ENTRPREV, * FROM NET_Temp_Abastecimiento WHERE CG_ORDEN = 4");
                dbAbastecimientoMP = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloAbastecimiento> xLista = dbAbastecimientoMP.AsEnumerable().Select(m => new ModeloAbastecimiento()
                {
                    CG_PROD = m.Field<string>("CG_PROD"),
                    CG_MAT = m.Field<string>("CG_MAT"),
                    DES_MAT = m.Field<string>("DES_MAT"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CALCULADO = m.Field<decimal?>("CALCULADO"),
                    ACOMPRAR = m.Field<decimal?>("ACOMPRAR"),
                    ACOMPRAR_INFORMADO = m.Field<decimal?>("ACOMPRAR_INFORMADO"),
                    STOCK = m.Field<decimal?>("STOCK"),
                    UNIDMED = m.Field<string>("UNIDMED"),
                    UNIDCOMER = m.Field<string>("UNIDCOMER"),
                    STOCK_MINIMO = m.Field<decimal?>("STOCK_MINIMO"),
                    PEND_SIN_OC = m.Field<decimal?>("PEND_SIN_OC"),
                    COMP_DE_ENTRADA = m.Field<decimal?>("COMP_DE_ENTRADA"),
                    COMP_DE_SALIDA = m.Field<decimal?>("COMP_DE_SALIDA"),
                    STOCK_CORREG = m.Field<decimal?>("STOCK_CORREG"),
                    EN_PROCESO = m.Field<decimal?>("EN_PROCESO"),
                    REQUERIDO = m.Field<decimal?>("REQUERIDO"),
                    ENTRPREV = m.Field<DateTime>("ENTRPREV"),
                    CG_CIA = m.Field<int>("CG_CIA"),
                    USUARIO = m.Field<string>("USUARIO"),
                }).ToList<ModeloAbastecimiento>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloAbastecimiento>();
            }
        }

        // GET: api/Abastecimiento/AbastecimientoSE
        [HttpGet("AbastecimientoSE")]
        public List<ModeloAbastecimiento> AbastecimientoSE()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQL("EXEC NET_PCP_Abastecimiento");

                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = ("SELECT CG_MAT, DES_MAT, CALCULADO, ACOMPRAR, STOCK, UNIDMED, UNIDCOMER, STOCK_MINIMO, PEND_SIN_OC, COMP_DE_ENTRADA, COMP_DE_SALIDA" +
                                                        ", STOCK_CORREG, EN_PROCESO, REQUERIDO, ENTRPREV, * FROM NET_Temp_Abastecimiento WHERE CG_ORDEN = 3");
                dbAbastecimientoSE = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloAbastecimiento> xLista = dbAbastecimientoSE.AsEnumerable().Select(m => new ModeloAbastecimiento()
                {
                    CG_PROD = m.Field<string>("CG_PROD"),
                    CG_MAT = m.Field<string>("CG_MAT"),
                    DES_MAT = m.Field<string>("DES_MAT"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CALCULADO = m.Field<decimal?>("CALCULADO"),
                    ACOMPRAR = m.Field<decimal?>("ACOMPRAR"),
                    ACOMPRAR_INFORMADO = m.Field<decimal?>("ACOMPRAR_INFORMADO"),
                    STOCK = m.Field<decimal?>("STOCK"),
                    UNIDMED = m.Field<string>("UNIDMED"),
                    UNIDCOMER = m.Field<string>("UNIDCOMER"),
                    STOCK_MINIMO = m.Field<decimal?>("STOCK_MINIMO"),
                    PEND_SIN_OC = m.Field<decimal?>("PEND_SIN_OC"),
                    COMP_DE_ENTRADA = m.Field<decimal?>("COMP_DE_ENTRADA"),
                    COMP_DE_SALIDA = m.Field<decimal?>("COMP_DE_SALIDA"),
                    STOCK_CORREG = m.Field<decimal?>("STOCK_CORREG"),
                    EN_PROCESO = m.Field<decimal?>("EN_PROCESO"),
                    REQUERIDO = m.Field<decimal?>("REQUERIDO"),
                    ENTRPREV = m.Field<DateTime>("ENTRPREV"),
                    CG_CIA = m.Field<int>("CG_CIA"),
                    USUARIO = m.Field<string>("USUARIO"),
                }).ToList<ModeloAbastecimiento>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloAbastecimiento>();
            }
        }

        // GET: api/Abastecimiento/AbastecimientoSEX
        [HttpGet("AbastecimientoSEX")]
        public List<ModeloAbastecimiento> AbastecimientoSEX()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = ("SELECT CG_MAT, DES_MAT, CALCULADO, ACOMPRAR, STOCK, UNIDMED, UNIDCOMER, STOCK_MINIMO, PEND_SIN_OC, COMP_DE_ENTRADA, COMP_DE_SALIDA" +
                                                        ", STOCK_CORREG, EN_PROCESO, REQUERIDO, ENTRPREV, * FROM NET_Temp_Abastecimiento WHERE CG_ORDEN = 3");
                dbAbastecimientoSE = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloAbastecimiento> xLista = dbAbastecimientoSE.AsEnumerable().Select(m => new ModeloAbastecimiento()
                {
                    CG_PROD = m.Field<string>("CG_PROD"),
                    CG_MAT = m.Field<string>("CG_MAT"),
                    DES_MAT = m.Field<string>("DES_MAT"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CALCULADO = m.Field<decimal?>("CALCULADO"),
                    ACOMPRAR = m.Field<decimal?>("ACOMPRAR"),
                    ACOMPRAR_INFORMADO = m.Field<decimal?>("ACOMPRAR_INFORMADO"),
                    STOCK = m.Field<decimal?>("STOCK"),
                    UNIDMED = m.Field<string>("UNIDMED"),
                    UNIDCOMER = m.Field<string>("UNIDCOMER"),
                    STOCK_MINIMO = m.Field<decimal?>("STOCK_MINIMO"),
                    PEND_SIN_OC = m.Field<decimal?>("PEND_SIN_OC"),
                    COMP_DE_ENTRADA = m.Field<decimal?>("COMP_DE_ENTRADA"),
                    COMP_DE_SALIDA = m.Field<decimal?>("COMP_DE_SALIDA"),
                    STOCK_CORREG = m.Field<decimal?>("STOCK_CORREG"),
                    EN_PROCESO = m.Field<decimal?>("EN_PROCESO"),
                    REQUERIDO = m.Field<decimal?>("REQUERIDO"),
                    ENTRPREV = m.Field<DateTime>("ENTRPREV"),
                    CG_CIA = m.Field<int>("CG_CIA"),
                    USUARIO = m.Field<string>("USUARIO"),
                }).ToList<ModeloAbastecimiento>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloAbastecimiento>();
            }
        }

        // PUT: api/Abastecimiento/PutAbMP/{id}
        [HttpPut("PutAbMP/{id}")]
        public async Task<ActionResult<List<ModeloAbastecimiento>>> PutAbMP(string id, ModeloAbastecimiento Ab)
        {
            string xCg_mat = Ab.CG_MAT;
            string xValor = Ab.ACOMPRAR.ToString();
            // Reemplaza "," por "." para grabar en el SQL
            xValor = Convert.ToDouble(xValor.Replace(",", ".")).ToString();
            ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
            string xSQLcommandString = "UPDATE NET_Temp_Abastecimiento SET ACOMPRAR = " + xValor + " WHERE Cg_mat='" + xCg_mat + "'";
            xConexionSQL.EjecutarSQLNonQuery(xSQLcommandString);

            return NoContent();
        }

        // PUT: api/Abastecimiento/PutAbSE/{id}
        [HttpPut("PutAbSE/{id}")]
        public async Task<ActionResult<List<ModeloAbastecimiento>>> PutAbSE(string id, ModeloAbastecimiento Ab)
        {
            string xCg_mat = Ab.CG_MAT;
            string xValor = Ab.ACOMPRAR.ToString();
            // Reemplaza "," por "." para grabar en el SQL
            xValor = Convert.ToDouble(xValor.Replace(",", ".")).ToString();
            ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
            string xSQLcommandString = "UPDATE NET_Temp_Abastecimiento SET ACOMPRAR = " + xValor + " WHERE Cg_mat='" + xCg_mat + "'";
            xConexionSQL.EjecutarSQLNonQuery(xSQLcommandString);

            return NoContent();
        }

        // GET: api/Abastecimiento/AbastecerMP
        [HttpGet("AbastecerMP")]
        public List<ModeloAbastecimiento> AbastecerMP()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQL("EXEC NET_PCP_Abastecer_MP '" + "User" + "'");
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = ("SELECT CG_MAT, DES_MAT, CALCULADO, ACOMPRAR, ACOMPRAR_INFORMADO, STOCK, UNIDMED, UNIDCOMER, STOCK_MINIMO, PEND_SIN_OC, COMP_DE_ENTRADA, COMP_DE_SALIDA" +
                                                            ", STOCK_CORREG, EN_PROCESO, REQUERIDO, ENTRPREV, * FROM NET_Temp_Abastecimiento WHERE CG_ORDEN = 4");
                dbAbastecimientoMP = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloAbastecimiento> xLista = dbAbastecimientoMP.AsEnumerable().Select(m => new ModeloAbastecimiento()
                {
                    CG_PROD = m.Field<string>("CG_PROD"),
                    CG_MAT = m.Field<string>("CG_MAT"),
                    DES_MAT = m.Field<string>("DES_MAT"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CALCULADO = m.Field<decimal?>("CALCULADO"),
                    ACOMPRAR = m.Field<decimal?>("ACOMPRAR"),
                    ACOMPRAR_INFORMADO = m.Field<decimal?>("ACOMPRAR_INFORMADO"),
                    STOCK = m.Field<decimal?>("STOCK"),
                    UNIDMED = m.Field<string>("UNIDMED"),
                    UNIDCOMER = m.Field<string>("UNIDCOMER"),
                    STOCK_MINIMO = m.Field<decimal?>("STOCK_MINIMO"),
                    PEND_SIN_OC = m.Field<decimal?>("PEND_SIN_OC"),
                    COMP_DE_ENTRADA = m.Field<decimal?>("COMP_DE_ENTRADA"),
                    COMP_DE_SALIDA = m.Field<decimal?>("COMP_DE_SALIDA"),
                    STOCK_CORREG = m.Field<decimal?>("STOCK_CORREG"),
                    EN_PROCESO = m.Field<decimal?>("EN_PROCESO"),
                    REQUERIDO = m.Field<decimal?>("REQUERIDO"),
                    ENTRPREV = m.Field<DateTime>("ENTRPREV"),
                    CG_CIA = m.Field<int>("CG_CIA"),
                    USUARIO = m.Field<string>("USUARIO"),
                }).ToList<ModeloAbastecimiento>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloAbastecimiento>();
            }
        }

        // GET: api/Abastecimiento/AbastecerSE
        [HttpGet("AbastecerSE")]
        public List<ModeloAbastecimiento> AbastecerSE()
        {
            try
            {
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQL("EXEC NET_PCP_Abastecer_SE '" + "User" + "'");
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = ("SELECT CG_MAT, DES_MAT, CALCULADO, ACOMPRAR, STOCK, UNIDMED, UNIDCOMER, STOCK_MINIMO, PEND_SIN_OC, COMP_DE_ENTRADA, COMP_DE_SALIDA" +
                                                        ", STOCK_CORREG, EN_PROCESO, REQUERIDO, ENTRPREV, * FROM NET_Temp_Abastecimiento WHERE CG_ORDEN = 3");
                dbAbastecimientoSE = xConexionSQL.EjecutarSQL(xSQLCommandString);

                List<ModeloAbastecimiento> xLista = dbAbastecimientoSE.AsEnumerable().Select(m => new ModeloAbastecimiento()
                {
                    CG_PROD = m.Field<string>("CG_PROD"),
                    CG_MAT = m.Field<string>("CG_MAT"),
                    DES_MAT = m.Field<string>("DES_MAT"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CALCULADO = m.Field<decimal?>("CALCULADO"),
                    ACOMPRAR = m.Field<decimal?>("ACOMPRAR"),
                    ACOMPRAR_INFORMADO = m.Field<decimal?>("ACOMPRAR_INFORMADO"),
                    STOCK = m.Field<decimal?>("STOCK"),
                    UNIDMED = m.Field<string>("UNIDMED"),
                    UNIDCOMER = m.Field<string>("UNIDCOMER"),
                    STOCK_MINIMO = m.Field<decimal?>("STOCK_MINIMO"),
                    PEND_SIN_OC = m.Field<decimal?>("PEND_SIN_OC"),
                    COMP_DE_ENTRADA = m.Field<decimal?>("COMP_DE_ENTRADA"),
                    COMP_DE_SALIDA = m.Field<decimal?>("COMP_DE_SALIDA"),
                    STOCK_CORREG = m.Field<decimal?>("STOCK_CORREG"),
                    EN_PROCESO = m.Field<decimal?>("EN_PROCESO"),
                    REQUERIDO = m.Field<decimal?>("REQUERIDO"),
                    ENTRPREV = m.Field<DateTime>("ENTRPREV"),
                    CG_CIA = m.Field<int>("CG_CIA"),
                    USUARIO = m.Field<string>("USUARIO"),
                }).ToList<ModeloAbastecimiento>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloAbastecimiento>();
            }
        }
    }
}
