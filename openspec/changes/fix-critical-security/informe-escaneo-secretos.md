# Informe de escaneo de secretos post-purga (tarea 3.5)

- Fecha: 2026-10-06
- Herramienta: `gitleaks 8.30.1` (winget) + verificación propia (pickaxe `-S` y `git grep -F` sobre todas las formas, cruda y JSON-escaped)
- Valores buscados: pepper (`PEPPER-1`, 47 chars) y los 2 connection strings distintos (`CONN-1`, `CONN-2`) — mapeo etiqueta→valor solo en el archivo de secretos fuera del repo.

## 1. Todas las ramas del remoto

- Clon limpio: `git clone https://github.com/luis782006/HotelTools.git` (9 ramas).
- `gitleaks git <clon> --log-opts=--all`: **83 commits escaneados, no leaks found (exit 0)**.
- Verificación propia sobre el mismo clon: pickaxe de los 3 valores (formas cruda + escapada) = **0 commits**; `git grep -F` en todas las puntas = **0 archivos**.
- Reporte JSON: `%TEMP%\opencode\gitleaks-remoto.json` (vacío; fuera del repo a propósito).

## 2. Árbol actual (repo de trabajo)

- `gitleaks dir <repo>`: 2 hallazgos `generic-api-key` en `.vs/HotelTools/config/applicationhost.config`.
  - Ese archivo **no está versionado** (`.vs/` está en `.gitignore` línea 42; `git ls-files .vs/` = 0). Es un artefacto local de Visual Studio, fuera del alcance del repo.
- Re-ejecución con `--baseline-path` (los 2 hallazgos conocidos ignorados): **no leaks found (exit 0)**.

## 3. Residuos conocidos (documentados, fuera del alcance del purge)

1. **`refs/pull/1|2|3/head`** en GitHub: apuntan a commits viejos con secretos (pepper en los 3, conn en los 3). GitHub no permite reescribir estos refs con push. Opciones: pedir a GitHub Support su eliminación, o aceptar el residuo sabiendo que:
   - la rotación de pepper (grupo 4) invalida el pepper;
   - se recomienda rotar también la contraseña de la BD (connection string) — queda como recomendación fuera de alcance.
2. **Ramas locales sin remoto** `cargaProductosPaquetes` y `prestamoHabitacion`: aún apuntan a la historia vieja (con secretos) en el repo local. No deben hacerse push sin reescribirse antes (mismo `filter-repo --replace-text` con las reglas del TEMP) o, si están obsoletas, eliminarse.

## 4. Verificación intermedia del mirror (tarea 3.3)

- `git filter-repo --replace-text` sobre el mirror: 90 commits reescritos.
- Control positivo: 18 archivos en las 9 ramas contienen el marcador `PELIGRO-ELIMINADO` → el reemplazo se aplicó.
- Pickaxe + grep posteriores: 0 hallazgos.
