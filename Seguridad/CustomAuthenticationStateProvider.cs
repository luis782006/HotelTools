using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelTools.Seguridad
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;
        private ClaimsPrincipal _currentUser;
        private readonly IJSRuntime _js;
        
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
            var identity = new ClaimsIdentity();
            if (_currentUser?.Identity != null && _currentUser.Identity.IsAuthenticated)
            {
                identity = _currentUser.Identity as ClaimsIdentity;
            }
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        ///<sumary>
        /// Valida las credenciales del usuario y actualiza el estado de autenticación
        /// </sumary>
        /// 
        public async Task<bool> ValidateLoginAsync(string username, string password)
        {
            _currentUser = await _authService.ValidateUserAsync(username, password);

            if (_currentUser != null && _currentUser.Identity.IsAuthenticated)
            {
                // Registrar la sesión en la base de datos
                var userId = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var sessionId = Guid.NewGuid().ToString();
                _authService.RegisterSession(decimal.Parse(userId), sessionId);

                // Actualizar la cookie de sesión
                _authService.UpdateSessionInDatabase(sessionId, "Activa");

                // Aquí deberías implementar la lógica para actualizar la cookie en el navegador del cliente
                _js.InvokeAsync<object>("extrasJS.SetCookie", CookieName, sessionId);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
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
