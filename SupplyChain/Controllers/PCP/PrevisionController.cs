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
    public class PrevisionController : ControllerBase
    {
        private string CadenaConexionSQL = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
        private readonly AppDbContext _context;

        public PrevisionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Prevision
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PresAnual>>> GetPrev()
        {

            try
            {
                return await _context.PresAnual.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Prevision/GetProd
        [HttpGet("GetProd")]
        public IEnumerable<Prod> GetProd(string PEDIDO)
        {
            string xSQL = string.Format("SELECT * FROM Prod ");
            return _context.Prod.FromSqlRaw(xSQL).ToList<Prod>();
        }

        // GET: api/Prevision/BuscarPorCG_PROD/{CG_PROD}
        [HttpGet("BuscarPorCG_PROD/{CG_PROD}")]
        public async Task<ActionResult<List<Prod>>> BuscarPorCG_PROD(string CG_PROD)
        {
            List<Prod> lDesProd = new List<Prod>();
            if (_context.Prod.Any())
            {
                lDesProd = await _context.Prod.Where(p => p.CG_PROD == CG_PROD).ToListAsync();
            }
            if (lDesProd == null)
            {
                return NotFound();
            }
            return lDesProd;
        }

        // GET: api/Prevision/BuscarPorDES_PROD/{DES_PROD}
        [HttpGet("BuscarPorDES_PROD/{DES_PROD}")]
        public async Task<ActionResult<List<Prod>>> BuscarPorDES_PROD(string DES_PROD)
        {
            List<Prod> lDesProd = new List<Prod>();
            if (_context.Prod.Any())
            {
                lDesProd = await _context.Prod.Where(p => p.DES_PROD == DES_PROD).ToListAsync();
            }
            if (lDesProd == null)
            {
                return NotFound();
            }
            return lDesProd;
        }

        // GET: api/Prevision/AgregarProdPrevision/{CG_PROD}/{DES_PROD}
        [HttpGet("BuscarProdPrevision/{CG_PROD}/{DES_PROD}/{Busqueda}")]
        public async Task<ActionResult<List<Prod>>> BuscarProdPrevision(string CG_PROD, string DES_PROD, int Busqueda)
        {
            List<Prod> lContiene = new List<Prod>();
            if (DES_PROD == "Vacio")
            {
                if (_context.Prod.Any())
                {
                    lContiene = await _context.Prod.Where(p => p.CG_PROD.Contains(CG_PROD)).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            else if (CG_PROD == "Vacio")
            {
                if (_context.Prod.Any())
                {
                    lContiene = await _context.Prod.Where(p => p.DES_PROD.Contains(DES_PROD)).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            else if (CG_PROD != "Vacio" && DES_PROD != "Vacio")
            {
                if (_context.Prod.Any())
                {
                    lContiene = await _context.Prod.Where(p => p.CG_PROD.Contains(CG_PROD) && p.DES_PROD.Contains(DES_PROD)).Take(Busqueda).ToListAsync();
                }
                if (lContiene == null)
                {
                    return NotFound();
                }
            }
            return lContiene;
        }

        // GET: api/Prevision/AgregarProdPrevision/{CG_PROD}/{DES_PROD}
        [HttpPut("AgregarProdPrevision")]
        public async Task<IActionResult> AgregarProdPrevision(PresAnual parametros)
        {
            try
            {
                string xFecha = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");

                // Averigua unidad
                ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                string xSQLCommandString = "SELECT Unid FROM Prod WHERE Cg_prod = '" + parametros.CG_ART.Trim() + "'";
                DataTable xTabla = xConexionSQL.EjecutarSQL(xSQLCommandString);
                string xUnidad = "";
                if (xTabla.Rows.Count > 0)
                {
                    xUnidad = xTabla.Rows[0]["Unid"].ToString();
                }
                else
                {
                    //MessageBox.Show("El Prod está vacío o es inexistente", "Previsión", MessageBoxButton.OK, MessageBoxImage.None);
                }
                string xCantidad = "1";

                // Inserta registro en PresAnual
                xConexionSQL = new ConexionSQL(CadenaConexionSQL);
                xConexionSQL.EjecutarSQLNonQuery("NET_PCP_PrevisionAgregar '" + parametros.CG_ART.Trim() + "', " +
                                                                          "'" + parametros.DES_ART.Trim() + "', " +
                                                                          "'" + xUnidad + "', " +
                                                                          " " + xCantidad + ", " +
                                                                          "'" + xFecha + "'");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }

        // GET: api/Prevision/AgregarProdPrevision/{CG_PROD}
        [HttpGet("AgregarProdPrevision/{CG_ART}")]
        public async Task<ActionResult<IEnumerable<PresAnual>>> AgregarProdPrevision(string CG_ART)
        {
            string xFecha = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");

            // Averigua unidad
            ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
            string xSQLCommandString = "SELECT Unid FROM Prod WHERE Cg_prod = '" + CG_ART.Trim() + "'";
            DataTable xTabla = xConexionSQL.EjecutarSQL(xSQLCommandString);
            string xUnidad = "";
            if (xTabla.Rows.Count > 0)
            {
                xUnidad = xTabla.Rows[0]["Unid"].ToString();
            }
            else
            {
                //MessageBox.Show("El Prod está vacío o es inexistente", "Previsión", MessageBoxButton.OK, MessageBoxImage.None);
            }
            string xCantidad = "1";

            xConexionSQL = new ConexionSQL(CadenaConexionSQL);
            xSQLCommandString = "SELECT Des_prod FROM Prod WHERE Cg_prod = '" + CG_ART.Trim() + "'";
            xTabla = xConexionSQL.EjecutarSQL(xSQLCommandString);
            string xDes_art = "";
            if (xTabla.Rows.Count > 0)
            {
                xDes_art = xTabla.Rows[0]["Des_prod"].ToString();
            }
            else
            {
                //MessageBox.Show("El Prod está vacío o es inexistente", "Previsión", MessageBoxButton.OK, MessageBoxImage.None);
            }
            // Inserta registro en PresAnual
            xConexionSQL = new ConexionSQL(CadenaConexionSQL);
            xConexionSQL.EjecutarSQLNonQuery("NET_PCP_PrevisionAgregar '" + CG_ART.Trim() + "', " +
                                                                      "'" + xDes_art + "', " +
                                                                      "'" + xUnidad + "', " +
                                                                      " " + xCantidad + ", " +
                                                                      "'" + xFecha + "'");
            return await _context.PresAnual.ToListAsync();
        }

        // GET: api/Prevision/BorrarPrevision/{REGISTRO}
        [HttpGet("BorrarPrevision/{REGISTRO}")]
        public async Task<ActionResult<IEnumerable<PresAnual>>> BorrarPrevision(int REGISTRO)
        {
            ConexionSQL xConexionSQL = new ConexionSQL(CadenaConexionSQL);
            xConexionSQL.EjecutarSQLNonQuery($"DELETE FROM Presanual WHERE Registro= {REGISTRO}");
            return await _context.PresAnual.ToListAsync();
        }

        // PUT: api/Prevision/PutPrev/{id}
        [HttpPut("PutPrev/{id}")]
        public async Task<IActionResult> PutPrev(int id, PresAnual prev)
        {
            if (id != prev.REGISTRO)
            {
                return BadRequest();
            }

            _context.Entry(prev).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrevExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool PrevExists(int id)
        {
            return _context.PresAnual.Any(e => e.REGISTRO == id);
        }
    }
}
