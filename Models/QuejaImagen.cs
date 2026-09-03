using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("QuejaImagen", Schema = "Quejas")]
    public class QuejaImagen
    {
        [Key]
        public decimal ID_QuejaImagen { get; set; }
        public decimal ID_Queja { get; set; }
        public decimal ID_Imagen { get; set; }
        public DateTime FechaAdjunto { get; set; }
    }
}
