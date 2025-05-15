using HotelTools.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelTools.Seguridad
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        
        public const string CookieName = "Cookie_Hotel_Tools";

        public CustomAuthenticationStateProvider(AuthService authService, IJSRuntime js)
        {
             _authService = authService;            
            _js = js;
        }

        /// <summary>
        /// Metodo para obtener el estado de la autenticación
        /// </summary>        
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void NotificarLogin(Empleado empleado, string rol, string token)
        {
            _currentUser = await _authService.ValidateUserAsync(username, password);

            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, empleado.Nombre.Trim()),
                    new Claim(ClaimTypes.Role, rol),
                    new Claim(ClaimTypes.NameIdentifier, empleado.ID_Empleado.ToString())
                };
            var claimsIdentity = new ClaimsIdentity(claims, "apiauth_type");
                
                return true;
            }
            else
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return false;
            }

        }

        /// <summary>
        /// Cierra la sesión del usuario actual.
        /// </summary>
        public async Task LogoutAsync()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

            // Obtener la sessionId
            var TokenSessionId = await _js.InvokeAsync<string>("extrasJS.GetCookie", CookieName);

            // Actualizar la cookie de sesión en BD
            if (!string.IsNullOrEmpty(TokenSessionId))
            {
                _authService.UpdateSessionInDatabase(TokenSessionId, "Inactiva");
            }

            // Eliminar la cookie en el navegador del cliente
            await _js.InvokeVoidAsync("extrasJS.DeleteCookie", CookieName);
            
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


    }
}
