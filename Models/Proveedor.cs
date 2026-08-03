using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Proveedores", Schema = "Inventarios")]
    public class Proveedor
    {
        [Key]
        public decimal ID_Proveedor { get; set; }
        public decimal ID_RazonSocialFK { get; set; }
        public decimal ID_RepresentanteFK { get; set; }
    }
}
