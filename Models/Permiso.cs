using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Permisos", Schema = "Empleados")]
    public class Permiso
    {
        [Key]
        public decimal ID_Permiso { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
