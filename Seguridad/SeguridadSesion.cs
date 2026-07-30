using HotelTools.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HotelTools.Seguridad
{
    public class SeguridadSesion
    {
        private readonly HotelContext _context;
        private readonly SeguridadGlobal _seguridadGlobal;
        private readonly CustomAuthenticationStateProvider _authProvider;
        private readonly BrowserJS _browserJS;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SeguridadSesion> _logger;

        public SesionActiva SesionActual { get; set; }
        public Empleado EmpleadoActual { get; set; }
        public List<string> Permisos { get; set; } = new List<string>();
        public string LastError { get; set; }

        public SeguridadSesion(HotelContext context, SeguridadGlobal seguridadGlobal,
            AuthenticationStateProvider authProvider, BrowserJS browserJS,
            IConfiguration configuration, ILogger<SeguridadSesion> logger)
        {
            _context = context;
            _seguridadGlobal = seguridadGlobal;
            _authProvider = (CustomAuthenticationStateProvider)authProvider;
            _browserJS = browserJS;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> IniciarSesion(string nombreUsuario, string password)
        {
            var candidatos = await _context.Empleados
                .Where(e => e.Nombre.Trim() == nombreUsuario.Trim())
                .ToListAsync();

            var empleado = candidatos.FirstOrDefault(u =>
                PasswordHasher.VerifyPassword(password, u.Password, _configuration));

            if (empleado == null)
            {
                LastError = "Usuario o Contraseña Incorrectos.";
                return false;
            }

            if (empleado.Activo == 0)
            {
                LastError = "Usuario desactivado.";
                return false;
            }

            var rol = await _context.Rol.FindAsync(empleado.ID_Rol);
            if (rol == null)
            {
                LastError = "Rol no asignado.";
                return false;
            }

            string token = Guid.NewGuid().ToString().ToUpper();

            SesionActual = new SesionActiva
            {
                Token = token,
                ID_Empleado = empleado.ID_Empleado,
                FechaExpiracion = DateTime.Now.AddMinutes(30),
                EstadoSesion = true
            };

            _context.SesionesActivas.Add(SesionActual);
            await _context.SaveChangesAsync();

            EmpleadoActual = empleado;
            Permisos = await _seguridadGlobal.PermisosDelEmpleado(empleado.ID_Empleado);

            await _browserJS.SetCookie(_configuration["Util:CookieName"], token);

            _authProvider.LoginNotify(token, empleado.Nombre.Trim(), rol.NombreRol.Trim(), empleado.Nombre.Trim(), Permisos);

            return true;
        }

        public async Task<bool> TienePermiso(string codigoPermiso)
        {
            if (SesionActual == null) return false;

            bool permisoExiste = _seguridadGlobal.PermisosDisponibles
                .Exists(p => p.Codigo.Trim().ToUpper() == codigoPermiso.Trim().ToUpper());

            if (permisoExiste)
            {
                return Permisos.Exists(p => p.Trim().ToUpper() == codigoPermiso.Trim().ToUpper());
            }
            else
            {
                await _seguridadGlobal.CrearPermiso(codigoPermiso, "Creado automaticamente");
                return false;
            }
        }

        public async Task CerrarSesion(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var sesion = await _context.SesionesActivas
                    .FirstOrDefaultAsync(s => s.Token == token);
                if (sesion != null)
                {
                    sesion.EstadoSesion = false;
                    sesion.FechaExpiracion = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
            }

            SesionActual = null;
            EmpleadoActual = null;
            Permisos.Clear();

            await _browserJS.DeleteCookie(_configuration["Util:CookieName"]);
            _authProvider.LogoutNotify();
        }

        public async Task<bool> ValidarSession(string token)
        {
            if (SesionActual == null && !string.IsNullOrWhiteSpace(token))
            {
                SesionActual = await _context.SesionesActivas
                    .FirstOrDefaultAsync(s => s.Token == token.Trim().ToUpper());
            }

            if (SesionActual == null) return false;

            if (SesionActual.Token.Trim().ToUpper() != token.Trim().ToUpper())
            {
                LastError = "Token Invalido.";
                return false;
            }

            if (!SesionActual.EstadoSesion)
            {
                LastError = "Sesion cerrada.";
                await CerrarSesion(token);
                return false;
            }

            if (SesionActual.FechaExpiracion < DateTime.Now)
            {
                LastError = "Sesion caducada.";
                await CerrarSesion(token);
                return false;
            }

            if (EmpleadoActual == null)
            {
                EmpleadoActual = await _context.Empleados.FindAsync(SesionActual.ID_Empleado);
            }

            if (EmpleadoActual != null && Permisos.Count == 0)
            {
                Permisos = await _seguridadGlobal.PermisosDelEmpleado(EmpleadoActual.ID_Empleado);
            }

            return true;
        }
    }
}
