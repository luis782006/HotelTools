using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelTools.Models
{
    [Table("Departamentos", Schema = "General")]
    public class Departamento
    {
        [Key]
        public decimal ID_Departamento { get; set; }
        public string NombreDepartamento { get; set; }
    }
}
