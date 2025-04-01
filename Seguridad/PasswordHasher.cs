using BCrypt.Net;


namespace HotelTools.Seguridad
{
    public class PasswordHasher
    {
        private static IConfiguration _config;

        public PasswordHasher(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static string HashPassword(string password)
        {
           string claveSecreta = _config["Util:ClaveSecreta"];
            return BCrypt.Net.BCrypt.HashPassword(password + claveSecreta);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string claveSecreta = _config["Util:ClaveSecreta"];
            return BCrypt.Net.BCrypt.Verify(password + claveSecreta, hashedPassword);
        }
    }
}
