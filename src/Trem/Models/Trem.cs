using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("trem")]
    public class Trem
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
    }
}