using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("apresentacao")]
    public class Apresentacao
    {
        public int Id { get; set; }
        public int IdTranslado { get; set; }
        public int IdArtista { get; set; }
        public DateTime HoraComeca { get; set; }
        public DateTime HoraTermina { get; set; }
        public int Status { get; set; }
    }
}