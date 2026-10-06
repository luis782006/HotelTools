using BCrypt.Net;


namespace HotelTools.Seguridad
{
    public class PasswordHasher
    {
        /// <summary>
        /// Hashea con el pepper vigente (Util:ClaveSecreta). Lanza si no está definido.
        /// </summary>
        public static string HashPassword(string password, IConfiguration _config)
        {
            string claveSecreta = _config["Util:ClaveSecreta"]
                ?? throw new InvalidOperationException(
                    "Falta la configuración 'Util:ClaveSecreta' (variable de entorno 'Util__ClaveSecreta').");
            if (string.IsNullOrEmpty(claveSecreta))
                throw new InvalidOperationException(
                    "La configuración 'Util:ClaveSecreta' está vacía (variable de entorno 'Util__ClaveSecreta').");

            return BCrypt.Net.BCrypt.HashPassword(password + claveSecreta);
        }

        public static bool VerifyPassword(string passwordLogin, string passwordUser, IConfiguration _config)
        {
            return VerifyPassword(passwordLogin, passwordUser, _config, out _);
        }

        /// <summary>
        /// Verificación dual durante la ventana de migración del pepper:
        /// primero con el pepper vigente y, si falla y está definido
        /// Util:ClaveSecretaAnterior, con el anterior. El parámetro de salida
        /// indica que el hash debe re-hashearse con el pepper vigente.
        /// Sin Util:ClaveSecretaAnterior definida, el pepper viejo no se considera.
        /// </summary>
        public static bool VerifyPassword(string passwordLogin, string passwordUser,
            IConfiguration _config, out bool verificadoConPepperAnterior)
        {
            verificadoConPepperAnterior = false;

            string claveSecreta = _config["Util:ClaveSecreta"];
            if (string.IsNullOrEmpty(claveSecreta))
                return false;

            if (BCrypt.Net.BCrypt.Verify(passwordLogin + claveSecreta, passwordUser))
                return true;

            string claveAnterior = _config["Util:ClaveSecretaAnterior"];
            if (!string.IsNullOrEmpty(claveAnterior) &&
                BCrypt.Net.BCrypt.Verify(passwordLogin + claveAnterior, passwordUser))
            {
                verificadoConPepperAnterior = true;
                return true;
            }

            return false;
        }
    }
}
