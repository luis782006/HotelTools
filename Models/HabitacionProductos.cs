using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("HabitacionProductos", Schema = "Inventarios")]
    public class HabitacionProductos
    {
        [Key]
        public decimal ID_HabProductos { get; set; }
        public decimal ID_HabFK { get; set; }
        public decimal ID_ProductosFK { get; set; }
        public int Cantidad { get; set; }
        public bool EsNativo { get; set; } = true;
        public bool EsPrestamo { get; set; } = false;
        public bool Activo { get; set; } = true;
        public bool PrestadoFuera { get; set; } = false;
        public DateTime? FechaAsignacion { get; set; }
        public DateTime? FechaRetiro { get; set; }
    }
}
