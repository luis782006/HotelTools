using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("ProductosMovimientos", Schema = "Inventarios")]
    public class ProductosMovimientos
    {
        [Key]
        public decimal ID_ProductosMovimientos { get; set; }
        public decimal ID_Productos { get; set; }
        public decimal ID_HabOrigen { get; set; }
        public decimal ID_HabDestino { get; set; }
        public DateTime FechaMov { get; set; }
        public decimal ID_Empleado { get; set; }
        public string? Observaciones { get; set; }
        public decimal ID_EmpleadoMov { get; set; }
        public string? TipoMovimiento { get; set; }
        public decimal? ID_HabitacionCasa { get; set; }
    }
}
