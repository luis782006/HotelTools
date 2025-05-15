using HotelTools.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace HotelTools.Seguridad
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous;
        private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

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
