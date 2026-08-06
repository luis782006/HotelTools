using MudBlazor;

namespace HotelTools.Models
{
    public static class EstadoHabitacionHelper
    {
        public static string GetColorHex(this EstadoHabitacion estado) => estado switch
        {
            EstadoHabitacion.Libre => "#81C784",
            EstadoHabitacion.Ocupada => "#6123d3",
            _ => "#9E9E9E"
        };

        public static string GetMudColor(this EstadoHabitacion estado) => estado switch
        {
            EstadoHabitacion.Libre => "#81C784",
            EstadoHabitacion.Ocupada => "#6123d3",
            _ => "#9E9E9E"
        };

        public static string GetLeyenda(this EstadoHabitacion estado) => estado switch
        {
            EstadoHabitacion.Libre => "Libre",
            EstadoHabitacion.Ocupada => "Ocupada",
            _ => "Desconocido"
        };

        public static EstadoHabitacion FromBool(bool ocupada) =>
            ocupada ? EstadoHabitacion.Ocupada : EstadoHabitacion.Libre;
    }
}