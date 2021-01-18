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
    public class SolutionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolutionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Solution> Get(string CAMPO)
        {
            try
            {
                string xSQL = string.Format("select CAMPO, VALORC from Solution where CAMPO = 'RUTAOF' OR CAMPO = 'RUTACNC' OR CAMPO = 'RUTAENSAYO' OR CAMPO = 'RUTATRAZABILIDAD'");
                return _context.Solution.FromSqlRaw(xSQL).ToList<Solution>();
            }
            catch
            {
                return new List<Solution>();
            }
        }
    }
}