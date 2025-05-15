namespace HotelTools.Autenticacion
{
 // Esta clase representa una cuenta de usuario en el sistema y sera utilizada para autenticar al usuario.
 // Contiene las propiedades NombreUsuario, Password y Role.
 

    /// <summary>
    /// Clase que representa una cuenta de usuario en el sistema.
    /// </summary>
    public class CuentaUsuario
    {
        /// <summary>
        /// Obtiene o establece el nombre de usuario asociado a la cuenta.
        /// </summary>
        public string  NombreUsuario { get; set; }=string.Empty;

        /// <summary>
        /// Obtiene o establece la contrase a asociada a la cuenta.
        /// </summary>
        public string Password { get; set; }=string.Empty;

        /// <summary>
        /// Obtiene o establece el rol asociado a la cuenta.
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
