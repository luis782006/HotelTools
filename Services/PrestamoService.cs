using HotelTools.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTools.Services
{
    public class PrestamoService
    {
        private readonly HotelContext _context;

        public PrestamoService(HotelContext context)
        {
            _context = context;
        }

        public async Task<ResultadoOperacion> PrestamoInicial(
            decimal idProducto, decimal idHabitacionDestino, decimal idEmpleado, string? observaciones = null)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null)
                return new ResultadoOperacion(false, "Producto no encontrado.");

            var idCasa = producto.ID_HabitacionOrigenFK;
            if (idCasa == null)
                return new ResultadoOperacion(false, "El producto no tiene casa definida.");

            if (idHabitacionDestino == idCasa.Value)
                return new ResultadoOperacion(false, "No se puede prestar un producto a su propia casa.");

            if (producto.ID_HabitacionFK != idCasa.Value)
                return new ResultadoOperacion(false, "El producto ya está prestado. Use traslado para moverlo.");

            var habDestino = await _context.Habitaciones.FindAsync(idHabitacionDestino);
            if (habDestino == null)
                return new ResultadoOperacion(false, "Habitación destino no encontrada.");

            var habCasa = await _context.Habitaciones.FindAsync(idCasa.Value);
            if (habCasa == null)
                return new ResultadoOperacion(false, "Habitación casa no encontrada.");

            var registroCasa = await _context.HabitacionProductos.FirstOrDefaultAsync(h =>
                h.ID_ProductosFK == idProducto && h.ID_HabFK == idCasa.Value && h.EsNativo);

            var now = DateTime.Now;

            // 1. Registrar movimiento PRESTAMO
            var movimiento = new ProductosMovimientos
            {
                ID_Productos = idProducto,
                ID_HabOrigen = idCasa.Value,
                ID_HabDestino = idHabitacionDestino,
                FechaMov = now,
                ID_Empleado = idEmpleado,
                ID_EmpleadoMov = idEmpleado,
                Observaciones = observaciones,
                TipoMovimiento = "PRESTAMO",
                ID_HabitacionCasa = idCasa.Value
            };
            _context.ProductosMovimientos.Add(movimiento);

            // 2. Actualizar producto
            producto.ID_HabitacionFK = idHabitacionDestino;
            producto.Prestado = true;
            producto.Prestamo = true;
            _context.Productos.Update(producto);

            // 3. En registro nativo de casa: PrestadoFuera = true
            if (registroCasa != null)
            {
                registroCasa.PrestadoFuera = true;
                _context.HabitacionProductos.Update(registroCasa);
            }

            // 4. Crear registro puente en destino (préstamo entrante)
            var registroDestino = new HabitacionProductos
            {
                ID_HabFK = idHabitacionDestino,
                ID_ProductosFK = idProducto,
                Cantidad = 1,
                EsNativo = false,
                EsPrestamo = true,
                Activo = true,
                PrestadoFuera = false,
                FechaAsignacion = now
            };
            _context.HabitacionProductos.Add(registroDestino);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion(true,
                $"Producto prestado exitosamente a habitación {habDestino.DescripcionHab}.");
        }

        public async Task<ResultadoOperacion> Traslado(
            decimal idProducto, decimal idHabitacionDestino, decimal idEmpleado, string? observaciones = null)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null)
                return new ResultadoOperacion(false, "Producto no encontrado.");

            var idCasa = producto.ID_HabitacionOrigenFK;
            if (idCasa == null)
                return new ResultadoOperacion(false, "El producto no tiene casa definida.");

            if (idHabitacionDestino == idCasa.Value)
                return new ResultadoOperacion(false, "Si el destino es la casa, use devolución, no traslado.");

            var idHabitacionOrigen = producto.ID_HabitacionFK;
            if (idHabitacionOrigen == null)
                return new ResultadoOperacion(false, "El producto no tiene ubicación actual.");

            if (idHabitacionOrigen.Value == idCasa.Value)
                return new ResultadoOperacion(false, "El producto está en su casa. Use préstamo inicial, no traslado.");

            if (idHabitacionOrigen.Value == idHabitacionDestino)
                return new ResultadoOperacion(false, "El producto ya se encuentra en esa habitación.");

            var habDestino = await _context.Habitaciones.FindAsync(idHabitacionDestino);
            if (habDestino == null)
                return new ResultadoOperacion(false, "Habitación destino no encontrada.");

            var now = DateTime.Now;

            // 1. Registrar movimiento TRASLADO
            var movimiento = new ProductosMovimientos
            {
                ID_Productos = idProducto,
                ID_HabOrigen = idHabitacionOrigen.Value,
                ID_HabDestino = idHabitacionDestino,
                FechaMov = now,
                ID_Empleado = idEmpleado,
                ID_EmpleadoMov = idEmpleado,
                Observaciones = observaciones,
                TipoMovimiento = "TRASLADO",
                ID_HabitacionCasa = idCasa.Value
            };
            _context.ProductosMovimientos.Add(movimiento);

            // 2. Actualizar producto (ubicación cambia, flags se mantienen)
            producto.ID_HabitacionFK = idHabitacionDestino;
            _context.Productos.Update(producto);

            // 3. Soft delete del registro puente en origen
            var registroOrigen = await _context.HabitacionProductos.FirstOrDefaultAsync(h =>
                h.ID_ProductosFK == idProducto && h.ID_HabFK == idHabitacionOrigen.Value && h.Activo && !h.EsNativo);
            if (registroOrigen != null)
            {
                registroOrigen.Activo = false;
                registroOrigen.FechaRetiro = now;
                _context.HabitacionProductos.Update(registroOrigen);
            }

            // 4. Crear registro puente en destino (préstamo entrante)
            var registroDestino = new HabitacionProductos
            {
                ID_HabFK = idHabitacionDestino,
                ID_ProductosFK = idProducto,
                Cantidad = 1,
                EsNativo = false,
                EsPrestamo = true,
                Activo = true,
                PrestadoFuera = false,
                FechaAsignacion = now
            };
            _context.HabitacionProductos.Add(registroDestino);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion(true,
                $"Producto trasladado exitosamente a habitación {habDestino.DescripcionHab}.");
        }

        public async Task<ResultadoOperacion> Devolucion(
            decimal idProducto, decimal idHabitacionDestino, decimal idEmpleado, string? observaciones = null)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null)
                return new ResultadoOperacion(false, "Producto no encontrado.");

            var idCasa = producto.ID_HabitacionOrigenFK;
            if (idCasa == null)
                return new ResultadoOperacion(false, "El producto no tiene casa definida.");

            var idHabitacionOrigen = producto.ID_HabitacionFK;
            if (idHabitacionOrigen == null)
                return new ResultadoOperacion(false, "El producto no tiene ubicación actual.");

            if (idHabitacionOrigen.Value == idHabitacionDestino)
                return new ResultadoOperacion(false, "El producto ya se encuentra en esa habitación.");

            var habDestino = await _context.Habitaciones.FindAsync(idHabitacionDestino);
            if (habDestino == null)
                return new ResultadoOperacion(false, "Habitación destino no encontrada.");

            var now = DateTime.Now;
            bool esDevolucionACasa = idHabitacionDestino == idCasa.Value;

            // 1. Registrar movimiento DEVOLUCION
            var movimiento = new ProductosMovimientos
            {
                ID_Productos = idProducto,
                ID_HabOrigen = idHabitacionOrigen.Value,
                ID_HabDestino = idHabitacionDestino,
                FechaMov = now,
                ID_Empleado = idEmpleado,
                ID_EmpleadoMov = idEmpleado,
                Observaciones = observaciones,
                TipoMovimiento = "DEVOLUCION",
                ID_HabitacionCasa = idCasa.Value
            };
            _context.ProductosMovimientos.Add(movimiento);

            // 2. Soft delete del registro puente en origen actual
            var registroOrigen = await _context.HabitacionProductos.FirstOrDefaultAsync(h =>
                h.ID_ProductosFK == idProducto && h.ID_HabFK == idHabitacionOrigen.Value && h.Activo && !h.EsNativo);
            if (registroOrigen != null)
            {
                registroOrigen.Activo = false;
                registroOrigen.FechaRetiro = now;
                _context.HabitacionProductos.Update(registroOrigen);
            }

            if (esDevolucionACasa)
            {
                // 3a. Devolución a casa: actualizar producto
                producto.ID_HabitacionFK = idCasa.Value;
                producto.Prestado = false;
                producto.Prestamo = false;
                _context.Productos.Update(producto);

                // 4a. En registro nativo de casa: PrestadoFuera = false
                var registroCasa = await _context.HabitacionProductos.FirstOrDefaultAsync(h =>
                    h.ID_ProductosFK == idProducto && h.ID_HabFK == idCasa.Value && h.EsNativo);
                if (registroCasa != null)
                {
                    registroCasa.PrestadoFuera = false;
                    _context.HabitacionProductos.Update(registroCasa);
                }
            }
            else
            {
                // 3b. Devolución a intermedia: actualizar ubicación, flags se mantienen
                producto.ID_HabitacionFK = idHabitacionDestino;
                _context.Productos.Update(producto);

                // 4b. Crear registro puente en intermedia destino
                var registroDestino = new HabitacionProductos
                {
                    ID_HabFK = idHabitacionDestino,
                    ID_ProductosFK = idProducto,
                    Cantidad = 1,
                    EsNativo = false,
                    EsPrestamo = true,
                    Activo = true,
                    PrestadoFuera = false,
                    FechaAsignacion = now
                };
                _context.HabitacionProductos.Add(registroDestino);
            }

            await _context.SaveChangesAsync();

            return new ResultadoOperacion(true,
                $"Producto devuelto exitosamente a habitación {habDestino.DescripcionHab}.");
        }

        public async Task<List<ProductosMovimientos>> ObtenerHistorial(decimal idProducto)
        {
            return await _context.ProductosMovimientos
                .Where(m => m.ID_Productos == idProducto)
                .OrderBy(m => m.FechaMov)
                .ToListAsync();
        }

        public async Task<List<decimal>> ObtenerDestinosPosibles(decimal idProducto)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto?.ID_HabitacionOrigenFK == null)
                return new List<decimal>();

            var idCasa = producto.ID_HabitacionOrigenFK.Value;
            var idActual = producto.ID_HabitacionFK ?? 0;

            var destinos = new List<decimal> { idCasa };

            var movimientosPrevios = await _context.ProductosMovimientos
                .Where(m => m.ID_Productos == idProducto
                    && (m.TipoMovimiento == "PRESTAMO" || m.TipoMovimiento == "TRASLADO"))
                .ToListAsync();

            foreach (var mov in movimientosPrevios)
            {
                if (mov.ID_HabDestino != idActual && mov.ID_HabDestino != idCasa && !destinos.Contains(mov.ID_HabDestino))
                    destinos.Add(mov.ID_HabDestino);
            }

            return destinos;
        }
    }

    public class ResultadoOperacion
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public ResultadoOperacion(bool exito, string mensaje)
        {
            Exito = exito;
            Mensaje = mensaje;
        }
    }
}
