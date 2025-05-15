using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace HotelTools.Autenticacion
{
    public class EstadoAuthProveedor : AuthenticationStateProvider

    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _principal=new ClaimsPrincipal(new ClaimsIdentity());

        public EstadoAuthProveedor(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage; 
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Obtengo La session con el Nombre SessionUsuario
                var userSessionStorageResult = await _sessionStorage.GetAsync<SessionUsuario>("SessionUsuario"); 
                // Si la session existe, obtengo el usuario sino es null
                var sessionUsuario = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

                //Si no hay session AuthenticationState no tendra claims, no hay informacion.
                if (sessionUsuario == null) 
                    return await Task.FromResult(new AuthenticationState(_principal));

                // Creo un ClaimsPrincipal con el nombre y el rol del usuario
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,sessionUsuario.NombreUsuario),
                        new Claim(ClaimTypes.Role,sessionUsuario.Role)
                    },"AuthCustomizada")) ;
                    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(_principal));
            }          
        }

        public async Task UpdateAuthenticationState(SessionUsuario sessionUsuario)
        {
            ClaimsPrincipal claimPrincipal;
            if (sessionUsuario != null)
            {
                await _sessionStorage.SetAsync("SessionUsuario", sessionUsuario);
                claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,sessionUsuario.NombreUsuario),
                    new Claim(ClaimTypes.Role,sessionUsuario.Role)
                }));
            }
            else
            {
                await _sessionStorage.DeleteAsync("SessionUsuario");
                claimPrincipal = _principal;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimPrincipal)));
        }
    }
}
