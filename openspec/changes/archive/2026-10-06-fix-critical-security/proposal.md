# Proposal

## Why

Un hallazgo CRÍTICO (C1): la clave secreta con la que se preparan las contraseñas (`Util:ClaveSecreta`) y el connection string están en el historial de git accesible públicamente — se puede descargar directamente desde commits antiguos de GitHub (`raw.githubusercontent.com/.../appsettings.json`). Cualquiera puede crackear los hashes BCrypt offline. Un segundo hallazgo CRÍTICO (C2): la página de prueba `/usuariostest` crea cuentas Empleado con rol Admin sin ninguna autenticación ni autorización, y el middleware solo valida la *presencia* de la cookie (una cookie falsa basta), por lo que es alcanzable de forma anónima.

## What Changes

- **Purgar los secretos del historial de git** en todas las ramas que contienen `appsettings.json` / `appsettings.Development.json` con la clave y el connection string: las ramas solicitadas `seguridad-1`, `seguridad`, `productos`, `hab-proveedor-razonSocial-Rep`, `categorias-abm`, `carga-productos`, **más `master` y `quejas`** (también contienen el secreto: 5 commits cada una — sin incluirlas el fix no sería efectivo). Implica historial reescrito + force-push en el remoto.
- **Rotar `Util:ClaveSecreta`** (la actual se considera comprometida) y sacarla del repositorio: pasa a variable de entorno (o user-secrets en dev), dejando solo un template sin secretos en el repo.
- **Migración de contraseñas existentes**: con el pepper nuevo, los hashes actuales no verifican. Ruta de transición: verificación dual (pepper nuevo, y como fallback el viejo solo durante la ventana de migración) con re-hasheo en el próximo login exitoso; sin re-hasheo, forzar restablecimiento de contraseña.
- **Eliminar la página `/usuariostest`** (`Components/Pages/Usuarios/D_Usuarios.razor`), que registra usuarios con `ID_Rol = 1` (Admin) sin `[Authorize]` y escribe contraseña + pepper por consola. **BREAKING**: la ruta deja de existir.
- **Denegación por defecto para páginas con ruta**: toda página enrutable exige autenticación (política fallback global o `[Authorize]` en cada página), de modo que una página nueva sin permisos explícitos nunca quede expuesta. Esto cierra la clase de vulnerabilidad de la que C2 es un caso, no solo el caso.

## Capabilities

### New Capabilities

- `security/secret-management`: Los secretos de la aplicación (pepper de contraseñas, connection string) no residen en el repositorio ni en su historial; provienen de configuración de entorno, y la verificación de contraseñas sigue funcionando tras rotar el pepper.
- `security/page-access`: Toda página enrutable de la aplicación exige autenticación por defecto; no existen páginas públicas salvo las explícitamente anónimas (login, error, denegado), y ninguna página crea cuentas ni realiza mutaciones sin sesión y permiso válidos.

### Modified Capabilities

(ninguna — el proyecto aún no tiene specs; todas las capacidades son nuevas)

## Impact

- **Git**: reescritura de historial en 8 ramas y force-push a `origin`; cualquier clone local deberá clonarse de nuevo. El repo remoto pasa a tratarse como comprometido en el pasado (rotar el pepper es obligatorio aunque se purgue).
- **Usuarios**: todos los empleados con contraseña existente requieren la ventana de migración dual o un restablecimiento.
- **Código**: `Program.cs` (fallback policy / middleware), `appsettings*.json` (secretos → template), `seguridad/PasswordHasher.cs` y `SeguridadSesion.IniciarSesion` (verificación dual), borrado de `Components/Pages/Usuarios/D_Usuarios.razor`.
- **Fuera de alcance** (quedan para cambios posteriores): los 6 hallazgos ALTOS — cookie sin HttpOnly (H1), brute-force (H2), sesiones inmortales (H3), DetailedErrors (H4), permisos `Quejas.*` sin aplicar (H5), IDOR (H6).
