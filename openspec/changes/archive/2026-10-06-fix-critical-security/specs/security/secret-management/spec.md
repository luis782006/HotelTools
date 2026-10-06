# Spec Delta

## Purpose

Mantiene los secretos de la aplicación (pepper de contraseñas, connection string) fuera del repositorio y de su historial, y garantiza que la autenticación siga funcionando de forma controlada después de rotar el pepper.

## ADDED Requirements

### Requirement: Sin secretos en el repositorio ni en su historial
Ningún commit de ninguna rama del repositorio SHALL contener el valor de `Util:ClaveSecreta` ni el connection string completo de la base de datos.

#### Scenario: Historial limpio en todas las ramas
- **WHEN** se escanea el historial de las ramas `seguridad-1`, `seguridad`, `productos`, `hab-proveedor-razonSocial-Rep`, `categorias-abm`, `carga-productos`, `master` y `quejas` en el remoto
- **THEN** ningún commit contiene el valor del pepper ni el connection string

#### Scenario: Archivos de configuración en el repositorio sin secretos
- **WHEN** se inspecciona cualquier `appsettings*.json` versionado en el repositorio
- **THEN** el pepper y el connection string no aparecen con sus valores reales (solo placeholders o ausentes)

### Requirement: El pepper proviene de configuración de entorno
La aplicación SHALL obtener `Util:ClaveSecreta` de configuración externa al repositorio (variable de entorno u otro mecanismo fuera de git) y SHALL fallar de forma visible al iniciar si no está definida.

#### Scenario: Pepper no configurado
- **WHEN** la aplicación inicia sin la variable `Util:ClaveSecreta` definida
- **THEN** la aplicación se detiene con un error que indica la variable faltante, sin quedar operativa con un pepper vacío o por defecto

#### Scenario: Pepper configurado
- **WHEN** la aplicación inicia con `Util:ClaveSecreta` definida en el entorno
- **THEN** el hash y la verificación de contraseñas usan ese valor

### Requirement: Rotación del pepper comprometido
El pepper efectivamente en uso SHALL diferir del valor que fue comprometido en el historial público; el valor antiguo SHALL dejar de ser válido para verificar contraseñas al cerrar la ventana de migración.

#### Scenario: Pepper antiguo rechazado tras la migración
- **WHEN** se intenta verificar una contraseña usando el pepper antiguo comprometido después de cerrar la ventana de migración
- **THEN** la verificación falla

### Requirement: Verificación de contraseñas tras la rotación
Después de rotar el pepper, los usuarios con hashes generados con el pepper anterior SHALL poder autenticarse durante la ventana de migración, y su contraseña SHALL quedar re-hasheada con el nuevo pepper en el mismo login.

#### Scenario: Login exitoso durante la ventana de migración
- **WHEN** un usuario se autentica con la contraseña correcta y su hash fue generado con el pepper antiguo, dentro de la ventana de migración
- **THEN** el login tiene éxito y la contraseña almacenada se re-hashea con el pepper nuevo

#### Scenario: Login con hash ya migrado
- **WHEN** un usuario se autentica con la contraseña correcta y su hash ya usa el pepper nuevo
- **THEN** el login tiene éxito sin usar el pepper antiguo

#### Scenario: Ventana de migración cerrada
- **WHEN** un usuario con hash del pepper antiguo se autentica después de cerrar la ventana de migración
- **THEN** el login falla y el usuario debe restablecer su contraseña

#### Scenario: Contraseña incorrecta no migra el hash
- **WHEN** un usuario se autentica con una contraseña incorrecta
- **THEN** el login falla y el hash almacenado no se modifica
