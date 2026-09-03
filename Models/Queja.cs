using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Quejas", Schema = "Quejas")]
    public class Queja
    {
        [Key]
        public decimal ID_Quejas { get; set; }
        public string Quejas { get; set; } = "";
        public decimal ID_Habitaciones { get; set; }
        public decimal ID_Huesped { get; set; }
        public decimal ID_Empleado { get; set; }
        public decimal ID_DepartamentoRecibe { get; set; }
        public decimal ID_DepartamentoEjecuta { get; set; }
        public decimal ID_Prioridad { get; set; }
        public decimal? ID_EmpleadoAsignacion { get; set; }
        public decimal? ID_Estado { get; set; }
    }
}
