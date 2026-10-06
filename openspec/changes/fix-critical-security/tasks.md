# Tasks

## 1. Código: pepper en entorno y verificación dual (se despliega con el pepper viejo todavía activo)

- [x] 1.1 Modificar `PasswordHasher` para verificación dual: intenta con `Util:ClaveSecreta` (nuevo) y, si falla y existe `Util:ClaveSecretaAnterior`, intenta con ese y re-hashea con el nuevo; crear proyecto de tests `HotelTools.Tests` con tests que cubran: login con pepper nuevo, login con pepper viejo dentro de ventana (y re-hasheo), login con pepper viejo sin `ClaveSecretaAnterior` (rechazado), y contraseña incorrecta (no migra). Verificación: `dotnet test` en verde.
- [x] 1.2 Agregar validación de arranque en `Program.cs`: si `Util:ClaveSecreta` no está definida, el proceso se detiene con mensaje que nombra la variable faltante. Verificación: ejecutar sin la variable → arranque falla con ese mensaje; con la variable → la app inicia.
- [x] 1.3 Asegurar que solo quede versionado un `appsettings.json` de plantilla sin valores reales (pepper y connection string como placeholders) y que `.gitignore` cubra `appsettings*.json` reales. Verificación: `git ls-files "*appsettings*"` muestra únicamente el template y grep del valor actual del pepper sobre esos archivos no da coincidencias.

## 2. Código: eliminar `/usuariostest` y denegación por defecto

- [x] 2.1 Eliminar `Components/Pages/Usuarios/D_Usuarios.razor` (incluye el `Console.WriteLine` de contraseña+pepper). Verificación: `dotnet build` sin errores y grep de la ruta `usuariostest` sobre `Components/` sin coincidencias.
- [x] 2.2 Completar `[Authorize]` en toda página enrutable que falte (`Home`, `Counter` o su eliminación como sobra de plantilla), manteniendo como anónimas solo `/login`, `/Error` y `/accesoDenegado`. Verificación: script/grep que lista cada `@page` y confirma que todas tienen `[Authorize]` o están en la allowlist — cero violaciones.
- [x] 2.3 Agregar FallbackPolicy `RequireAuthenticatedUser` en `Program.cs` y cambiar el middleware para validar el token contra la BD (existe, activo, no expirado) en lugar de solo presencia, conservando la allowlist de prefijos estáticos. Verificación: con la app corriendo, una cookie forjada (`document.cookie = "HotelTools=falso"`) redirige a `/login` en ruta protegida. Nota: el FallbackPolicy retaba también al endpoint de `/login` (bucle de redirección); se añadió `@attribute [AllowAnonymous]` a `Components/Login.razor`. Verificado con curl: `/login` → 200 (anónimo y cookie forjada), `/listaquejas` y `/home` → 302 `/login` (anónimo y cookie forjada).
- [x] 2.4 Convertir la auditoría de 2.2 en un comando documentado (script de una línea) y agregarlo al repo para re-ejecutarlo cuando aparezcan páginas nuevas. Verificación: el comando se ejecuta y sale con código 0 en el estado actual.

## 3. Purga del historial en las 8 ramas (se ejecuta después del merge de los grupos 1-2)

- [ ] 3.1 Crear un mirror de respaldo del remoto (`git clone --mirror`) y verificar que contiene las 8 ramas objetivo (`seguridad-1`, `seguridad`, `productos`, `hab-proveedor-razonSocial-Rep`, `categorias-abm`, `carga-productos`, `master`, `quejas`). Verificación: `git branch -l` dentro del mirror lista las 8.
- [ ] 3.2 Registrar el baseline del hallazgo: por cada rama, `git rev-list --count <rama> -- appsettings.json appsettings.Development.json` y localizar los commits exactos que contienen el pepper y el connection string. Verificación: reporte de los 8 conteos (4-5 commits por rama) guardado en el directorio del cambio.
- [ ] 3.3 Ejecutar `git filter-repo --replace-text` en el mirror con reglas para el valor exacto del `ClaveSecreta` y del connection string, forzando la reescritura de todas las ramas (con fetch explícito de `origin/seguridad-1`, que solo existe en el remoto). Verificación: dentro del mirror, `git grep <valor-del-pepper> $(git rev-list --all)` no devuelve ningún commit.
- [ ] 3.4 Force-push de las 8 ramas reescritas a `origin`. Verificación: clon limpio del remoto (`git clone` nuevo) + grep sobre todo su historial sin coincidencias del pepper ni del connection string.
- [ ] 3.5 Escaneo de secretos post-purga con `gitleaks` (o grep equivalente) sobre todas las ramas del remoto y sobre el árbol actual. Verificación: reporte del escaneo sin hallazgos.

## 4. Rotación del pepper y migración de usuarios (requiere código de los grupos 1-2 desplegado)

- [ ] 4.1 Generar el pepper nuevo y definir en el entorno de despliegue `Util__ClaveSecreta` (nuevo) y `Util__ClaveSecretaAnterior` (viejo) **antes** de reiniciar la aplicación. Verificación: la app arranca sin el error del paso 1.2 y una consulta de login de prueba resuelve con verificación dual.
- [ ] 4.2 Login de prueba con una cuenta existente (hash del pepper viejo): debe autenticar y el hash almacenado en BD debe cambiar (re-hasheado con el pepper nuevo). Verificación: comparar el valor de `Empleados.Password` antes y después del login.
- [ ] 4.3 Monitorear hasta que todos los empleados activos hayan iniciado sesión (logs de migración emitidos por 1.1 vs empleados activos en BD). Verificación: reporte que lista los empleados activos aún sin migrar = vacío.
- [ ] 4.4 Eliminar `Util__ClaveSecretaAnterior` del entorno y reiniciar (cierre de la ventana). Verificación: los tests de 1.1 cubren el rechazo del pepper viejo sin la variable; un login manual de una cuenta migrada sigue funcionando.
- [ ] 4.5 Documentar el runbook de operación en `docs/seguridad-pepper.md`: cómo definir las variables de entorno, orden de rotación, cierre de ventana y rollback (volver el pepper viejo a `Util__ClaveSecreta`). Verificación: el documento existe y su procedimiento de rollback coincide con el Migration Plan del design.md.

## 5. Verificación final (integración)

- [ ] 5.1 `dotnet build` + `dotnet test` completos sin errores. Verificación: salida en 0 errores y tests en verde.
- [ ] 5.2 Prueba manual con la app corriendo: anónimo redirigido a `/login` en ruta protegida; cookie forjada no da acceso; `/usuariostest` ya no existe; login normal funciona con el pepper nuevo. Verificación: checklist completado con capturas o notas en el directorio del cambio.
- [ ] 5.3 Re-ejecutar el escaneo de secretos (3.5) y la auditoría de páginas (2.4) sobre el estado final. Verificación: ambos comandos en 0 hallazgos.
