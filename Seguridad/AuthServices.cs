using HotelTools.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTools.Seguridad
{
    public class AuthServices
    {
        private readonly SeguridadSesion _seguridadSesion;
        private readonly HotelContext _context;
        private readonly BrowserJS _browserJS;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthServices> _logger;

        public AuthServices(SeguridadSesion seguridadSesion, HotelContext context,
            BrowserJS browserJS, IConfiguration configuration, ILogger<AuthServices> logger)
        {
            _seguridadSesion = seguridadSesion;
            _context = context;
            _browserJS = browserJS;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> Login(string NombreUsuario, string Password)
        {
            return await _seguridadSesion.IniciarSesion(NombreUsuario, Password);
        }

        public async Task<bool> Logout(string token)
        {
            try
            {
                await _seguridadSesion.CerrarSesion(token);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al cerrar sesion.");
                return false;
            }
        }
    }
}
