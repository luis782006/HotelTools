# Checklist de verificación manual — tarea 5.2

Fecha: 2026-10-06 — app corriendo en `http://localhost:5000` (Development, pepper nuevo,
`Util__ClaveSecretaAnterior` eliminada del entorno).

Verificaciones con `curl` (sin sesión / cookie forjada) y prueba manual en navegador
(realizada por el operador).

| # | Prueba | Esperado | Resultado |
|---|--------|----------|-----------|
| 1 | Anónimo → `/listaquejas` | 302 a `/login` | ✅ `302 http://localhost:5000/login` |
| 2 | Anónimo → `/home` | 302 a `/login` | ✅ `302 http://localhost:5000/login` |
| 3 | Anónimo → `/login` | 200 (página accesible) | ✅ `200` |
| 4 | Cookie forjada (`HotelTools=falso`) → `/listaquejas` | 302 a `/login` (sin acceso) | ✅ `302 http://localhost:5000/login` |
| 5 | Cookie forjada → `/usuariostest` | sin acceso | ✅ `302 http://localhost:5000/login` |
| 6 | `/usuariostest` ya no existe (ruta borrada en 2.1) | con sesión válida → 404 | ✅ Confirmado: el archivo `D_Usuarios.razor` no existe (`git ls-files` = 0) y el script 2.4 da exit 0; con sesión válida la ruta no tiene endpoint → 404 |
| 7 | Login normal con pepper nuevo | autentica | ✅ Operador: login de **Luis** y **LuisU** OK; tras cerrar la ventana (4.4) login de **Luis** OK sin la variable vieja |
| 8 | Assets del circuito (`/_framework/blazor.web.js`, `/_content/MudBlazor/*`) | 200 (no HTML) | ✅ 200 `text/javascript` (fix de `BlazorCircuitResultHandler` para `/_framework`) |
| 9 | `/_blazor/negotiate` anónimo (la login no tiene prerender) | 200 | ✅ `200` |

## Evidencia de migración de pepper (4.2/4.3)

- Log de la app: `Hash migrado a pepper nuevo para empleado 3` y `... para empleado 5`.
- Empleados activos: 2 (Luis=3, LuisU=5); pendientes de migrar: **ninguno**.

## Comandos de las verificaciones automatizadas

```powershell
# 5.3 — escaneo de secretos (árbol) con baseline de los .vs/ conocidos
gitleaks dir --no-banner --redact --baseline-path $env:TEMP\opencode\gitleaks-baseline.json .   # no leaks found, exit 0

# 5.3 — auditoría de páginas @page con autorización
powershell -File scripts\check-pages-auth.ps1                                                    # exit 0
```

Resultado: ambos en **0 hallazgos / exit 0**.
