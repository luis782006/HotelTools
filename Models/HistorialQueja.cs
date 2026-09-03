using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Historial", Schema = "Quejas")]
    public class HistorialQueja
    {
        [Key]
        public decimal ID_Orden { get; set; }
        public decimal ID_Quejas { get; set; }
        public decimal ID_Empleado { get; set; }
        public string? observaciones { get; set; }
        public decimal ID_Compra { get; set; }
        public bool Aprobado { get; set; }
        public decimal ID_Imagen { get; set; }
        public decimal ID_Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
