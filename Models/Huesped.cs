using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Huespedes", Schema = "Quejas")]
    public class Huesped
    {
        [Key]
        public decimal ID_Huesped { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public DateTime FechaIn { get; set; }
        public DateTime? FechaOut { get; set; }
        public int DiasAlojados { get; set; }
        public decimal ID_Idioma { get; set; }
        public decimal ID_Pais { get; set; }
    }
}
