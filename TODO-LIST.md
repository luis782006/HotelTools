# TODO-LIST - HotelTools

## Alta Prioridad

- [ ] **Implementar validación de formularios** en componentes Blazor - Varios componentes usan `MudCheckBox`, `MudSelect` sin validación completa
- [ ] **Crear tests unitarios** para los servicios de seguridad (`AuthServices`, `SeguridadSesion`, `PasswordHasher`)
- [x] **Agregar migraciones de base de datos** - Baseline `20260820140016_InitialCreate` generado y marcado como aplicado en la BD existente (sin ejecutar DDL, datos preservados)
- [x] **Limitar sesiones concurrentes** - Máximo 2 sesiones activas por empleado; un 3er login cierra todas las sesiones del usuario (`SeguridadSesion.IniciarSesion`)

## Media Prioridad

- [ ] **Refactorizar DynamicAuthorizationPolicyProvider** - Actualmente usa claim "Permiso" hardcodeado; debería ser más dinámico
- [ ] **Agregar logging estructurado** - Ya hay Serilog configurado, pero faltaría correlación de logs con ID de sesión
- [ ] **Implementar exportación de reportes** - PDF/Excel para inventario, movimientos de producto
- [ ] **Crear endpoint API REST** - Para consumir desde aplicaciones móviles o frontend separado
- [ ] **Añadir manejo de errores global** - Middleware para capturar y formatear errores consistentes

## Baja Prioridad / Mejora UI

- [ ] **Mejorar accesibilidad** - Validar contrastes, labels en componentes MudBlazor, focus management
- [ ] **Temas adicionales** - Beyond the default MudBlazor theme
- [ ] **Animaciones y transiciones** - Mejorar UX en diálogos y transiciones de página
- [ ] **Responsive design** - Verificar en móvil/tablet todas las páginas
- [ ] ** Internacionalización** - Agregar recursos para español/inglés

## Pendientes del Branch Fusionado

- [ ] **Probar flujo de carga de productos por paquetes** - El nuevo código en `CargaProductos.razor` necesita testing
- [ ] **Validar relaciones FK en base de datos** - Los nuevos modelos (PaqueteProducto, TipoProducto) need DB validation
- [ ] **Revisar constraints en Entity Framework** - Algunos modelos tienen propiedades sin configuración completa
- [ ] **Actualizar documentación SQL** - Los scripts `limpiar_esquema_*.sql` need review

## Ideas Futuras

- [ ] **Mobile app** - PWA o aplicación nativa complementaria
- [ ] **Sistema de notificaciones** - Blazor Server notification system
- [ ] **Integración con proveedores externos** - API de proveedores, sincronización de inventario
- [ ] **Panel de estadísticas** - Gráficos de ocupación, rotación de productos, ventas
- [ ] **Sistema de turnos** - Para gestión de empleados