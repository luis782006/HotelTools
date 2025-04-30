using HotelTools.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace HotelTools.Seguridad
{
    public class AuthService
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
      

        public AuthService( HotelContext context, IConfiguration config)
        {
            _context = context;  
            _configuration = config;
        }

        public async Task<ClaimsPrincipal> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Empleados.Where( user => user.Nombre == username).FirstOrDefaultAsync();
            var rol = await _context.Rol.Where(rol => rol.ID_Rol == user.ID_Rol).FirstOrDefaultAsync();
            if (user != null && PasswordHasher.VerifyPassword(password, user.Password, _configuration))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre.Trim()),
                    new Claim(ClaimTypes.Role, rol.NombreRol.Trim()),
                    new Claim(ClaimTypes.NameIdentifier, user.ID_Empleado.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, "apiauth_type");
                return new ClaimsPrincipal(claimsIdentity);
            }
            return null;
        }

        public void RegisterSession(decimal userId, string sessionId)
        {
            _context.SesionesActivas.Add(new SesionActiva { Token = sessionId, ID_Empleado = userId, FechaExpiracion = DateTime.Now.AddMinutes(30), EstadoSesion = "Activa" });
            _context.SaveChanges();
        }

        public void UpdateSessionInDatabase(string sessionId, string? estado = "Activa")
        {
            var session = _context.SesionesActivas.FirstOrDefault(s => s.Token == sessionId);
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

        public async Task<ClaimsPrincipal> ValidateSessionAsync(string sessionId)
        {
            // Check if session exists and is active in database
            //var session = await GetSessionFromDatabase(sessionId);
            var session= await _context.SesionesActivas.FirstOrDefaultAsync(s => s.Token == sessionId);
            var user = await _context.Empleados.Where(user => user.ID_Empleado == session.ID_Empleado).FirstOrDefaultAsync();
            var rol = await _context.Rol.Where(rol => rol.ID_Rol == session.ID_Empleado).FirstOrDefaultAsync();
            if (session != null && session.EstadoSesion == "Activa")
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre.Trim()),
                    new Claim(ClaimTypes.Role, rol.NombreRol.Trim()),
                    new Claim(ClaimTypes.NameIdentifier, user.ID_Empleado.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, "apiauth_type");

                return claimsIdentity.IsAuthenticated ? new ClaimsPrincipal(claimsIdentity) : null;
            }
            return null;
        }
    }
}

