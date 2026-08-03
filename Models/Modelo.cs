using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Modelos", Schema = "Inventarios")]
    public class Modelo
    {
        [Key]
        public decimal ID_Modelos { get; set; }
        public string NombreModelos { get; set; }
    }
}
