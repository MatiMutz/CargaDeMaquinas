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
    public class ModelosGenericosIntStringController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelosGenericosIntStringController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{SQLcommandString}")]
        public IEnumerable<ModeloGenericoIntString> Get(string SQLcommandString)
        {
            try
            {
                List<ModeloGenericoIntString> xResultado = _context.ModelosGenericosIntString.FromSqlRaw(SQLcommandString).ToList<ModeloGenericoIntString>();
                return xResultado;
            }
            catch
            {
                return new List<ModeloGenericoIntString>();
            }
        }
    }
}
