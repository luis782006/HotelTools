using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Categorias", Schema = "Inventarios")]
    public class CategoriaHabitacion
    {
        [Key]
        public decimal ID_Categoria { get; set; }
        public string TipoHab { get; set; }
    }
}
