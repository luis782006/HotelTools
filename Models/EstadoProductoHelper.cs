using MudBlazor;

namespace HotelTools.Models
{
    public static class EstadoProductoHelper
    {
        public static string GetColorHex(this EstadoProducto estado) => estado switch
        {
            EstadoProducto.Nuevo => "#81C784",
            EstadoProducto.Reparado => "#FFB74D",
            EstadoProducto.Baja => "#EF5350",
            _ => "#9E9E9E"
        };

        public static Color GetMudColor(this EstadoProducto estado) => estado switch
        {
            EstadoProducto.Nuevo => Color.Success,
            EstadoProducto.Reparado => Color.Warning,
            EstadoProducto.Baja => Color.Error,
            _ => Color.Default
        };

        public static string GetLeyenda(this EstadoProducto estado) => estado switch
        {
            EstadoProducto.Nuevo => "Nuevo",
            EstadoProducto.Reparado => "Reparado",
            EstadoProducto.Baja => "Baja",
            _ => "Desconocido"
        };

        public static EstadoProducto FromString(string estado) => estado?.ToLower() switch
        {
            "nuevo" => EstadoProducto.Nuevo,
            "reparado" => EstadoProducto.Reparado,
            "baja" => EstadoProducto.Baja,
            _ => EstadoProducto.Nuevo
        };

        public static string ToDbString(this EstadoProducto estado) => estado switch
        {
            EstadoProducto.Nuevo => "Nuevo",
            EstadoProducto.Reparado => "Reparado",
            EstadoProducto.Baja => "Baja",
            _ => "Nuevo"
        };
    }
}