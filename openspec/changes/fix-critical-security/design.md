# Design

## Context

El repo remoto (`github.com/luis782006/HotelTools`) es público y el historial de 8 ramas contiene `appsettings.json` con `Util:ClaveSecreta` (el pepper BCrypt) y el connection string. Los archivos fueron quitados del árbol actual (`.gitignore`), pero los commits viejos siguen descargables. En paralelo, `Components/Pages/Usuarios/D_Usuarios.razor` publica `/usuariostest`, que crea Empleados con `ID_Rol = 1` sin `[Authorize]`, y el middleware de `Program.cs:108-132` solo comprueba la *presencia* de la cookie, no su validez. Ver proposal.md para motivación y specs para los contratos de comportamiento.

Restricciones: las contraseñas existentes son hashes `BCrypt(password + pepper)` — cambiar el pepper los invalida todos, y no existe flujo de auto-restablecimiento de contraseña en la app.

## Goals / Non-Goals

**Goals:**
- Eliminar el pepper y el connection string de todo el historial de las 8 ramas afectadas.
- Rotar el pepper sin dejar a los empleados sin acceso (migración de hashes).
- Eliminar `/usuariostest` y establecer denegación por defecto para toda ruta.
- Verificación comprobable: escaneo de historial limpio + auditoría de que toda página enrutable exige sesión.

**Non-Goals:**
- Los 6 hallazgos ALTOS del informe (cookie HttpOnly, brute-force, sesiones inmortales, DetailedErrors, permisos `Quejas.*`, IDOR) — cambios posteriores.
- Migrar a Azure Key Vault o Secret Manager (ver Open Questions).
- Reescribir el esquema de autenticación completo (se conserva el scheme cookie actual).

## Decisions

### D1. Purga del historial con `git filter-repo` + force-push, y rotación del pepper igualmente obligatoria
**Decisión:** usar `git filter-repo --replace-text` sobre un mirror clone, reescribiendo las 8 ramas (`seguridad-1`, `seguridad`, `productos`, `hab-proveedor-razonSocial-Rep`, `categorias-abm`, `carga-productos`, `master`, `quejas`), y forzar el push a `origin`. El pepper se rota **aunque la purga tenga éxito**, porque el viejo ya estuvo público (y puede quedar en cachés de GitHub, forks y clones locales).
**Alternativas consideradas:**
- *Solo hacer el repo privado, sin purgar*: rechazado — el historial ya fue accesible; privarlo no borra lo descargado.
- *BFG Repo-Carrier*: equivalente, pero `git filter-repo` es la herramienta mantenida y recomendada; se usa la que esté disponible.
**Nota:** `seguridad-1` solo existe en el remoto — debe hacerse fetch explícito para reescribirla.

### D2. Pepper en variable de entorno con falla al arranque
**Decisión:** `Util__ClaveSecreta` como variable de entorno (ASP.NET Core mapea `__` a `:`). `appsettings.json` versionado queda solo con placeholder. Validación temprana en `Program.cs`: si la variable no está definida, el proceso se detiene con mensaje claro (evita arrancar con pepper vacío).
**Alternativas:** user-secrets (solo apto para dev, no para el servidor); Key Vault (dependencia externa nueva — fuera de alcance).

### D3. Rotación con verificación dual y re-hasheo en login
**Decisión:** `PasswordHasher` verifica primero con el pepper nuevo (`Util__ClaveSecreta`) y, si falla y está definido `Util__ClaveSecretaAnterior`, verifica con el viejo; en caso de éxito con el viejo, re-hashea con el nuevo y guarda. La ventana de migración se cierra **eliminando la variable `Util__ClaveSecretaAnterior`** del entorno: sin ella, el código ni siquiera considera el pepper viejo.
**Alternativas consideradas:**
- *Forzar restablecimiento de todos los passwords*: rechazado — no existe flujo de restablecimiento en la app; habría que crearlo completo (queda como opción de contingencia si la ventana se cierra con usuarios sin migrar).
- *Script de migración*: imposible — no se puede re-hashear sin la contraseña en texto plano.

### D4. Borrado completo de `/usuariostest`, no solo protegerla
**Decisión:** eliminar `Components/Pages/Usuarios/D_Usuarios.razor`. La página es un formulario de prueba que además hace `Console.WriteLine(password + pepper)`, colisiona de nombre con el diálogo real `D_Usuario.razor`, y su lógica de alta ya existe en `RegistroUsuarios`/`D_Usuario`.
**Alternativa:** agregarle `[Authorize]` — rechazado: código muerto que solo aporta riesgo.

### D5. Denegación por defecto en tres capas
**Decisión:** combinar tres mecanismos porque la navegación client-side de Blazor no pasa por el middleware:
1. **Middleware (`Program.cs`)**: validar el token contra la BD (presencia **y** validez: existe, activo, no expirado), no solo presencia — cierra el escenario de cookie forjada. Allowlist explícita: `/login`, `/logout`, `/Error`, `/accesoDenegado`, prefijos estáticos.
2. **`[Authorize]` explícito en toda página enrutable** + **FallbackPolicy** (`RequireAuthenticatedUser`) como red de seguridad para páginas nuevas.
3. **`Routes.razor`**: `AuthorizeRouteView` ya deniega rutas con `[Authorize]`; el fallback cubre las que no lo tengan.
**Verificación:** tarea de auditoría grep que lista todo `@page` sin `[Authorize]`/`[AllowAnonymous]` y falla el build si aparece uno nuevo fuera de la allowlist.
**Riesgo conocido:** en Blazor Server la cobertura del FallbackPolicy depende de la versión/endpoint metadata; por eso la capa 2 (atributos explícitos) es la garantía primaria y el fallback es la defensa adicional.

### D6. Orden de operaciones: código primero, secreto después
La rotación del pepper solo puede ocurrir **después** de desplegar el código con verificación dual; la purga del historial puede hacerse en paralelo pero antes de publicar el nuevo pepper en cualquier lado. El orden detallado está en el Migration Plan.

## Risks / Trade-offs

- **Force-push rompe clones locales de otros desarrolladores** → anunciar el cambio; todos deben clonar de nuevo desde cero. Hacer un mirror de respaldo (`git clone --mirror`) antes de reescribir.
- **La purga no borra forks, cachés de GitHub ni clones ya descargados** → por eso la rotación del pepper es obligatoria y no condicional a la purga.
- **Ventana de migración demasiado corta** → usuarios que no ingresen en el período quedan sin login → mitigación: mantener `Util__ClaveSecretaAnterior` hasta confirmar por logs/BD que todos migraron; contingencia: restablecimiento por administrador (fuera de alcance del cambio, documentada).
- **Bug en la verificación dual = nadie puede entrar** → probar con una cuenta conocida antes de cerrar la ventana; rollback: redefinir el pepper viejo como `Util__ClaveSecreta` (los hashes originales siguen válidos, no se pierden datos).
- **La falla de arranque por variable faltante puede tumbar un despliegue** → configurar las variables en el entorno **antes** de actualizar la aplicación; documentado en el plan.
- **`/logout` en la allowlist** debe manejar cookie ausente sin error (hoy ya pasa).

## Migration Plan

1. **Respaldo**: `git clone --mirror` del remoto (punto de restauración).
2. **Verificar alcance**: `git rev-list --all -- appsettings.json` por rama (baseline ya medido: 4-5 commits por rama).
3. **Desplegar código con verificación dual** (PasswordHasher con `Util__ClaveSecretaAnterior`, validación de arranque, borrado de `/usuariostest`, capas de denegación por defecto) — con el pepper **viejo** todavía como principal.
4. **Purgar historial**: `git filter-repo --replace-text` con reglas para el valor exacto del pepper y del connection string, sobre las 8 ramas; force-push de todas (incluida `origin/seguridad-1`).
5. **Rotar**: generar pepper nuevo; en el servidor definir `Util__ClaveSecreta` = nuevo y `Util__ClaveSecretaAnterior` = viejo; reiniciar.
6. **Migrar usuarios**: cada login exitoso re-hashea con el pepper nuevo. Monitorear (log de migraciones) hasta que todos los empleados activos hayan ingresado.
7. **Cerrar ventana**: eliminar `Util__ClaveSecretaAnterior` del entorno; reiniciar.
8. **Verificación final**: escaneo de historial (sin secretos), `gitleaks`/grep en todas las ramas, auditoría de `@page` sin autorización, login funcional con cuenta de prueba.

**Rollback:** respaldo mirror (restaurar ramas si la purga falla); en producción, volver el pepper viejo a `Util__ClaveSecreta` (los hashes de los usuarios no migrados siguen siendo válidos); el código desplegado es compatible en ambas direcciones mientras exista la ventana dual.

## Open Questions

- **Mecanismo de variable de entorno en producción** (servicio Windows, IIS o contenedor): no cambia specs ni desglose — la tarea es "definir las variables en el entorno de despliegue", se concreta al ejecutar.
- **Si el repo remoto debe quedarse privado** como defensa adicional tras la purga: decisión operativa del propietario, no afecta los requisitos (el spec exige historial limpio, no visibilidad del repo).
