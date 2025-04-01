using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Rol", Schema = "Empleados")]
    public class Rol
    {
        [Key]
        public decimal ID_Rol { get; set; }
        public string NombreRol { get; set; }      
    }

}
