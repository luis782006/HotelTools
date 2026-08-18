using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("PaqueteProductoDetalle", Schema = "Inventarios")]
    public class PaqueteProductoDetalle
    {
        [Key]
        public decimal ID_Detalle { get; set; }
        public decimal ID_PaqueteFK { get; set; }
        public decimal ID_TipoProductoFK { get; set; }
        public int Cantidad { get; set; } = 1;

        public virtual PaqueteProducto? Paquete { get; set; }
        public virtual TipoProducto? TipoProducto { get; set; }
    }
}
