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

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ApresentacaoController : ControllerBase
    {
        private readonly ILogger<ApresentacaoController> _logger;
        private readonly ApresentacaoContext _context;
        private readonly IConfiguration _conf;

        public ApresentacaoController(ILogger<ApresentacaoController> logger,
         ApresentacaoContext context, IConfiguration conf)
        {
            _logger = logger;
            _context = context;
            _conf = conf;
        }




        [HttpGet("/apresentacao")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Apresentacao.ToListAsync());
        }

        [HttpGet("/apresentacao/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var apresentacao = await _context.Apresentacao.Where(t => id == t.Id.ToString()).ToListAsync();
            return Ok(apresentacao);
        }


        [HttpGet("/apresentacao/range/{startDatetime}/{endDatetime}")]
        public async Task<IActionResult> GetByRangeDate(DateTime startDatetime, DateTime endDatetime)
        {
            var apresentacao = await _context.Apresentacao.Where(a => a.HoraComeca >= startDatetime && a.HoraTermina <= endDatetime).ToListAsync();
            return Ok(apresentacao);
        }

        [HttpDelete("/delete/apresentacao/{id}")]
        public async Task<int> DeleteById(int id)
        {
            var Apresentacao = await _context.Apresentacao.Where(t => id == t.Id).ToListAsync();
            _context.Apresentacao.RemoveRange(Apresentacao);
            return await _context.SaveChangesAsync();
        }
        [HttpPut("/update/apresentacao/{id}")]
        public async Task<int> UpdateById(int id, int idTranslado, int idArtista, DateTime horaComeca, DateTime horaTermina, int status)
        {

            Apresentacao Apresentacao = await _context.Apresentacao.Where(a => id == a.Id).FirstAsync();
            if (!string.IsNullOrEmpty(idTranslado.ToString()))
            {
                var http = new HttpClient();
                var data = http.GetAsync("http://translado:5003/translado/exist/" + idTranslado.ToString())
                .Result.Content.ReadAsStringAsync().Result;
                if (data != "true")
                    return -2;
                Apresentacao.IdTranslado = idTranslado;
            }
            if (!string.IsNullOrEmpty(idArtista.ToString()))
            {
                var http = new HttpClient();
                var data = http.GetAsync("http://artista:5002/artista/exist/" + idArtista.ToString())
                .Result.Content.ReadAsStringAsync().Result;
                if (data != "true")
                    return -3;
                Apresentacao.IdTranslado = idTranslado;
            }
            if (!string.IsNullOrEmpty(status.ToString()))
                Apresentacao.Status = status;
            if (!String.IsNullOrEmpty(horaComeca.ToString()))
                Apresentacao.HoraComeca = horaComeca;
            if (!String.IsNullOrEmpty(horaTermina.ToString()))
                Apresentacao.HoraTermina = horaTermina;

            int response = await _context.SaveChangesAsync();
            return response;
        }
        [HttpPost("/create/apresentacao")]
        public async Task<int> Create(int id, int idTranslado, int idArtista, DateTime horaComeca, DateTime horaTermina, int status)
        {
            var http = new HttpClient();
            var data = http.GetAsync("http://translado:5003/translado/exist/" + idTranslado.ToString())
            .Result.Content.ReadAsStringAsync().Result;
            if (data != "true")
                return -4;
            // data = http.GetAsync("http://translado:5003/translado/range/" + horaComeca.ToString() + "/" + horaTermina.ToString())
            // .Result.Content.ReadAsStringAsync().Result;
            
            // if (data.Length < 3)
            //     return -3;

            data = http.GetAsync("http://artista:5002/artista/exist/" + idArtista.ToString())
            .Result.Content.ReadAsStringAsync().Result;
            if (data != "true")
                return -2;

            if (!_context.Apresentacao.Any(t => t.IdTranslado == idTranslado && t.HoraComeca >= horaComeca && t.HoraTermina <= horaTermina))
            {
                var Apresentacao = new Apresentacao { IdTranslado = idTranslado, IdArtista = idArtista, HoraComeca = horaComeca, HoraTermina = horaTermina, Status = status };
                await _context.Apresentacao.AddAsync(Apresentacao);
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
