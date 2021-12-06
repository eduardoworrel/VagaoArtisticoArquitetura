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
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ArtistaController : ControllerBase
    {
        private readonly ILogger<ArtistaController> _logger;
        private readonly ArtistaContext _context;
        private readonly IConfiguration _conf;

        public ArtistaController(ILogger<ArtistaController> logger, IConfiguration conf, ArtistaContext context)
        {
            _logger = logger;
            _context = context;
            _conf = conf;
        }


        [HttpGet("/artista")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Artista.ToListAsync());
        }

        [HttpGet("/artista/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var artista = await _context.Artista.Where(t => id == t.Id.ToString()).ToListAsync();
            return Ok(artista);
        }
        
        [HttpGet("/artista/exist/{id}")]
        public async Task<IActionResult> ExistById(int id)
        {
            bool artista = await _context.Artista.AnyAsync(a => id == a.Id);
            return Ok(artista);
        }
        [HttpDelete("/delete/artista/{id}")]
        public async Task<int> DeleteById(int id)
        {
            var artista = await _context.Artista.Where(t => id == t.Id).ToListAsync();
            _context.Artista.RemoveRange(artista);
            return await _context.SaveChangesAsync();
        }
        [HttpPut("/update/artista/{id}")]
        public async Task<int> UpdateById(int id, string nome, string cpf, string estilo, string celular, string senha)
        {
            Artista artista = await _context.Artista.Where(a => id == a.Id).FirstAsync();
            if (!String.IsNullOrEmpty(nome))
                artista.Nome = nome;
            if (!String.IsNullOrEmpty(cpf))
                artista.CPF = cpf;
            if (!String.IsNullOrEmpty(estilo))
                artista.Estilo = estilo;
            if (!String.IsNullOrEmpty(celular))
                artista.Celular = celular;
            if (!String.IsNullOrEmpty(senha))
                artista.Senha = senha;
            int response = await _context.SaveChangesAsync();
            return response;
        }
        [HttpPost("/create/artista")]
        public async Task<int> Create(string nome, string cpf, string estilo, string celular, string senha)
        {
            if (!_context.Artista.Any(a => a.CPF == cpf || a.Celular == celular))
            {
                var artista = new Artista { Nome = nome, CPF = cpf, Estilo = estilo, Celular = celular, Senha = senha };
                await _context.Artista.AddAsync(artista);
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
