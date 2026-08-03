using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Habitaciones", Schema = "General")]
    public class Habitacion
    {
        [Key]
        public decimal ID_NroHab { get; set; }
        public decimal ID_CategoriasFK { get; set; }
        public int MaxPersonas { get; set; }
        public string DescripcionHab { get; set; }
        public bool Estado { get; set; }
    }
}
