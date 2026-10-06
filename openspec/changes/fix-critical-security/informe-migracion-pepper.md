# Informe de migraci&oacute;n de pepper (tarea 4.3)

Fecha: 2026-10-06

## M&eacute;todo

La migraci&oacute;n no se puede detectar desde el hash BCrypt almacenado (el pepper se
concatena con la contrase&ntilde;a antes de hashear, y el prefijo `$2a$10$/$2a$11$` no lo
revela), por lo que se verific&oacute; con dos se&ntilde;ales:

1. Los logs de la app emitidos por la verificaci&oacute;n dual (1.1):
   `Hash migrado a pepper nuevo para empleado <ID>`.
2. La lista de empleados activos en `Empleados.Empleados` (`Activo = 1`).

## Empleados activos (baseline previo a la rotaci&oacute;n)

| ID | Nombre  | Activo | Hash pre-rotaci&oacute;n (prefijo) | Migr&oacute; |
|----|---------|--------|----------------------------------|---------|
| 3  | Luis    | 1      | `$2a$10$ea9MVLsndqAGd...`        | S&iacute;    |
| 5  | LuisU   | 1      | `$2a$11$hipnkx3Dxg4ik...`        | S&iacute;    |

Hashes pre-rotaci&oacute;n completos: `%TEMP%\opencode\hashes-antes.txt`.

## Evidencia de migraci&oacute;n (log de la app)

```
Hash migrado a pepper nuevo para empleado 3.
Hash migrado a pepper nuevo para empleado 5.
```

Ambos logins fueron realizados manualmente en el navegador por el operador
(`http://localhost:5000/login`), con &eacute;xito.

## Resultado

**Empleados activos a&uacute;n sin migrar: ninguno (lista vac&iacute;a).**

Cobertura: 2 de 2 empleados activos (100%).

## Seguimiento (tarea 4.4)

Pendiente: eliminar `Util__ClaveSecretaAnterior` del entorno, reiniciar y
confirmar un login manual con una cuenta ya migrada.
