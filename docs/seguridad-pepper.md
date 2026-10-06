# Runbook: rotación del pepper de contraseñas (`Util__ClaveSecreta`)

Este documento describe la operación rutinaria del pepper (clave secreta con la que se
hashean las contraseñas: `BCrypt(password + pepper)`) y su rotación. Coherente con el
Migration Plan de `openspec/changes/fix-critical-security/design.md`.

## Variables de entorno

| Variable | Significado |
|---|---|
| `Util__ClaveSecreta` | Pepper **vigente**. Obligatoria: sin ella la app no arranca (validación en `Program.cs`). |
| `Util__ClaveSecretaAnterior` | Pepper **anterior**. Solo durante la ventana de migración; permite verificar hashes viejos y re-hashear en el primer login. |
| `ConnectionStrings__Hotel_Tools` | Cadena de conexión a la BD (también fuera del repo). |

Se definen **a nivel de usuario/servidor** (no en `appsettings*.json`, que son plantillas
sin secretos y están parcialmente trackeados).

```powershell
# PowerShell (usuario)
[Environment]::SetEnvironmentVariable('Util__ClaveSecreta', '<nuevo>', 'User')
[Environment]::SetEnvironmentVariable('Util__ClaveSecretaAnterior', '<viejo>', 'User')
```

En despliegue (servicio/IIS/Docker), definirlas como variables de entorno del proceso y
**reiniciar la aplicación** para que apliquen.

## Orden de rotación

1. **Requisito previo:** el código con verificación dual (grupo 1 del cambio) debe estar
   desplegado y probado con el pepper viejo todavía vigente.
2. **Generar el pepper nuevo** con RNG criptográfico, por ejemplo:
   ```powershell
   $rng = [Security.Cryptography.RandomNumberGenerator]::Create()
   $bytes = New-Object byte[] 48
   $rng.GetBytes($bytes)
   $nuevo = [Convert]::ToBase64String($bytes)   # 64 caracteres
   ```
   Guardarlo **fuera del repositorio** (gestor de secretos / archivo local excluido de git).
3. **Definir** `Util__ClaveSecreta` = nuevo y `Util__ClaveSecretaAnterior` = viejo
   **antes** de reiniciar.
4. **Reiniciar** la app. Verificar: arranca sin el error de `Util:ClaveSecreta` y `/login`
   responde 200.
5. **Login de prueba** con una cuenta existente (hash hecho con el pepper viejo):
   debe autenticar y el hash en `Empleados.Empleados.Password` debe cambiar
   (re-hasheo con el nuevo). En los logs aparece:
   `Hash migrado a pepper nuevo para empleado <ID>.`
6. **Monitorear** hasta que todos los empleados activos hayan iniciado sesión:
   ```sql
   SELECT ID_Empleado, Nombre FROM Empleados.Empleados WHERE Activo = 1;
   ```
   contra los IDs con log de migración. Si queda alguno sin migrar, la ventana sigue abierta.

## Cierre de la ventana

1. Eliminar `Util__ClaveSecretaAnterior` del entorno.
2. Reiniciar la app.
3. Verificar: un login manual de una cuenta **ya migrada** sigue funcionando; un hash
   viejo sin la variable es rechazado (cubierto por `HotelTools.Tests`).

Sin la variable, el código ni siquiera considera el pepper viejo: la migración queda
cerrada de forma segura.

## Rollback

Si la verificación dual falla en producción (nadie puede entrar):

1. Volver a definir `Util__ClaveSecreta` = **pepper viejo** (y quitar
   `Util__ClaveSecretaAnterior` si aporta ruido).
2. Reiniciar. Los hashes originales siguen siendo válidos: no se pierden datos.
3. Los hashes ya migrados con el pepper nuevo dejarán de verificar → esas cuentas
   quedan temporalmente sin login hasta re-definir el pepper nuevo (ambos valores deben
   conservarse durante varios días tras la rotación).

Contingencia si una cuenta quedó sin poder entrar: restablecimiento de contraseña por
administrador (fuera de alcance de este cambio).

## Higiene asociada

- Los secretos viejos usados para la purga del historial (`git filter-repo --replace-text`)
  viven fuera del repo; borrarlos cuando la ventana de soporte histórico haya pasado.
- El connection string filtrado también rotó de exposición: se recomienda cambiar la
  contraseña de la BD como parte de la rotación completa.
- `refs/pull/*` de GitHub conserva commits viejos (no reescribibles con push); ver
  `openspec/changes/fix-critical-security/informe-escaneo-secretos.md`.
