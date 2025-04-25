using BCrypt.Net;


namespace HotelTools.Seguridad
{
    public class PasswordHasher
    {
       
        public static string HashPassword(string password, IConfiguration _config)
        {
           string claveSecreta = _config["Util:ClaveSecreta"];
            return BCrypt.Net.BCrypt.HashPassword(password + claveSecreta);
        }

        public static bool VerifyPassword(string passwordLogin, string passwordUser, IConfiguration _config)
        {            
            string claveSecreta = _config["Util:ClaveSecreta"];                        
            bool isValid = BCrypt.Net.BCrypt.Verify(passwordLogin+claveSecreta, passwordUser);
            return isValid;
        }
    }
}
