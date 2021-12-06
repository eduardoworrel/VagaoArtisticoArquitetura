using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    [ApiController]
    public class TremController : ControllerBase
    {
        private readonly ILogger<TremController> _logger;
        private readonly TremContext _context;

        private readonly IConfiguration _conf;


        public TremController(ILogger<TremController> logger, IConfiguration conf, TremContext context)
        {
            _logger = logger;
            _context = context;
            _conf = conf;
        }
   
        [HttpGet("/trem")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Trem.ToListAsync());
        }

        [HttpGet("/trem/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var trem = await _context.Trem.Where(t => id == t.Id.ToString()).ToListAsync();
            return Ok(trem);
        }

        [HttpGet("/trem/exist/{id}")]
        public async Task<IActionResult> ExistById(int id)
        {
            bool trem = await _context.Trem.AnyAsync(t => id == t.Id);
            return Ok(trem);
        }

        [HttpDelete("/delete/trem/{id}")]
        public async Task<int> DeleteById(int id)
        {
            var trem = await _context.Trem.Where(t => id == t.Id).ToListAsync();
            Console.WriteLine(trem.First().Id);
            _context.Trem.RemoveRange(trem);
            return await _context.SaveChangesAsync();
        }

        [HttpPut("/update/trem/{id}")]
        public async Task<int> UpdateById(int id, string codigo)
        {
            Trem trem = await _context.Trem.Where(t => id == t.Id).FirstAsync();
            if (!String.IsNullOrEmpty(codigo))
                trem.Codigo = codigo;
            int response = await _context.SaveChangesAsync();
            return response;
        }
        [HttpPost("/create/trem")]
        public async Task<int> Create(string codigo)
        {
            if (!_context.Trem.Any(t => t.Codigo.Contains(codigo)))
            {
                var trem = new Trem { Codigo = codigo };
                await _context.Trem.AddAsync(trem);
                int response = await _context.SaveChangesAsync();
                return response;
            }
            else
            {
                return -1;
            }
        }
    }
}
