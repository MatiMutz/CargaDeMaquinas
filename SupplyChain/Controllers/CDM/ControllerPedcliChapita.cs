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
    public class PedcliChapitaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedcliChapitaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<PedCli> Get(string CG_ART)
        {
            try
            {
                string xSQL = string.Format("select LOTE from Pedcli where CG_ART = '{0}'", CG_ART);
                return _context.PedCli.FromSqlRaw(xSQL).ToList<PedCli>();
            }
            catch
            {
                return new List<PedCli>();
            }
        }
    }
}