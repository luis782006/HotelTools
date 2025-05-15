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
        public readonly AuthServices _authService;
        public ClaimsPrincipal claimsPrincipal = new();
        public readonly IJSRuntime _js;

        public CustomAuthenticationStateProvider(AuthServices authService, IJSRuntime js)
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

        public void NotificarLogin(Empleado empleado, string rol, string token)
        {
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, empleado.Nombre.Trim()),
                    new Claim(ClaimTypes.Role, rol),
                    new Claim(ClaimTypes.NameIdentifier, empleado.ID_Empleado.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "apiauth_type");

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

    }    

      
     

    
}
