using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Estados", Schema = "Quejas")]
    public class EstadoQueja
    {
        [Key]
        public decimal ID_Estado { get; set; }
        public string NombreEstado { get; set; } = "";
    }
}
