using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("SesionesActiva", Schema = "Empleados")]
    public class SesionActiva
    {
        [Key]
        public decimal ID_SesionesActiva { get; set; }
        public string Token { get; set; }
        public decimal ID_Empleado { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool EstadoSesion { get; set; }
    }

}
