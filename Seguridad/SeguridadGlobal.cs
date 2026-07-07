using HotelTools.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTools.Seguridad
{
    public class SeguridadGlobal
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private List<Permiso> _permisosDisponibles;

        public string CookieName { get; set; }
        public List<Permiso> PermisosDisponibles => _permisosDisponibles;

        public SeguridadGlobal(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            CookieName = configuration["Util:CookieName"] ?? "HotelTools";
            _permisosDisponibles = new List<Permiso>();
            ActualizarCache();
        }

        public void ActualizarCache()
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<HotelContext>();
            _permisosDisponibles = context.Permisos.AsNoTracking().ToList();
        }

        public async Task CrearPermiso(string codigo, string descripcion)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<HotelContext>();

            var permiso = new Permiso
            {
                Codigo = codigo.Trim(),
                Descripcion = descripcion
            };

            context.Permisos.Add(permiso);
            await context.SaveChangesAsync();

            var desarrolloRol = await context.Rol.FirstOrDefaultAsync(r => r.NombreRol.Trim() == "Admin");
            if (desarrolloRol != null)
            {
                var rolPermiso = new RolPermiso
                {
                    ID_Rol = desarrolloRol.ID_Rol,
                    ID_Permiso = permiso.ID_Permiso,
                    ID_Departamento = null
                };
                context.RolPermisos.Add(rolPermiso);
                await context.SaveChangesAsync();
            }

            ActualizarCache();
        }

        public async Task<List<string>> PermisosDelEmpleado(decimal idEmpleado)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<HotelContext>();

            var empleado = await context.Empleados.FindAsync(idEmpleado);
            if (empleado == null) return new List<string>();

            var query = from rp in context.RolPermisos
                        join p in context.Permisos on rp.ID_Permiso equals p.ID_Permiso
                        where rp.ID_Rol == empleado.ID_Rol
                           && (rp.ID_Departamento == null || rp.ID_Departamento == empleado.ID_Departamento)
                        select p.Codigo.Trim();

            return await query.Distinct().ToListAsync();
        }
    }
}
