using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Prioridad", Schema = "Quejas")]
    public class PrioridadQueja
    {
        [Key]
        public decimal ID_Prioridad { get; set; }
        public int NombrePrioridad { get; set; }
        public string? Descripcion { get; set; }
        public string? Color { get; set; }
    }
}
