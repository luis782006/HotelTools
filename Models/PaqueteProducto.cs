using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("PaqueteProducto", Schema = "Inventarios")]
    public class PaqueteProducto
    {
        [Key]
        public decimal ID_Paquete { get; set; }
        public string Nombre { get; set; }
        public decimal ID_CategoriaHabitacionFK { get; set; }
    }
}
