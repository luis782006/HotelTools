using HotelTools.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace HotelTools.Seguridad
{
    public class AuthService
    {
        private readonly HotelContext _context;
      

        public AuthService( HotelContext context)
        {
            _context = context;            
        }

        public async Task<ClaimsPrincipal> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Empleados.Where( user => user.Nombre == username).FirstOrDefaultAsync();
            if (user != null && PasswordHasher.VerifyPassword(user.Password,password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre),
                    new Claim(ClaimTypes.Role, user.ID_Rol.ToString())                   
                };
                var claimsIdentity = new ClaimsIdentity(claims, "apiauth_type");
                return new ClaimsPrincipal(claimsIdentity);
            }
            return null;
        }

        public void RegisterSession(decimal userId, string sessionId)
        {
            _context.SesionActiva.Add(new SesionActiva { TokenValue = sessionId, ID_Empleado = userId, FechaExpiracion = DateTime.Now.AddMinutes(30), EstadoSesion = "Actica" });
            _context.SaveChanges();
        }

        public void UpdateSessionInDatabase(string sessionId, string? estado = "Activa")
        {
            var session = _context.SesionActiva.FirstOrDefault(s => s.TokenValue == sessionId);
            if (session == null) return;

            if (!estado.Equals("Activa"))
            {
                session.FechaExpiracion = DateTime.Now.AddMinutes(30);            
            }
            else
            {                        
                session.FechaExpiracion = DateTime.Now;
            }
            //Actualizo accion EF
            _context.SaveChanges();
        }
    }
}

