using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("CategoriasProductos", Schema = "Inventarios")]
    public class CategoriaProducto
    {
        [Key]
        public decimal ID_CategoriasPro { get; set; }
        public string NombreCatProductos { get; set; }
    }
}
