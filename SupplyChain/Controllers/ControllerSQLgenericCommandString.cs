﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SupplyChain
{
    [Route("api/[controller]")]
    [ApiController]
    public class SQLgenericCommandStringController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SQLgenericCommandStringController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPut("{SQLcommandString}")]
        public async Task<IActionResult> Put(string SQLcommandString, object indistintoNOseUsa )
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(SQLcommandString);
            }
            catch (DbUpdateConcurrencyException)
            {
                BadRequest();
            }
            return Ok("Gurdado correctamente" + SQLcommandString);
        }
    }
}
