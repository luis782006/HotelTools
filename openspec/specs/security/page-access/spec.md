# security/page-access Specification

## Purpose

Establece la denegación por defecto del acceso a páginas enrutables: toda página exige sesión autenticada salvo que se declare anónima, y ninguna página realiza operaciones sensibles (como crear cuentas) sin sesión y permiso válidos.

## Requirements

### Requirement: Denegación por defecto para páginas enrutables
Toda página enrutable SHALL exigir una sesión autenticada; solo las páginas explícitamente declaradas como anónimas (login, error y acceso denegado) SHALL ser accesibles sin sesión.

#### Scenario: Acceso anónimo a una página protegida
- **WHEN** un usuario sin sesión válida navega a cualquier ruta protegida (por ejemplo `/listaquejas` o `/usuariostest`)
- **THEN** se le redirige a `/login` y la página no se renderiza

#### Scenario: Página nueva sin marca de autorización
- **WHEN** se agrega una página enrutable nueva sin declarar permisos específicos
- **THEN** sigue exigiendo autenticación: un anónimo no puede verla

#### Scenario: Página anónima explícita
- **WHEN** un usuario sin sesión navega a `/login`
- **THEN** la página se renderiza normalmente

### Requirement: Sesión forjada no concede acceso
Una cookie de sesión inventada o manipulada por el cliente SHALL otorgar los mismos derechos que no tener sesión: sin claims de usuario ni de permisos.

#### Scenario: Cookie con token inexistente
- **WHEN** el cliente envía una cookie de sesión con un token que no corresponde a una sesión activa y vigente
- **THEN** la petición se trata como anónima y el acceso a páginas protegidas se deniega

### Requirement: Creación de usuarios solo con sesión y permiso
Las páginas o diálogos que crean cuentas de empleado SHALL exigir sesión autenticada y el permiso de administración de usuarios; ninguna operación de alta de usuario SHALL estar disponible de forma anónima.

#### Scenario: Acceso anónimo a creación de usuario
- **WHEN** un usuario sin sesión (o con cookie forjada) intenta acceder a una página de alta de usuarios
- **THEN** el acceso se deniega y no se crea ninguna cuenta

#### Scenario: Sesión sin permiso de administración de usuarios
- **WHEN** un usuario autenticado sin el permiso de administración de usuarios intenta crear una cuenta
- **THEN** el acceso se deniega

### Requirement: Páginas de prueba no publicables como rutas
No SHALL existir en la aplicación ninguna página enrutable de prueba o desarrollo que realice operaciones reales sobre datos sin los controles de autorización.

#### Scenario: Eliminación de la página de prueba de usuarios
- **WHEN** se solicita la ruta `/usuariostest`
- **THEN** la ruta ya no existe en la aplicación y no hay página asociada

#### Scenario: Sin salida sensible por consola
- **WHEN** se ejecuta cualquier página o servicio de la aplicación
- **THEN** no se escribe por consola ni a archivos de log ninguna contraseña, pepper o token de sesión
