using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("RolPermisos", Schema = "Empleados")]
    public class RolPermiso
    {
        [Key]
        public decimal ID_RolPermiso { get; set; }
        public decimal ID_Rol { get; set; }
        public decimal ID_Permiso { get; set; }
        public decimal? ID_Departamento { get; set; }
    }
}
