using HotelTools.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelTools.Seguridad
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
       
        public readonly AuthServices _authService;
        public ClaimsPrincipal claimsPrincipal = new();
        

        public CustomAuthenticationStateProvider()
        {
           
            
        }

        /// <summary>
        /// Metodo para obtener el estado de la autenticación
        /// </summary>        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.FromResult(0); 
            return new AuthenticationState(claimsPrincipal);
        }

        public void LoginNotify(string nombre, decimal ID_Empleado,string rol)
        {
            var identity= new ClaimsIdentity(new[]
            {
               new Claim(ClaimTypes.Name, nombre.Trim()),
               new Claim(ClaimTypes.Role, rol.Trim()),
               new Claim(ClaimTypes.NameIdentifier, ID_Empleado.ToString())
            }, "apiauth_type");

            claimsPrincipal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


        //public void NotificarLogin(Empleado empleado, string rol, string token)
        //{
        //    var claims = new List<Claim>
        //    {
        //            new Claim(ClaimTypes.Name, empleado.Nombre.Trim()),
        //            new Claim(ClaimTypes.Role, rol),
        //            new Claim(ClaimTypes.NameIdentifier, empleado.ID_Empleado.ToString())
        //    };
        //    var claimsIdentity = new ClaimsIdentity(claims, "apiauth_type");

        //    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        //}

    }    

      
     

    
}
