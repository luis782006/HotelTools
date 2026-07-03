using HotelTools.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelTools.Seguridad
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly BrowserJS _browserJS;
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
        private AuthenticationState _currentState;
        private bool _stateSetExplicitly;

        public CustomAuthenticationStateProvider(BrowserJS browserJS, HotelContext context, IConfiguration configuration)
        {
            _browserJS = browserJS;
            _context = context;
            _configuration = configuration;
            _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(_currentState);
        }

        public async Task<AuthenticationState> GetAuthenticationStateFromCookieAsync()
        {
            if (_stateSetExplicitly)
                return _currentState;

            var cookieName = _configuration["Util:CookieName"];
            var cookie = await _browserJS.GetCookie(cookieName);

            if (string.IsNullOrEmpty(cookie))
            {
                _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                return _currentState;
            }

            var sesion = await _context.SesionesActivas
                .FirstOrDefaultAsync(s => s.Token == cookie && s.EstadoSesion && s.FechaExpiracion > DateTime.Now);

            if (sesion == null)
            {
                _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                return _currentState;
            }

            // Renovar expiracion por inactividad
            sesion.FechaExpiracion = DateTime.Now.AddMinutes(30);
            await _context.SaveChangesAsync();

            var empleado = await _context.Empleados.FindAsync(sesion.ID_Empleado);
            if (empleado == null)
            {
                _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                return _currentState;
            }

            var rol = await _context.Rol.FindAsync(empleado.ID_Rol);
            if (rol == null)
            {
                _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                return _currentState;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, empleado.Nombre.Trim()),
                new Claim(ClaimTypes.Role, rol.NombreRol.Trim()),
                new Claim(ClaimTypes.NameIdentifier, empleado.ID_Empleado.ToString())
            };

            var identity = new ClaimsIdentity(claims, "cookie");
            _currentState = new AuthenticationState(new ClaimsPrincipal(identity));
            return _currentState;
        }

        public void LoginNotify(string nombre, decimal ID_Empleado, string rol)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nombre.Trim()),
                new Claim(ClaimTypes.Role, rol.Trim()),
                new Claim(ClaimTypes.NameIdentifier, ID_Empleado.ToString())
            };

            var identity = new ClaimsIdentity(claims, "cookie");
            var user = new ClaimsPrincipal(identity);

            _currentState = new AuthenticationState(user);
            _stateSetExplicitly = true;
            NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
        }

        public void LogoutNotify()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            _currentState = new AuthenticationState(anonymous);
            _stateSetExplicitly = false;
            NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
        }
    }
}
