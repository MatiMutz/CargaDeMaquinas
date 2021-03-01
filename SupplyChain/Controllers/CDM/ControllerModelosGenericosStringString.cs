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
    public class ModelosGenericosStringStringController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelosGenericosStringStringController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{SQLcommandString}")]
        public IEnumerable<ModeloGenericoStringString> Get(string SQLcommandString)
        {
            try
            {
                List<ModeloGenericoStringString> xResultado = _context.ModelosGenericosStringString.FromSqlRaw(SQLcommandString).ToList<ModeloGenericoStringString>();
                return xResultado;
            }
            catch (Exception ex)
            {
                return new List<ModeloGenericoStringString>();
            }
        }
    }
}
