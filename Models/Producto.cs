using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Productos", Schema = "Inventarios")]
    public class Producto
    {
        [Key]
        public decimal ID_Productos { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal ID_CategoriaProFK { get; set; }
        public decimal ID_ModelosFK { get; set; }
        public decimal? ID_NroFacturaFK { get; set; }
    }
}
