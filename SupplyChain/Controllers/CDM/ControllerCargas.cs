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
                // Llena FECHA_PREVISTA_FABRICACION en casos que esté en null
                //_context.Database.ExecuteSqlRawAsync("EXEC NET_PCP_Carga_Poner_Fecha_Prevista_Fabricacion 0");

                // Llena tabla de carga
                List<ModeloCarga> dbCarga;
                dbCarga = _context.Cargas.FromSqlRaw("EXEC NET_PCP_Carga_Poner_Fecha_Prevista_Fabricacion 0; EXEC NET_PCP_Carga_Maq 1").ToList<ModeloCarga>();
                dbCarga = dbCarga.OrderBy(x => x.ORDEN_CELDA).ThenBy(x => x.CG_CELDA).ThenByDescending(x => x.CG_ESTADOCARGA).ThenBy(x => x.ORDEN).ThenBy(x => x.FECHA_PREVISTA_FABRICACION).ToList< ModeloCarga>();

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
                foreach (ModeloCarga xRowCarga in dbCarga)
                {
                    foreach (ModeloCarga xRowCargaColores in dbCarga)
                    {
                        // Busca si alguna de las ordenes asociadas tiene BACKGROUND, si lo tiene se lo asigna
                        if (xRowCarga.CG_ORDFASOC.ToString() == xRowCargaColores.CG_ORDFASOC.ToString() && xRowCargaColores.BACKGROUND.ToString() != "")
                        {
                            xRowCarga.BACKGROUND = xRowCargaColores.BACKGROUND;
                            break;
                        }
                    }
                    // Si todavia no tiene color, se le asigna uno del Vector de Colores
                    if (xRowCarga.BACKGROUND.ToString().Trim() == "")
                    {
                        xRowCarga.BACKGROUND = xArrayColores[xColor];
                        xColor++;
                        if (xColor == (xArrayColores.Length))
                        {
                            xColor = 0;
                        }
                    }
                }

                return dbCarga;
            }
            catch (Exception ex)
            {
                return new List<ModeloCarga>();
            }
        }
    }
}
