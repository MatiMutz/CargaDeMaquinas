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
    public class CargasController : ControllerBase
    {
        private string CadenaConexionSQL = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
        private int CantidadColumnasPorPeriodo = 8;
        private int xExtensionColumnas;
        private DataTable dbCarga;
        private int xAnchoColumna = 30;
        private DateTime xFechaInicial;

        private readonly AppDbContext _context;

        public CargasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cargas
        [HttpGet]
        public IEnumerable<ModeloCarga> Get()
        {
            try
            {
                // Busca turno
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                DataTable xTabla = xConexionSQL.EjecutarSQL("Select Top 1 ValorN From Solution Where Campo = 'HORASDIA'");
                if (xTabla.Rows.Count > 0)
                {
                    CantidadColumnasPorPeriodo = Convert.ToInt16(xTabla.Rows[0]["ValorN"]);
                }
                // Busca dias de calendario
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xTabla = xConexionSQL.EjecutarSQL("Select Top 1 ValorN From Solution Where Campo = 'DIASCARGA'");
                if (xTabla.Rows.Count > 0)
                {
                    xExtensionColumnas = Convert.ToInt16(xTabla.Rows[0]["ValorN"]);
                }

                int xAvanceAutomatico = 0;
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xTabla = xConexionSQL.EjecutarSQL("SELECT ValorN FROM solution WHERE Campo = 'CARGA_AVANCE_AUTOMATICO'");
                if (xTabla.Rows.Count > 0)
                {
                    xAvanceAutomatico = Convert.ToInt32(((DataRow)xTabla.Rows[0])["ValorN"]);
                }

                // Llena FECHA_PREVISTA_FABRICACION en casos que esté en null
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQLNonQuery("EXEC NET_PCP_Carga_Poner_Fecha_Prevista_Fabricacion 0");

                // Llena tabla de carga
                String xSQLSelect = "SELECT B.CG_ORDEN, A.CG_ESTADOCARGA, A.CG_ORDF, CG_ORDFORIG, CG_ORDFASOC, LTRIM(RTRIM(A.CG_CELDA)) AS CG_CELDA, C.DES_CELDA, " +
                                    "A.CG_PROD, A.DES_PROD, isnull(rTrim(A.PROCESO) + ' - ' + Protab.DESCRIP, '') AS PROCESO, isnull(PEDCLI.CG_CLI, 0) AS CG_CLI, " +
                                    "isnull(PEDCLI.DES_CLI, '') AS DES_CLI, FECHA_PREVISTA_FABRICACION, CONVERT(VARCHAR, CONVERT(TIME(0), FECHA_PREVISTA_FABRICACION)) AS HORA, " +
                                    "(A.DIASFAB * 24) HORASFAB, (select max(cg_ordf) from programa where  CG_ORDFASOC = A.CG_ORDFASOC) ULTIMAORDENASOCIADA, " +
                                    "(A.DIASFAB * 1440) MINUTOSFAB, A.FE_ENTREGA, A.DIASFAB, A.CANT, A.CANTFAB, (A.CANTFAB * 100 / A.CANT) AS AVANCE, '' AS BACKGROUND, " +
                                    "(CASE WHEN B.CG_ORDEN=1 THEN 'Producto' ELSE (CASE WHEN B.CG_ORDEN=2 THEN 'Semi-Elaborado de Proceso' ELSE (CASE WHEN B.CG_ORDEN=3 THEN 'Semi-Elaborado' ELSE (CASE WHEN B.CG_ORDEN=4 THEN 'Materia Prima' ELSE (CASE WHEN B.CG_ORDEN=10 THEN 'Insumo No Productivo / Articulo de Reventa' ELSE (CASE WHEN B.CG_ORDEN=11 THEN 'Herramental e Insumos Inventariables' ELSE (CASE WHEN B.CG_ORDEN=12 THEN 'Repuestos' ELSE (CASE WHEN B.CG_ORDEN=13 THEN 'Servicios' ELSE '' END) END) END) END) END) END) END) END) AS CLASE, " +
                                    "A.INSUMOS_ENTREGADOS_A_PLANTA, 0 AS COLUMNA, 0 AS COLUMNSPAN, 0 AS FLAG_DEPENDENCIAS, ISNULL(PROD.EXIGEOA, 0) EXIGEOA, isnull(A.PEDIDO, 0) PEDIDO " +
                                    "FROM Prod B, Celdas C, Programa A " +
                                    "LEFT JOIN ProTab ON ProTab.PROCESO = A.PROCESO " +
                                    "LEFT JOIN PEDCLI ON PEDCLI.PEDIDO = A.PEDIDO " +
                                    "LEFT JOIN PROD ON PROD.CG_PROD = A.CG_PROD " +
                                    "WHERE A.CG_PROD=B.CG_PROD AND LTRIM(RTRIM(A.CG_CELDA))=LTRIM(RTRIM(C.CG_CELDA)) " +
                                    "AND FECHA_PREVISTA_FABRICACION IS NOT NULL " +
                                    "AND LTRIM(RTRIM(A.CG_CELDA)) != '' AND A.DIASFAB > 0 AND A.CG_ESTADOCARGA = 3 " +
                                    "ORDER BY A.CG_ORDFASOC, A.CG_ORDFORIG";
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                DataTable dbCargaTemp = xConexionSQL.EjecutarSQL(xSQLSelect);
                DateTime xFecha;
                if (dbCargaTemp.Rows.Count == 0)
                {
                    return new List<ModeloCarga>();
                }

                // Ordena fechas correlativamente de las ordenes dependientes
                int i = 0;
                int xOrdAsoc;
                while (i < dbCargaTemp.Rows.Count)
                {
                    xFecha = Convert.ToDateTime(dbCargaTemp.Rows[i]["FECHA_PREVISTA_FABRICACION"]);
                    xOrdAsoc = Convert.ToInt32(dbCargaTemp.Rows[i]["CG_ORDFASOC"]);
                    while (i < dbCargaTemp.Rows.Count && xOrdAsoc == Convert.ToInt32(dbCargaTemp.Rows[i]["CG_ORDFASOC"]))
                    {
                        if (Convert.ToInt32(dbCargaTemp.Rows[i]["CG_ORDFORIG"]) > 0)
                        {
                            xFecha = xFecha.AddHours(1);
                            dbCargaTemp.Rows[i]["FECHA_PREVISTA_FABRICACION"] = xFecha;
                        }
                        xFecha = Convert.ToDateTime(dbCargaTemp.Rows[i]["FECHA_PREVISTA_FABRICACION"]);
                        i++;
                    }
                }

                // Ordena Celdas por CG_CELDA + CG_ESTADOCARGA DESC + FECHA_PREVISTA_FABRICACION
                try
                {
                    dbCarga = dbCargaTemp.AsEnumerable()
                                .OrderBy(r => r.Field<string>("CG_CELDA"))
                                .ThenByDescending(r => r.Field<int>("CG_ESTADOCARGA"))
                                .ThenBy(r => r.Field<DateTime>("FECHA_PREVISTA_FABRICACION"))
                                .CopyToDataTable();
                }
                catch (Exception)
                {
                    return null;
                }

                xFechaInicial = DateTime.Now.Date;
                foreach (DataRow xRowCarga in dbCarga.Rows)
                {
                    if (Convert.ToDateTime(xRowCarga["FECHA_PREVISTA_FABRICACION"]) < xFechaInicial)
                    {
                        xFechaInicial = Convert.ToDateTime(xRowCarga["FECHA_PREVISTA_FABRICACION"]);
                    }
                }

                // Llena tabla de Festivos
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                DataTable dbFestivos = xConexionSQL.EjecutarSQL("SELECT Fecha FROM CalendarioFestivos ORDER BY Fecha");

                // Arma vector con colores para las barras
                string[] xArrayColores = new string[138] {
                Color.AliceBlue.Name,
                Color.AntiqueWhite.Name,
                Color.Aqua.Name,
                Color.Aquamarine.Name,
                Color.Azure.Name,
                Color.Beige.Name,
                Color.Bisque.Name,
                Color.Black.Name,
                Color.BlanchedAlmond.Name,
                Color.Blue.Name,
                Color.BlueViolet.Name,
                Color.Brown.Name,
                Color.BurlyWood.Name,
                Color.Chartreuse.Name,
                Color.Chocolate.Name,
                Color.Coral.Name,
                Color.CornflowerBlue.Name,
                Color.Cornsilk.Name,
                Color.Crimson.Name,
                Color.Cyan.Name,
                Color.DarkBlue.Name,
                Color.DarkCyan.Name,
                Color.DarkGoldenrod.Name,
                Color.DarkGray.Name,
                Color.DarkGreen.Name,
                Color.DarkKhaki.Name,
                Color.DarkMagenta.Name,
                Color.DarkOliveGreen.Name,
                Color.DarkOrange.Name,
                Color.DarkOrchid.Name,
                Color.DarkRed.Name,
                Color.DarkSalmon.Name,
                Color.DarkSeaGreen.Name,
                Color.DarkSlateBlue.Name,
                Color.DarkSlateGray.Name,
                Color.DarkTurquoise.Name,
                Color.DarkViolet.Name,
                Color.DeepPink.Name,
                Color.DeepSkyBlue.Name,
                Color.DimGray.Name,
                Color.DodgerBlue.Name,
                Color.Firebrick.Name,
                Color.FloralWhite.Name,
                Color.ForestGreen.Name,
                Color.Fuchsia.Name,
                Color.Gainsboro.Name,
                Color.GhostWhite.Name,
                Color.Gold.Name,
                Color.Goldenrod.Name,
                Color.Gray.Name,
                Color.Green.Name,
                Color.GreenYellow.Name,
                Color.Honeydew.Name,
                Color.HotPink.Name,
                Color.IndianRed.Name,
                Color.Indigo.Name,
                Color.Ivory.Name,
                Color.Khaki.Name,
                Color.Lavender.Name,
                Color.LavenderBlush.Name,
                Color.LawnGreen.Name,
                Color.LemonChiffon.Name,
                Color.LightBlue.Name,
                Color.LightCoral.Name,
                Color.LightCyan.Name,
                Color.LightGoldenrodYellow.Name,
                Color.LightGreen.Name,
                Color.LightGray.Name,
                Color.LightPink.Name,
                Color.LightSalmon.Name,
                Color.LightSeaGreen.Name,
                Color.LightSkyBlue.Name,
                Color.LightSlateGray.Name,
                Color.LightSteelBlue.Name,
                Color.LightYellow.Name,
                Color.Lime.Name,
                Color.LimeGreen.Name,
                Color.Linen.Name,
                Color.Magenta.Name,
                Color.Maroon.Name,
                Color.MediumAquamarine.Name,
                Color.MediumBlue.Name,
                Color.MediumOrchid.Name,
                Color.MediumPurple.Name,
                Color.MediumSeaGreen.Name,
                Color.MediumSlateBlue.Name,
                Color.MediumSpringGreen.Name,
                Color.MediumTurquoise.Name,
                Color.MediumVioletRed.Name,
                Color.MidnightBlue.Name,
                Color.MintCream.Name,
                Color.MistyRose.Name,
                Color.Moccasin.Name,
                Color.NavajoWhite.Name,
                Color.Navy.Name,
                Color.OldLace.Name,
                Color.Olive.Name,
                Color.OliveDrab.Name,
                Color.Orange.Name,
                Color.OrangeRed.Name,
                Color.Orchid.Name,
                Color.PaleGoldenrod.Name,
                Color.PaleGreen.Name,
                Color.PaleTurquoise.Name,
                Color.PaleVioletRed.Name,
                Color.PapayaWhip.Name,
                Color.PeachPuff.Name,
                Color.Peru.Name,
                Color.Pink.Name,
                Color.Plum.Name,
                Color.PowderBlue.Name,
                Color.Purple.Name,
                Color.Red.Name,
                Color.RosyBrown.Name,
                Color.RoyalBlue.Name,
                Color.SaddleBrown.Name,
                Color.Salmon.Name,
                Color.SandyBrown.Name,
                Color.SeaGreen.Name,
                Color.SeaShell.Name,
                Color.Sienna.Name,
                Color.Silver.Name,
                Color.SkyBlue.Name,
                Color.SlateBlue.Name,
                Color.SlateGray.Name,
                Color.Snow.Name,
                Color.SpringGreen.Name,
                Color.SteelBlue.Name,
                Color.Tan.Name,
                Color.Teal.Name,
                Color.Thistle.Name,
                Color.Tomato.Name,
                Color.Turquoise.Name,
                Color.Violet.Name,
                Color.Wheat.Name,
                Color.WhiteSmoke.Name,
                Color.Yellow.Name,
                Color.YellowGreen.Name
            };

                // Carga colores en dbCarga
                int xColor = 0;
                foreach (DataRow xRowCarga in dbCarga.Rows)
                {
                    foreach (DataRow xRowCargaColores in dbCarga.Rows)
                    {
                        // Busca si alguna de las ordenes asociadas tiene BACKGROUND, si lo tiene se lo asigna
                        if (xRowCarga["CG_ORDFASOC"].ToString() == xRowCargaColores["CG_ORDFASOC"].ToString() && xRowCargaColores["BACKGROUND"].ToString() != "")
                        {
                            xRowCarga["BACKGROUND"] = xRowCargaColores["BACKGROUND"];
                            break;
                        }
                    }
                    // Si todavia no tiene color, se le asigna uno del Vector de Colores
                    if (xRowCarga["BACKGROUND"].ToString().Trim() == "")
                    {
                        xRowCarga["BACKGROUND"] = xArrayColores[xColor];
                        xColor++;
                        if (xColor == (xArrayColores.Length))
                        {
                            xColor = 0;
                        }
                    }
                }

                // Busca Columnas para las ordenes de fabricacion
                int xColumnSpan = 0;
                int xColumnaInicialBarra = 0;
                int xHorasPasadas;
                int xDiasPasados;
                decimal xDiasFab;
                string xCg_celda = "";
                i = 0;
                while (i < dbCarga.Rows.Count)
                {
                    // recorre en el while mientras sea la misma celda
                    xFecha = xFechaInicial;
                    xColumnaInicialBarra = 0;
                    xCg_celda = dbCarga.Rows[i]["CG_CELDA"].ToString().Trim();
                    while (i < dbCarga.Rows.Count && xCg_celda == dbCarga.Rows[i]["CG_CELDA"].ToString().Trim())
                    {
                        // ColumnSpan
                        xHorasPasadas = 0;
                        if (xAvanceAutomatico == 1)
                        {
                            if (dbCarga.Rows[i]["CG_ESTADOCARGA"].ToString() == "3")
                            {
                                xDiasPasados = ((TimeSpan)(DateTime.Now - Convert.ToDateTime(dbCarga.Rows[i]["FECHA_PREVISTA_FABRICACION"]))).Days;
                                if (xDiasPasados > 0)
                                {
                                    xDiasFab = (Convert.ToDecimal(dbCarga.Rows[i]["DIASFAB"]) - xDiasPasados);
                                }
                                else
                                {
                                    xHorasPasadas = ((DateTime.Now.Hour - Convert.ToDateTime(dbCarga.Rows[i]["FECHA_PREVISTA_FABRICACION"]).Hour));
                                    xDiasFab = Convert.ToDecimal(dbCarga.Rows[i]["DIASFAB"]);
                                }
                            }
                            else
                            {
                                xDiasFab = Convert.ToDecimal(dbCarga.Rows[i]["DIASFAB"]);
                            }
                        }
                        else
                        {
                            if (Convert.ToDecimal(dbCarga.Rows[i]["AVANCE"]) > 0)
                            {
                                xDiasFab = Convert.ToDecimal(dbCarga.Rows[i]["DIASFAB"]) -
                                          (Convert.ToDecimal(dbCarga.Rows[i]["DIASFAB"]) * Convert.ToDecimal(dbCarga.Rows[i]["AVANCE"]) / 100);
                            }
                            else
                            {
                                xDiasFab = Convert.ToDecimal(dbCarga.Rows[i]["DIASFAB"]);
                            }
                        }
                        if (xDiasFab <= 0)
                        {
                            xColumnSpan = 1;
                        }
                        else
                        {
                            xColumnSpan = Convert.ToInt32(xDiasFab * (Decimal)CantidadColumnasPorPeriodo);
                            if (xHorasPasadas > 0)
                            {
                                xColumnSpan -= xHorasPasadas;
                            }
                        }
                        if (xColumnSpan <= 0)
                        {
                            xColumnSpan = 1;
                        }
                        dbCarga.Rows[i]["COLUMNA"] = 2 + xColumnaInicialBarra;
                        dbCarga.Rows[i]["COLUMNSPAN"] = xColumnSpan;
                        xColumnaInicialBarra = xColumnaInicialBarra + xColumnSpan;
                        i++;
                    }
                }

                bool xHayCambios1 = true;
                bool xHayCambios2 = true;
                while (xHayCambios1 == true && xHayCambios2 == true)
                {
                    // Corrige Columnas de Barras Segun Dependencias
                    xHayCambios1 = CorrigeColumnasSegunDependencias();

                    // Corrige Columnas de Barras celda por celda, puede haber superposiciones
                    xHayCambios2 = CorrigeColumnasPorCelda();
                }

                // Retorno
                List<ModeloCarga> xLista = dbCarga.AsEnumerable().Select(m => new ModeloCarga()
                {
                    CG_ORDF = m.Field<int>("CG_ORDF"),
                    CG_ORDEN = m.Field<int>("CG_ORDEN"),
                    CG_ESTADOCARGA = m.Field<int>("CG_ESTADOCARGA"),
                    CG_ORDFORIG = m.Field<int>("CG_ORDFORIG"),
                    CG_ORDFASOC = m.Field<int>("CG_ORDFASOC"),
                    CG_CELDA = m.Field<string>("CG_CELDA"),
                    DES_CELDA = m.Field<string>("DES_CELDA"),
                    CG_PROD = m.Field<string>("CG_PROD"),
                    DES_PROD = m.Field<string>("DES_PROD"),
                    PROCESO = m.Field<string>("PROCESO"),
                    CG_CLI = m.Field<int>("CG_CLI"),
                    DES_CLI = m.Field<string>("DES_CLI"),
                    FECHA_PREVISTA_FABRICACION = m.Field<DateTime>("FECHA_PREVISTA_FABRICACION"),
                    HORA = m.Field<string>("HORA"),
                    HORASFAB = m.Field<Decimal>("HORASFAB"),
                    MINUTOSFAB = m.Field<Decimal>("MINUTOSFAB"),
                    FE_ENTREGA = m.Field<DateTime>("FE_ENTREGA"),
                    DIASFAB = m.Field<Decimal>("DIASFAB"),
                    CANT = m.Field<Decimal>("CANT"),
                    CANTFAB = m.Field<Decimal>("CANTFAB"),
                    AVANCE = m.Field<Decimal>("AVANCE"),
                    BACKGROUND = m.Field<string>("BACKGROUND"),
                    CLASE = m.Field<string>("CLASE"),
                    INSUMOS_ENTREGADOS_A_PLANTA = m.Field<bool>("INSUMOS_ENTREGADOS_A_PLANTA"),
                    COLUMNA = m.Field<int>("COLUMNA"),
                    COLUMNSPAN = m.Field<int>("COLUMNSPAN"),
                    FLAG_DEPENDENCIAS = m.Field<int>("FLAG_DEPENDENCIAS"),
                    EXIGEOA = m.Field<bool>("EXIGEOA"),
                    PEDIDO = m.Field<int>("PEDIDO"),
                }).ToList<ModeloCarga>();

                return xLista;
            }
            catch (Exception ex)
            {
                return new List<ModeloCarga>();
            }
        }

        private bool CorrigeColumnasSegunDependencias()
        {
            // Corrige Columnas de Barras segun dependencias
            DataRow[] xDataRows;
            DataTable xTablaDependencias;
            int z;
            Int32 xColumnaInicial;
            int i;
            bool xHayCambios = false;
            // PRIMERO PONGO EL FLAG EN 0 PARA EVITAR QUE SE CORRIJAN DEPENDENCIAS POR 2DA VEZ
            i = 0;
            while (i < dbCarga.Rows.Count)
            {
                dbCarga.Rows[i]["FLAG_DEPENDENCIAS"] = 0;
                i++;
            }
            dbCarga.AcceptChanges();
            i = 0;
            while (i < dbCarga.Rows.Count)
            {
                if (Convert.ToInt32(dbCarga.Rows[i]["FLAG_DEPENDENCIAS"]) == 0)
                {
                    if (Convert.ToInt32(dbCarga.Rows[i]["CG_ORDFORIG"]) > 0)
                    {
                        // Corrige Columna de cada orden de cada Celda segun dependencias
                        xDataRows = dbCarga.Select("CG_ORDFASOC = " + dbCarga.Rows[i]["CG_ORDFASOC"].ToString());
                        if (xDataRows.Length > 1)
                        {
                            xTablaDependencias = xDataRows.CopyToDataTable();
                            xTablaDependencias.DefaultView.Sort = "CG_ORDFORIG";
                            xTablaDependencias = xTablaDependencias.DefaultView.ToTable();
                            z = 0;
                            xColumnaInicial = 0;
                            while (z < xTablaDependencias.Rows.Count)
                            {
                                if (Convert.ToInt32(xTablaDependencias.Rows[z]["CG_ORDFORIG"]) > 0)
                                {
                                    if (Convert.ToInt32(dbCarga.Rows[i]["COLUMNA"]) < (xColumnaInicial) && dbCarga.Rows[i]["CG_ORDF"].ToString() == xTablaDependencias.Rows[z]["CG_ORDF"].ToString())
                                    {
                                        dbCarga.Rows[i]["COLUMNA"] = xColumnaInicial;
                                        dbCarga.Rows[i]["FLAG_DEPENDENCIAS"] = 1;
                                        xHayCambios = true;
                                        break;
                                    }
                                }
                                xColumnaInicial = Convert.ToInt32(xTablaDependencias.Rows[z]["COLUMNA"]) + Convert.ToInt32(xTablaDependencias.Rows[z]["COLUMNSPAN"]);
                                z++;
                            }
                        }
                    }
                }
                i++;
            }
            dbCarga.AcceptChanges();
            return xHayCambios;
        }
        private bool CorrigeColumnasPorCelda()
        {
            // Corrige Columnas de Barras celda por celda, puede haber superposiciones
            bool xHayCambios = false;
            string xCg_celda = "";
            int xColumnaInicial;
            int i = 0;
            while (i < dbCarga.Rows.Count)
            {
                // recorre en el while mientras sea la misma celda
                xColumnaInicial = 0;
                xCg_celda = dbCarga.Rows[i]["CG_CELDA"].ToString().Trim();
                while (i < dbCarga.Rows.Count && xCg_celda == dbCarga.Rows[i]["CG_CELDA"].ToString().Trim())
                {
                    if (Convert.ToInt32(dbCarga.Rows[i]["COLUMNA"]) < xColumnaInicial)
                    {
                        dbCarga.Rows[i]["COLUMNA"] = xColumnaInicial;
                        xHayCambios = true;
                    }
                    xColumnaInicial = Convert.ToInt32(dbCarga.Rows[i]["COLUMNA"]) + Convert.ToInt32(dbCarga.Rows[i]["COLUMNSPAN"]);
                    i++;
                }
            }
            dbCarga.AcceptChanges();
            return xHayCambios;
        }
    }
}
