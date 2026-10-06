using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace HotelTools.Seguridad
{
    /// <summary>
    /// Exención del FallbackPolicy para los endpoints del circuito Blazor (/_blazor)
    /// y los assets públicos del framework (/_framework/blazor.web.js): sin ellos la
    /// página de login no puede arrancar el circuito (ambos llegan como endpoint sin
    /// [AllowAnonymous] y el fallback los retaba con 302 → HTML en vez de JS).
    /// Cualquier otra ruta sigue siendo retada por el FallbackPolicy; el acceso a
    /// páginas protegidas queda cubierto por [Authorize] en cada página, por el
    /// AuthorizeRouteView y por el middleware que exige sesión validada en BD.
    /// Delega al handler por defecto en todos los demás casos.
    /// </summary>
    public class BlazorCircuitResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _default = new();

        public Task HandleAsync(RequestDelegate next, HttpContext context,
            AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (!authorizeResult.Succeeded && (EsEndpointCircuito(context.Request.Path) ||
                                              EsAssetFramework(context.Request.Path)))
            {
                return next(context);
            }

            return _default.HandleAsync(next, context, policy, authorizeResult);
        }

        private static bool EsEndpointCircuito(PathString path) =>
            path.StartsWithSegments("/_blazor");

        // Assets estaticos del framework Blazor (p. ej. /_framework/blazor.web.js):
        // sin ellos la pagina de login no puede arrancar el circuito. Son archivos
        // publicos; el acceso a paginas sigue protegido por [Authorize] y por el
        // middleware de sesion validada en BD.
        private static bool EsAssetFramework(PathString path) =>
            path.StartsWithSegments("/_framework");
    }
}
