using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Idioma", Schema = "General")]
    public class Idioma
    {
        [Key]
        public decimal ID_Idioma { get; set; }
        public string NombreIdioma { get; set; } = "";
    }
}
