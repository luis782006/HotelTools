using HotelTools.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTools.Services
{
    public class QuejaService
    {
        private readonly HotelContext _context;

        public const string ESTADO_NUEVO = "Nuevo";
        public const string ESTADO_ASIGNADO = "Asignado";
        public const string ESTADO_EN_PROCESO = "En Proceso";
        public const string ESTADO_PENDIENTE_APROBACION = "Pendiente Aprobación";
        public const string ESTADO_RESUELTO = "Resuelto";
        public const string ESTADO_CERRADO = "Cerrado";
        public const string ESTADO_RECHAZADO = "Rechazado";

        public QuejaService(HotelContext context)
        {
            _context = context;
        }

        // RN-REG-01: Validar datos obligatorios
        public ResultadoOperacion ValidarQueja(Queja queja)
        {
            if (string.IsNullOrWhiteSpace(queja.Quejas))
                return new ResultadoOperacion(false, "El título es obligatorio.");

            if (queja.ID_Habitaciones <= 0)
                return new ResultadoOperacion(false, "La habitación es obligatoria.");

            if (queja.ID_Huesped <= 0)
                return new ResultadoOperacion(false, "El huésped es obligatorio.");

            if (queja.ID_DepartamentoRecibe <= 0)
                return new ResultadoOperacion(false, "El departamento receptor es obligatorio.");

            if (queja.ID_DepartamentoEjecuta <= 0)
                return new ResultadoOperacion(false, "El departamento ejecutor es obligatorio.");

            if (queja.ID_Prioridad <= 0)
                return new ResultadoOperacion(false, "La prioridad es obligatoria.");

            return new ResultadoOperacion(true, "Validación correcta.");
        }

        // RN-VAL-01: Verificar que la habitación exista y esté activa
        public async Task<ResultadoOperacion> ValidarHabitacion(decimal idHabitacion)
        {
            var hab = await _context.Habitaciones.FindAsync(idHabitacion);
            if (hab == null)
                return new ResultadoOperacion(false, "La habitación no existe.");

            if (!hab.Estado)
                return new ResultadoOperacion(false, "La habitación está desactivada.");

            return new ResultadoOperacion(true, "OK");
        }

        // RN-VAL-02: Verificar que el empleado esté activo
        public async Task<ResultadoOperacion> ValidarEmpleado(decimal idEmpleado)
        {
            var emp = await _context.Empleados.FindAsync(idEmpleado);
            if (emp == null)
                return new ResultadoOperacion(false, "El empleado no existe.");

            if (emp.Activo != 1)
                return new ResultadoOperacion(false, "El empleado está inactivo.");

            return new ResultadoOperacion(true, "OK");
        }

        // RN-VAL-04: Verificar que el depto ejecutor tenga empleados activos
        public async Task<ResultadoOperacion> ValidarDeptoEjecutor(decimal idDeptoEjecuta)
        {
            var tieneEmpleados = await _context.Empleados
                .AnyAsync(e => e.ID_Departamento == idDeptoEjecuta && e.Activo == 1);

            if (!tieneEmpleados)
                return new ResultadoOperacion(false, "El departamento ejecutor no tiene empleados activos.");

            return new ResultadoOperacion(true, "OK");
        }

        // RN-REG-05: Auto-sugerir depto ejecutor según categoría
        public async Task<decimal?> SugerirDeptoEjecuta(decimal idCategoria)
        {
            var cat = await _context.CategoriasQueja.FindAsync(idCategoria);
            if (cat == null) return null;

            var nombre = cat.NombreCategoria.ToLower();
            if (nombre.Contains("mantenimiento"))
                return await _context.Departamentos
                    .Where(d => d.NombreDepartamento.ToLower().Contains("mantenimiento"))
                    .Select(d => d.ID_Departamento)
                    .FirstOrDefaultAsync();

            if (nombre.Contains("limpieza"))
                return await _context.Departamentos
                    .Where(d => d.NombreDepartamento.ToLower().Contains("limpieza"))
                    .Select(d => d.ID_Departamento)
                    .FirstOrDefaultAsync();

            if (nombre.Contains("seguridad"))
                return await _context.Departamentos
                    .Where(d => d.NombreDepartamento.ToLower().Contains("seguridad"))
                    .Select(d => d.ID_Departamento)
                    .FirstOrDefaultAsync();

            if (nombre.Contains("recepción") || nombre.Contains("recepcion"))
                return await _context.Departamentos
                    .Where(d => d.NombreDepartamento.ToLower().Contains("recepción") || d.NombreDepartamento.ToLower().Contains("recepcion"))
                    .Select(d => d.ID_Departamento)
                    .FirstOrDefaultAsync();

            return null;
        }

        // RN-REG-06: Auto-elevar prioridad si es seguridad
        public async Task<decimal> AutoPrioridadSeguridad(decimal idPrioridadActual, string tituloQueja)
        {
            var texto = tituloQueja.ToLower();
            bool esSeguridad = texto.Contains("robo") || texto.Contains("agresión") ||
                               texto.Contains("agresion") || texto.Contains("accidente") ||
                               texto.Contains("incendio") || texto.Contains("emergencia");

            if (!esSeguridad) return idPrioridadActual;

            var critica = await _context.PrioridadesQueja
                .FirstOrDefaultAsync(p => p.NombrePrioridad == 4);
            return critica?.ID_Prioridad ?? idPrioridadActual;
        }

        // Crear queja
        public async Task<ResultadoOperacion> CrearQueja(Queja queja, decimal idEmpleadoCreador)
        {
            var validacion = ValidarQueja(queja);
            if (!validacion.Exito) return validacion;

            var valHab = await ValidarHabitacion(queja.ID_Habitaciones);
            if (!valHab.Exito) return valHab;

            var valDepto = await ValidarDeptoEjecutor(queja.ID_DepartamentoEjecuta);
            if (!valDepto.Exito) return valDepto;

            // Auto-elevar prioridad si seguridad
            queja.ID_Prioridad = await AutoPrioridadSeguridad(queja.ID_Prioridad, queja.Quejas);

            // Estado inicial = Nuevo
            var estadoNuevo = await _context.EstadosQueja
                .FirstOrDefaultAsync(e => e.NombreEstado == ESTADO_NUEVO);
            queja.ID_Estado = estadoNuevo?.ID_Estado;
            queja.ID_Empleado = idEmpleadoCreador;

            _context.Quejas.Add(queja);
            await _context.SaveChangesAsync();

            // RN-HIS-01: Registrar en historial
            await RegistrarHistorial(queja.ID_Quejas, idEmpleadoCreador,
                estadoNuevo?.ID_Estado ?? 0, "Queja creada");

            await _context.SaveChangesAsync();
            return new ResultadoOperacion(true, $"Queja #{queja.ID_Quejas} creada exitosamente.");
        }

        // Editar queja (solo si estado = Nuevo)
        public async Task<ResultadoOperacion> EditarQueja(Queja queja)
        {
            var existente = await _context.Quejas.FindAsync(queja.ID_Quejas);
            if (existente == null)
                return new ResultadoOperacion(false, "Queja no encontrada.");

            var estado = await _context.EstadosQueja.FindAsync(existente.ID_Estado);
            if (estado?.NombreEstado != ESTADO_NUEVO)
                return new ResultadoOperacion(false, "Solo se puede editar una queja en estado Nuevo.");

            var validacion = ValidarQueja(queja);
            if (!validacion.Exito) return validacion;

            existente.Quejas = queja.Quejas;
            existente.ID_Habitaciones = queja.ID_Habitaciones;
            existente.ID_Huesped = queja.ID_Huesped;
            existente.ID_DepartamentoRecibe = queja.ID_DepartamentoRecibe;
            existente.ID_DepartamentoEjecuta = queja.ID_DepartamentoEjecuta;
            existente.ID_Prioridad = queja.ID_Prioridad;

            await _context.SaveChangesAsync();
            return new ResultadoOperacion(true, "Queja actualizada exitosamente.");
        }

        // RN-ASI-01/02/03: Asignar empleado
        public async Task<ResultadoOperacion> AsignarEmpleado(decimal idQueja, decimal idEmpleadoAsignado, decimal idSupervisor)
        {
            var queja = await _context.Quejas.FindAsync(idQueja);
            if (queja == null)
                return new ResultadoOperacion(false, "Queja no encontrada.");

            var estado = await _context.EstadosQueja.FindAsync(queja.ID_Estado);
            if (estado?.NombreEstado != ESTADO_NUEVO && estado?.NombreEstado != ESTADO_ASIGNADO)
                return new ResultadoOperacion(false, "No se puede asignar en el estado actual.");

            var valEmp = await ValidarEmpleado(idEmpleadoAsignado);
            if (!valEmp.Exito) return valEmp;

            // RN-ASI-02: Empleado debe pertenecer al depto ejecutor
            var emp = await _context.Empleados.FindAsync(idEmpleadoAsignado);
            if (emp?.ID_Departamento != queja.ID_DepartamentoEjecuta)
                return new ResultadoOperacion(false, "El empleado no pertenece al departamento ejecutor.");

            // RN-ASI-03: Alertar si tiene >5 quejas activas
            var quejasActivas = await _context.Quejas
                .CountAsync(q => q.ID_EmpleadoAsignacion == idEmpleadoAsignado &&
                    q.ID_Estado != null && _context.EstadosQueja
                        .Where(e => e.ID_Estado == q.ID_Estado)
                        .Any(e => e.NombreEstado != ESTADO_CERRADO && e.NombreEstado != ESTADO_RECHAZADO));

            queja.ID_EmpleadoAsignacion = idEmpleadoAsignado;

            // Cambiar estado a Asignado
            var estadoAsignado = await _context.EstadosQueja
                .FirstOrDefaultAsync(e => e.NombreEstado == ESTADO_ASIGNADO);
            queja.ID_Estado = estadoAsignado?.ID_Estado;

            await _context.SaveChangesAsync();

            // RN-HIS-01/04: Registrar en historial
            await RegistrarHistorial(idQueja, idSupervisor,
                estadoAsignado?.ID_Estado ?? 0,
                $"Asignado a empleado {emp?.Nombre?.Trim()} {emp?.Apellido?.Trim()}");

            await _context.SaveChangesAsync();

            string msg = $"Queja #{idQueja} asignada exitosamente.";
            if (quejasActivas >= 5)
                msg += $" ALERTA: El empleado tiene {quejasActivas} quejas activas.";

            return new ResultadoOperacion(true, msg);
        }

        // Cambiar estado
        public async Task<ResultadoOperacion> CambiarEstado(decimal idQueja, string nuevoEstadoNombre,
            decimal idEmpleado, string? observaciones = null, decimal? idCompra = null)
        {
            var queja = await _context.Quejas.FindAsync(idQueja);
            if (queja == null)
                return new ResultadoOperacion(false, "Queja no encontrada.");

            var estadoActual = await _context.EstadosQueja.FindAsync(queja.ID_Estado);
            var nuevoEstado = await _context.EstadosQueja
                .FirstOrDefaultAsync(e => e.NombreEstado == nuevoEstadoNombre);

            if (nuevoEstado == null)
                return new ResultadoOperacion(false, "Estado destino no válido.");

            // RN-EST-01: Validar transiciones
            var transicionesPermitidas = GetTransicionesPermitidas(estadoActual?.NombreEstado ?? "");
            if (!transicionesPermitidas.Contains(nuevoEstadoNombre))
                return new ResultadoOperacion(false,
                    $"No se puede transicionar de '{estadoActual?.NombreEstado}' a '{nuevoEstadoNombre}'.");

            // RN-EST-02: Observación obligatoria para ciertos cambios
            if ((nuevoEstadoNombre == ESTADO_RESUELTO || nuevoEstadoNombre == ESTADO_RECHAZADO ||
                 nuevoEstadoNombre == ESTADO_EN_PROCESO || nuevoEstadoNombre == ESTADO_PENDIENTE_APROBACION)
                && string.IsNullOrWhiteSpace(observaciones))
                return new ResultadoOperacion(false, "La observación es obligatoria para este cambio de estado.");

            // RN-EST-03: Compra vinculada para Pendiente Aprobación
            if (nuevoEstadoNombre == ESTADO_PENDIENTE_APROBACION && idCompra == null)
                return new ResultadoOperacion(false, "Debe vincular una factura para Pendiente de Aprobación.");

            queja.ID_Estado = nuevoEstado.ID_Estado;
            await _context.SaveChangesAsync();

            // Historial
            string msg = observaciones ?? $"Estado cambiado a {nuevoEstadoNombre}";
            var hist = await RegistrarHistorial(idQueja, idEmpleado, nuevoEstado.ID_Estado, msg);

            if (idCompra.HasValue)
            {
                hist.ID_Compra = idCompra.Value;
                hist.Aprobado = false;
            }

            await _context.SaveChangesAsync();
            return new ResultadoOperacion(true, $"Estado cambiado a {nuevoEstadoNombre}.");
        }

        // RN-COM-03/04: Aprobar compra vinculada
        public async Task<ResultadoOperacion> AprobarCompra(decimal idHistorial, decimal idSupervisor, bool aprobar)
        {
            var hist = await _context.HistorialQuejas.FindAsync(idHistorial);
            if (hist == null)
                return new ResultadoOperacion(false, "Registro de historial no encontrado.");

            if (hist.ID_Compra == null || hist.ID_Compra == 0)
                return new ResultadoOperacion(false, "Este registro no tiene compra vinculada.");

            hist.Aprobado = aprobar;
            hist.ID_Empleado = idSupervisor;
            await _context.SaveChangesAsync();

            string msg = aprobar ? "Compra aprobada." : "Compra rechazada.";
            await RegistrarHistorial(hist.ID_Quejas, idSupervisor, hist.ID_Estado, msg);

            await _context.SaveChangesAsync();
            return new ResultadoOperacion(true, msg);
        }

        // RN-CIE-01/02: Cerrar queja
        public async Task<ResultadoOperacion> CerrarQueja(decimal idQueja, decimal calificacion,
            string? comentario, decimal idEmpleado)
        {
            var queja = await _context.Quejas.FindAsync(idQueja);
            if (queja == null)
                return new ResultadoOperacion(false, "Queja no encontrada.");

            var estado = await _context.EstadosQueja.FindAsync(queja.ID_Estado);
            if (estado?.NombreEstado != ESTADO_RESUELTO)
                return new ResultadoOperacion(false, "Solo se puede cerrar una queja en estado Resuelto.");

            // RN-CIE-02: Calificación 1-5 o justificación
            if (calificacion < 1 || calificacion > 5)
                return new ResultadoOperacion(false, "La calificación debe ser entre 1 y 5.");

            if (calificacion <= 2 && string.IsNullOrWhiteSpace(comentario))
                return new ResultadoOperacion(false, "Para calificación 1-2, debe indicar justificación.");

            var estadoCerrado = await _context.EstadosQueja
                .FirstOrDefaultAsync(e => e.NombreEstado == ESTADO_CERRADO);
            queja.ID_Estado = estadoCerrado?.ID_Estado;
            await _context.SaveChangesAsync();

            string msg = $"Queja cerrada. Calificación: {calificacion}/5.";
            if (!string.IsNullOrWhiteSpace(comentario))
                msg += $" Comentario: {comentario}";

            await RegistrarHistorial(idQueja, idEmpleado, estadoCerrado?.ID_Estado ?? 0, msg);

            // RN-CIE-03: Si calificación 1-2, notificar (snackbar en UI)
            await _context.SaveChangesAsync();

            string resultado = "Queja cerrada exitosamente.";
            if (calificacion <= 2)
                resultado += " Requiere revisión de calidad. Notificar al Gerente.";

            return new ResultadoOperacion(true, resultado);
        }

        // RN-EST-05: Reabrir queja (solo <24h)
        public async Task<ResultadoOperacion> ReabrirQueja(decimal idQueja, string? motivo, decimal idEmpleado)
        {
            var queja = await _context.Quejas.FindAsync(idQueja);
            if (queja == null)
                return new ResultadoOperacion(false, "Queja no encontrada.");

            var estado = await _context.EstadosQueja.FindAsync(queja.ID_Estado);
            if (estado?.NombreEstado != ESTADO_CERRADO)
                return new ResultadoOperacion(false, "Solo se pueden reabrir quejas cerradas.");

            // Verificar <24h desde el cierre
            var ultimoCierre = await _context.HistorialQuejas
                .Where(h => h.ID_Quejas == idQueja)
                .OrderByDescending(h => h.FechaRegistro)
                .FirstOrDefaultAsync();

            if (ultimoCierre != null && (DateTime.Now - ultimoCierre.FechaRegistro).TotalHours > 24)
                return new ResultadoOperacion(false, "No se puede reabrir: pasaron más de 24 horas desde el cierre.");

            if (string.IsNullOrWhiteSpace(motivo))
                return new ResultadoOperacion(false, "El motivo de reapertura es obligatorio.");

            var estadoEnProceso = await _context.EstadosQueja
                .FirstOrDefaultAsync(e => e.NombreEstado == ESTADO_EN_PROCESO);
            queja.ID_Estado = estadoEnProceso?.ID_Estado;
            await _context.SaveChangesAsync();

            await RegistrarHistorial(idQueja, idEmpleado, estadoEnProceso?.ID_Estado ?? 0,
                $"Reabierta. Motivo: {motivo}");

            await _context.SaveChangesAsync();
            return new ResultadoOperacion(true, "Queja reabierta exitosamente.");
        }

        // RN-NOT-05: Verificar SLA (para usar en background service o al consultar)
        public async Task<List<QuejaSLA>> ObtenerQuejasVencidasSLA()
        {
            var ahora = DateTime.Now;
            var prioridades = await _context.PrioridadesQueja.ToListAsync();
            var estados = await _context.EstadosQueja.ToListAsync();

            var quejasActivas = await _context.Quejas
                .Where(q => q.ID_Estado != null)
                .ToListAsync();

            var resultado = new List<QuejaSLA>();

            foreach (var q in quejasActivas)
            {
                var estado = estados.FirstOrDefault(e => e.ID_Estado == q.ID_Estado);
                if (estado?.NombreEstado == ESTADO_CERRADO || estado?.NombreEstado == ESTADO_RECHAZADO)
                    continue;

                var prioridad = prioridades.FirstOrDefault(p => p.ID_Prioridad == q.ID_Prioridad);
                var historial = await _context.HistorialQuejas
                    .Where(h => h.ID_Quejas == q.ID_Quejas)
                    .OrderBy(h => h.FechaRegistro)
                    .FirstOrDefaultAsync();

                if (historial == null || prioridad == null) continue;

                var tiempoTranscurrido = (ahora - historial.FechaRegistro).TotalMinutes;
                var minutosPrimeraRespuesta = GetMinutosPrimeraRespuesta(prioridad.NombrePrioridad);
                var minutosResolucion = GetMinutosResolucion(prioridad.NombrePrioridad);

                bool vencioPrimeraRespuesta = tiempoTranscurrido > minutosPrimeraRespuesta &&
                    (estado?.NombreEstado == ESTADO_NUEVO || estado?.NombreEstado == ESTADO_ASIGNADO);

                bool vencioResolucion = tiempoTranscurrido > minutosResolucion;

                if (vencioPrimeraRespuesta || vencioResolucion)
                {
                    resultado.Add(new QuejaSLA
                    {
                        ID_Queja = q.ID_Quejas,
                        Titulo = q.Quejas,
                        Estado = estado?.NombreEstado ?? "",
                        Prioridad = prioridad.NombrePrioridad,
                        MinutosTranscurridos = (int)tiempoTranscurrido,
                        VencioPrimeraRespuesta = vencioPrimeraRespuesta,
                        VencioResolucion = vencioResolucion
                    });
                }
            }

            return resultado;
        }

        // Helpers
        private List<string> GetTransicionesPermitidas(string estadoActual)
        {
            return estadoActual switch
            {
                ESTADO_NUEVO => new List<string> { ESTADO_ASIGNADO, ESTADO_RECHAZADO },
                ESTADO_ASIGNADO => new List<string> { ESTADO_EN_PROCESO, ESTADO_ASIGNADO },
                ESTADO_EN_PROCESO => new List<string> { ESTADO_RESUELTO, ESTADO_PENDIENTE_APROBACION, ESTADO_EN_PROCESO },
                ESTADO_PENDIENTE_APROBACION => new List<string> { ESTADO_EN_PROCESO, ESTADO_RESUELTO },
                ESTADO_RESUELTO => new List<string> { ESTADO_CERRADO, ESTADO_EN_PROCESO },
                ESTADO_CERRADO => new List<string> { ESTADO_EN_PROCESO },
                _ => new List<string>()
            };
        }

        private int GetMinutosPrimeraRespuesta(int prioridad)
        {
            return prioridad switch
            {
                1 => 180,    // Baja: 3 horas
                2 => 60,     // Media: 1 hora
                3 => 30,     // Alta: 30 minutos
                4 => 15,     // Crítica: 15 minutos
                _ => 60
            };
        }

        private int GetMinutosResolucion(int prioridad)
        {
            return prioridad switch
            {
                1 => 1440,   // Baja: 24 horas
                2 => 480,    // Media: 8 horas
                3 => 240,    // Alta: 4 horas
                4 => 120,    // Crítica: 2 horas
                _ => 480
            };
        }

        private async Task<HistorialQueja> RegistrarHistorial(decimal idQueja, decimal idEmpleado,
            decimal idEstado, string observaciones, decimal? idImagen = null)
        {
            var hist = new HistorialQueja
            {
                ID_Quejas = idQueja,
                ID_Empleado = idEmpleado,
                ID_Estado = idEstado,
                observaciones = observaciones?.Length > 10 ? observaciones.Substring(0, 10) : observaciones,
                ID_Imagen = idImagen ?? 0,
                Aprobado = false,
                FechaRegistro = DateTime.Now
            };

            _context.HistorialQuejas.Add(hist);
            return hist;
        }
    }

    public class QuejaSLA
    {
        public decimal ID_Queja { get; set; }
        public string Titulo { get; set; } = "";
        public string Estado { get; set; } = "";
        public int Prioridad { get; set; }
        public int MinutosTranscurridos { get; set; }
        public bool VencioPrimeraRespuesta { get; set; }
        public bool VencioResolucion { get; set; }
    }
}
