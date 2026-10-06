# Auditoría de denegación por defecto: toda página enrutable (@page) debe tener
# [Authorize] o estar en la allowlist de páginas anónimas.
# Uso:  powershell -File scripts/check-pages-auth.ps1
# Sale con código 0 si no hay violaciones; 1 si las hay.
param(
    [string]$Root = (Join-Path $PSScriptRoot "..")
)

$allowlist = @("/login", "/Error", "/accesoDenegado")
$violaciones = @()

Get-ChildItem -Path (Join-Path $Root "Components") -Recurse -Filter *.razor | ForEach-Object {
    $contenido = Get-Content $_.FullName -Raw
    if ($contenido -match '@page\s+"([^"]+)"') {
        $ruta = $Matches[1]
        if ($ruta -notin $allowlist -and $contenido -notmatch '\[Authorize') {
            $violaciones += "$($ruta) -> $($_.FullName)"
        }
    }
}

if ($violaciones.Count -eq 0) {
    Write-Output "OK: toda página enrutable exige autorización (allowlist: $($allowlist -join ', '))."
    exit 0
}

Write-Output "VIOLACIONES (páginas enrutable sin [Authorize]):"
$violaciones | ForEach-Object { Write-Output "  $_" }
exit 1
