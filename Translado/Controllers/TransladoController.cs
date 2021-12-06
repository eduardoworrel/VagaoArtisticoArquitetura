using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class TransladoController : ControllerBase
    {
        private readonly ILogger<TransladoController> _logger;
        private readonly TransladoContext _context;
        private readonly IConfiguration _conf;

        public TransladoController(ILogger<TransladoController> logger, IConfiguration conf, TransladoContext context)
        {
            _logger = logger;
            _context = context;
            _conf = conf;
        }


        [HttpGet("/translado")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Translado.ToListAsync());
        }

        [HttpGet("/translado/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var Translado = await _context.Translado.Where(t => id == t.Id.ToString()).ToListAsync();
            return Ok(Translado);
        }

        [HttpGet("/translado/exist/{id}")]
        public async Task<IActionResult> ExistById(int id)
        {
            bool translado = await _context.Translado.AnyAsync(a => id == a.Id);
            return Ok(translado);
        }

        [HttpGet("/Translado/range/{startDatetime}/{endDatetime}")]
        public async Task<IActionResult> GetByRangeDate(DateTime startDatetime, DateTime endDatetime)
        {
            var translado = await _context.Translado.Where(t => t.HoraPartida >= startDatetime && t.HoraChegada <= endDatetime).ToListAsync();
            return Ok(translado);
        }

        [HttpDelete("/delete/translado/{id}")]
        public async Task<int> DeleteById(int id)
        {
            var Translado = await _context.Translado.Where(t => id == t.Id).ToListAsync();
            _context.Translado.RemoveRange(Translado);
            return await _context.SaveChangesAsync();
        }
        [HttpPut("/update/translado/{id}")]
        public async Task<int> UpdateById(int id, int idTrem, string estacaoPartida, string horaPartida, string estacaoChegada, string horaChegada, int status)
        {
            DateTime horaC = DateTime.Parse(horaChegada);
            DateTime horaP = DateTime.Parse(horaPartida);
            Translado Translado = await _context.Translado.Where(a => id == a.Id).FirstAsync();
            if (!string.IsNullOrEmpty(idTrem.ToString()))
            {
                var http = new HttpClient();
                var data = http.GetAsync("http://trem:5001/trem/exist/" + idTrem.ToString())
                .Result.Content.ReadAsStringAsync().Result;
                if (data != "true")
                    return -1;
                Translado.IdTrem = idTrem;
            }
            if (!string.IsNullOrEmpty(status.ToString()))
                Translado.Status = status;
            if (!String.IsNullOrEmpty(estacaoPartida))
                Translado.EstacaoPartida = estacaoPartida;
            if (!String.IsNullOrEmpty(horaPartida))
                Translado.HoraPartida = horaP;
            if (!String.IsNullOrEmpty(horaChegada))
                Translado.HoraChegada = horaC;
            if (!String.IsNullOrEmpty(estacaoChegada))
                Translado.EstacaoChegada = estacaoChegada;
            int response = await _context.SaveChangesAsync();
            return response;
        }
        [HttpPost("/create/translado")]
        public async Task<int> Create(int idTrem, string estacaoPartida, DateTime horaP, string estacaoChegada, DateTime horaC)
        {
            try
            {
                var http = new HttpClient();
                var data = http.GetAsync("http://trem:5001/trem/exist/" + idTrem.ToString()).Result.Content.ReadAsStringAsync().Result;
                if (data != "true")
                    return -2;

            }
            catch (Exception requestException)
            {
                Console.WriteLine(requestException.Message);
            }


            if (!_context.Translado.Any(t => t.IdTrem == idTrem && t.HoraPartida == horaP))
            {
                var Translado = new Translado { IdTrem = idTrem, EstacaoPartida = estacaoPartida, HoraPartida = horaP, EstacaoChegada = estacaoChegada, HoraChegada = horaC };
                await _context.Translado.AddAsync(Translado);
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
