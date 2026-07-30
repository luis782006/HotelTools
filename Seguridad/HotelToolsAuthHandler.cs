using System.Security.Claims;
using System.Text.Encodings.Web;
using HotelTools.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HotelTools.Seguridad
{
    public class HotelToolsAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly SeguridadGlobal _seguridadGlobal;

        public HotelToolsAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, SeguridadGlobal seguridadGlobal)
            : base(options, logger, encoder)
        {
            _seguridadGlobal = seguridadGlobal;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var cookieName = Context.RequestServices.GetRequiredService<IConfiguration>()["Util:CookieName"] ?? "HotelTools";

            if (!Request.Cookies.TryGetValue(cookieName, out var token) || string.IsNullOrWhiteSpace(token))
                return AuthenticateResult.NoResult();

            using var scope = Context.RequestServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HotelContext>();

            var sesion = await context.SesionesActivas
                .FirstOrDefaultAsync(s => s.Token == token && s.EstadoSesion && s.FechaExpiracion > DateTime.Now);

            if (sesion == null)
                return AuthenticateResult.NoResult();

            var empleado = await context.Empleados.FindAsync(sesion.ID_Empleado);
            if (empleado == null)
                return AuthenticateResult.NoResult();

            var rol = await context.Rol.FindAsync(empleado.ID_Rol);
            var permisos = await _seguridadGlobal.PermisosDelEmpleado(empleado.ID_Empleado);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, sesion.Token),
                new Claim(ClaimTypes.Name, empleado.Nombre.Trim()),
                new Claim(ClaimTypes.NameIdentifier, empleado.ID_Empleado.ToString())
            };

            if (rol != null)
                claims.Add(new Claim(ClaimTypes.Role, rol.NombreRol.Trim()));

            foreach (var p in permisos)
                claims.Add(new Claim("Permiso", p));

            var identity = new ClaimsIdentity(claims, "HotelToolsCookie");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "HotelToolsCookie");

            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login");
            return Task.CompletedTask;
        }
    }
}
