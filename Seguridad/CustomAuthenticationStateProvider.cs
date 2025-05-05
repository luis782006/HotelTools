using HotelTools.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelTools.Seguridad
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;
        private ClaimsPrincipal _currentUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, AuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        //Metodo que se ejecuta en cada interaccion gracias al CascadingAuthenticationState
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            // Leer la cookie personalizada
            var cookieUser = context?.Request.Cookies["Cookie_Hotel_Tools"];
            

            if (!string.IsNullOrWhiteSpace(cookieUser))
            {
                // Supongamos que en la cookie tenés un JSON con los claims
                // Podés usar System.Text.Json para deserializar
                var claims = new List<Claim>
                {
                   // new Claim(ClaimTypes.Name, cookieUser.Nombre.Trim()),
                   // new Claim(ClaimTypes.Role, rol.NombreRol.Trim()),
                   // new Claim(ClaimTypes.NameIdentifier, user.ID_Empleado.ToString())
                };

                var identity = new ClaimsIdentity(claims, "custom");
                var user = new ClaimsPrincipal(identity);

                // Para no olvidarme. Actualizar la cookie de sesión
               // _authService.UpdateSessionInDatabase(sessionId, "Activa");

                return Task.FromResult(new AuthenticationState(user));
            }

            // Si no hay cookie, retornar usuario no autenticado
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }

        // Método auxiliar para notificar cambios
        public void NotifyUserAuthentication(ClaimsPrincipal user)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        //Método para verificar las credenciales
        public async Task<bool> ValidateLoginAsync(string username, string password)
        {
            _currentUser = await _authService.ValidateUserAsync(username, password);
            if (_currentUser != null && _currentUser.Identity.IsAuthenticated)
            {
                // Registrar la sesión en la base de datos
                var userId = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var sessionId = Guid.NewGuid().ToString();
                _authService.RegisterSession(decimal.Parse(userId), sessionId);

                
            }
                return true;
        }








    //    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    //    {
    //        private readonly AuthService _authService;
    //        private readonly IJSRuntime _js;
    //        private readonly ProtectedSessionStorage sessionStorage;

    //        public const string CookieName = "Cookie_Hotel_Tools";
    //        private ClaimsPrincipal? _currentUser;

    //        //Constructor
    //        public CustomAuthenticationStateProvider(AuthService authService, IJSRuntime js, ProtectedSessionStorage protectedSessionStorage)
    //        {
    //             _authService = authService;
    //            sessionStorage = protectedSessionStorage;
    //            _js = js;
    //        }

    //        /// <summary>
    //        /// Metodo para obtener el estado de la autenticación
    //        /// </summary>        
    //        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //        {
    //            //Obtener la sesion de la cookie
    //            var result = await sessionStorage.GetAsync<User>("Cookie_Hotel_Tools");


    //            ClaimsPrincipal user = _currentUser;

    //            if (_currentUser == null || !_currentUser.Identity?.IsAuthenticated == true)
    //            {
    //                // Usuario no autenticado: devolvemos una identidad vacía
    //                user = new ClaimsPrincipal(new ClaimsIdentity());
    //            }

    //            return Task.FromResult(new AuthenticationState(user));
    //        }

    //        ///<sumary>
    //        /// Valida las credenciales del usuario y actualiza el estado de autenticación
    //        /// </sumary>
    //        /// 
    //        public async Task<bool> ValidateLoginAsync(string username, string password)
    //        {
    //            _currentUser = await _authService.ValidateUserAsync(username, password);

    //            if (_currentUser != null && _currentUser.Identity.IsAuthenticated)
    //            {
    //                // Registrar la sesión en la base de datos
    //                var userId = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //                var sessionId = Guid.NewGuid().ToString();
    //                _authService.RegisterSession(decimal.Parse(userId), sessionId);

    //                // Para no olvidarme. Actualizar la cookie de sesión
    //                _authService.UpdateSessionInDatabase(sessionId, "Activa");

    //                // Seteo la cookie en el navegador
    //                await _js.InvokeAsync<object>("extrasJS.SetCookie", CookieName, sessionId);

    //                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    //                return true;
    //            }
    //            else
    //            {
    //                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    //                return false;
    //            }

    //        }

    //        /// <summary>
    //        /// Cierra la sesión del usuario actual.
    //        /// </summary>
    //        public async Task LogoutAsync()
    //        {
    //            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    //            // Obtener la sessionId
    //            var TokenSessionId = await _js.InvokeAsync<string>("extrasJS.GetCookie", CookieName);

    //            // Actualizar la cookie de sesión en BD
    //            if (!string.IsNullOrEmpty(TokenSessionId))
    //            {
    //                _authService.UpdateSessionInDatabase(TokenSessionId, "Inactiva");
    //            }

    //            // Eliminar la cookie en el navegador del cliente
    //            await _js.InvokeVoidAsync("extrasJS.DeleteCookie", CookieName);

    //            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    //        }


    //    }
}
