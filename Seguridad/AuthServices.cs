

using HotelTools.Autenticacion;
using HotelTools.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace HotelTools.Seguridad
{
    public class AuthServices
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthServices> _logger;
        private readonly BrowserJS _browserJS;
        


        public AuthServices(HotelContext context, IConfiguration config, ILogger<AuthServices> logger, 
                            BrowserJS browserJS )
        {
            _context = context;
            _configuration = config;
            _logger = logger;
            _browserJS = browserJS;
            
        }
        public async Task<bool> Login(string NombreUsuario, string Password)
        {
            try
            {
                // Busca en la base de datos un empleado cuyo nombre coincida con el proporcionado 
                // y cuya contraseña sea válida según el método de verificación de contraseñas.               
                var candidatos = await _context.Empleados
                     .Where(e => e.Nombre == NombreUsuario)
                     .ToListAsync();

                var usuario = candidatos.FirstOrDefault(u =>
                    PasswordHasher.VerifyPassword(Password, u.Password, _configuration));

                // obtener Objeto Rol (rol.NombreRol)
                var rol = await _context.Rol.Where(
                        rol => rol.ID_Rol == usuario.ID_Rol).FirstOrDefaultAsync();

                if (usuario != null)
                {   
                    //creo objeto sesion para la BD
                    var sesionUsuario=await CrearSession(usuario);

                    // Registro la Cookie en el navegador
                    await _browserJS.SetCookie(_configuration["Util:CookieName"], sesionUsuario.Token);

                    //Guardo la sesion activa en BD
                    var OkGuardoSesion= await GuardarSession(sesionUsuario);
                    
                    if (OkGuardoSesion)
                    {   // Notifico el login al AuthenticationStateProvider
                        var authStateProvider = new CustomAuthenticationStateProvider();
                        authStateProvider.LoginNotify(usuario.Nombre, usuario.ID_Empleado, rol.NombreRol);
                        return true;
                    }
                    else
                    {
                        _logger.LogError("No se pudo guardar la sesión activa en la base de datos.");

                    }
                }
                // crear una Session
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error durante el login.");
            }

            return false;           
        }

        //crear las claim
        

        /// <summary>
        /// Crea una nueva sesión activa para un empleado.
        /// </summary>
        /// <param name="empleado">El empleado para el cual se creará la sesión. Puede ser nulo.</param>
        /// <returns>
        /// Una instancia de <see cref="SesionActiva"/> que contiene el token de sesión, 
        /// el ID del empleado, la fecha de expiración y el estado de la sesión.
        /// </returns>
        public async Task<SesionActiva> CrearSession(Empleado empleado)
        {           
           Guid guid = Guid.NewGuid();
           SesionActiva session = new SesionActiva()
           {
               Token = guid.ToString(),
               ID_Empleado = empleado.ID_Empleado,
               FechaExpiracion = DateTime.Now.AddMinutes(30),
               EstadoSesion = true
           };
            return session;
        }

        /// <summary>
        /// Guarda la sesion para un empleado em BD. De tipo <see cref="SesionActiva"/>
        /// </summary>
        /// <param name="sesionUsuario">El empleado para el cual se guarda la sesión.</param>
        /// <returns>
        /// Un valor booleano que indica si la operación de guardado fue exitosa o no. TRUE/FALSE
        /// </returns>
        public async Task<bool> GuardarSession(SesionActiva sesionUsuario)
        {
            try
            {
                await _context.SesionesActivas.AddAsync(sesionUsuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error al guardar la sesión activa en la base de datos.");
                return false;
            }
        }
       
    }
}
