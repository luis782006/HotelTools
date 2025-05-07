
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Cargo", Schema = "Empleados")]
    public class Cargo
    {
        [Key]
        public decimal ID_Cargo { get; set; }

        public string NombreCargo { get; set; } = string.Empty;
    }
}