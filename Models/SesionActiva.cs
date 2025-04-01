using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("SesionActiva", Schema = "Empleados")]
    public class SesionActiva
    {
        [Key]
        public string TokenValue { get; set; }
        public decimal ID_Empleado { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string EstadoSesion { get; set; }
    }

}
