using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("RazonSocial", Schema = "Inventarios")]
    public class RazonSocial
    {
        [Key]
        public decimal ID_RazonSocial { get; set; }
        public string Nombre { get; set; }
        public decimal Cuil { get; set; }
        public string DireccionFisica { get; set; }
        public string DireccionDigital { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
    }
}
