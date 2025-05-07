using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelTools.Models
{

    [Table("Empleados", Schema = "Empleados")]
    public class Empleado
    {
        [Key]
        public decimal ID_Empleado { get; set; } 
        public string Nombre { get; set; }=string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public decimal ID_Cargo { get; set; }
        public decimal ID_Departamento { get; set; }
        public string NroContacto { get; set; } = string.Empty;
        public decimal ID_Rol { get; set; } 
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public byte Activo { get; set; }
        public string Password { get; set; } = string.Empty;
    }

}
