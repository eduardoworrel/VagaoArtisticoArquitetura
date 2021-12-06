using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("artista")]
    public class Artista
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Estilo { get; set; }
        public string Celular { get; set; }
        public string Senha { get; set; }
    }
}