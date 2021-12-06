using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
     [Table("translado")]
    public class Translado
    {
        public int Id { get; set; }
        public int IdTrem { get; set; }
        public int Status { get; set; }
        public string EstacaoPartida { get; set; }
        public DateTime HoraPartida { get; set; }
        public string EstacaoChegada { get; set; }
        public DateTime HoraChegada { get; set; }
    }
}