using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("CategoriasQueja", Schema = "Quejas")]
    public class CategoriaQueja
    {
        [Key]
        public decimal ID_CategoriaQueja { get; set; }
        public string NombreCategoria { get; set; } = "";
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
