using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("TipoProducto", Schema = "Inventarios")]
    public class TipoProducto
    {
        [Key]
        public decimal ID_TipoProducto { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal ID_CategoriaProFK { get; set; }
        public decimal ID_ModelosFK { get; set; }
    }
}
