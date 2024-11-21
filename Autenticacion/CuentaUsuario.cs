namespace HotelTools.Autenticacion
{
    public class CuentaUsuario
    {
        public string  NombreUsuario { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
