using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Imagen", Schema = "Quejas")]
    public class ImagenQueja
    {
        [Key]
        public decimal idImagen { get; set; }
        public byte[] imagen { get; set; } = Array.Empty<byte>();
    }
}
