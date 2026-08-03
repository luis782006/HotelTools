using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Representante", Schema = "Inventarios")]
    public class Representante
    {
        [Key]
        public decimal ID_Representante { get; set; }
        public string Nombre { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
    }
}
