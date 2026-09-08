using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Pais", Schema = "General")]
    public class Pais
    {
        [Key]
        public decimal ID_PaisOrigen { get; set; }
        public string NombrePais { get; set; } = "";
    }
}
