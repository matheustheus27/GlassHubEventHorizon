<#
.SYNOPSIS
    Pipeline de Build e Publicação Automatizada do GlassHub EventHorizon (GUI + CLI).
.DESCRIPTION
    Compila os binários em modo Release para a arquitetura win-x64, otimizados como
    Single-File autocontido (Self-Contained), prontos para execução e geração do instalador Inno Setup.
#>

param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64",
    [string]$OutputDir = "$PSScriptRoot\publish\win-x64",
    [switch]$NoInnoSetup = $false
)

$ErrorActionPreference = "Stop"

Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host "       🚀 GlassHub EventHorizon - Pipeline de Publicação        " -ForegroundColor DarkCyan
Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host " Configuração : $Configuration" -ForegroundColor Gray
Write-Host " Runtime Alvo : $Runtime" -ForegroundColor Gray
Write-Host " Saída        : $OutputDir" -ForegroundColor Gray
Write-Host "-----------------------------------------------------------------" -ForegroundColor DarkGray

# 1. Limpeza do diretório de saída
if (Test-Path $OutputDir) {
    Write-Host "[1/5] Limpando diretório de publicação anterior..." -ForegroundColor Yellow
    Remove-Item -Path $OutputDir -Recurse -Force | Out-Null
}
New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null

# 2. Sincronização de Ícones e Assets
Write-Host "[2/5] Verificando e sincronizando assets e ícones..." -ForegroundColor Yellow
$guiAssets = "$PSScriptRoot\src\GlassHub.EventHorizon.GUI\Assets"
if (-not (Test-Path $guiAssets)) {
    New-Item -ItemType Directory -Path $guiAssets -Force | Out-Null
}
Copy-Item "$PSScriptRoot\assets\app.ico" -Destination "$guiAssets\app.ico" -Force -ErrorAction SilentlyContinue
Copy-Item "$PSScriptRoot\assets\logo.png" -Destination "$guiAssets\logo.png" -Force -ErrorAction SilentlyContinue
Copy-Item "$PSScriptRoot\assets\logo.svg" -Destination "$guiAssets\logo.svg" -Force -ErrorAction SilentlyContinue

# 3. Publicação do GlassHub GUI
Write-Host "[3/5] Publicando GlassHub EventHorizon GUI (WPF Windows 11)..." -ForegroundColor Yellow
$guiProject = "$PSScriptRoot\src\GlassHub.EventHorizon.GUI\GlassHub.EventHorizon.GUI.csproj"
dotnet publish $guiProject `
    -c $Configuration `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o $OutputDir

if ($LASTEXITCODE -ne 0) {
    Write-Error "Falha na publicação do GUI!"
    exit 1
}

# 4. Publicação do GlassHub CLI (evh)
Write-Host "[4/5] Publicando GlassHub EventHorizon CLI (evh)..." -ForegroundColor Yellow
$cliProject = "$PSScriptRoot\src\GlassHub.EventHorizon.CLI\GlassHub.EventHorizon.CLI.csproj"
dotnet publish $cliProject `
    -c $Configuration `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o $OutputDir

if ($LASTEXITCODE -ne 0) {
    Write-Error "Falha na publicação do CLI!"
    exit 1
}

# Copiar arquivos complementares
Copy-Item "$PSScriptRoot\assets\app.ico" -Destination "$OutputDir\app.ico" -Force -ErrorAction SilentlyContinue
Copy-Item "$PSScriptRoot\assets\logo.png" -Destination "$OutputDir\logo.png" -Force -ErrorAction SilentlyContinue
Copy-Item "$PSScriptRoot\README.md" -Destination "$OutputDir\README.md" -Force -ErrorAction SilentlyContinue
Copy-Item "$PSScriptRoot\LICENSE" -Destination "$OutputDir\LICENSE" -Force -ErrorAction SilentlyContinue

# 5. Compilação do Instalador (Inno Setup) se disponível
Write-Host "[5/5] Verificando compilador do Inno Setup (ISCC.exe)..." -ForegroundColor Yellow
$isccPaths = @(
    "ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles}\Inno Setup 6\ISCC.exe",
    "${env:LOCALAPPDATA}\Programs\Inno Setup 6\ISCC.exe"
)

$iscc = $null
foreach ($p in $isccPaths) {
    if (Get-Command $p -ErrorAction SilentlyContinue) {
        $iscc = $p
        break
    }
    if (Test-Path $p) {
        $iscc = $p
        break
    }
}

if ($iscc -and -not $NoInnoSetup) {
    Write-Host "Compilando instalador Inno Setup via $iscc..." -ForegroundColor Green
    & $iscc "$PSScriptRoot\installer.iss"
    Write-Host "✅ Instalador gerado com sucesso em .\setup_output\ !" -ForegroundColor Green
} else {
    Write-Host "ℹ️ Inno Setup (ISCC.exe) não detectado no PATH padrão. Para gerar o instalador executável:" -ForegroundColor Cyan
    Write-Host "   Execute: iscc.exe installer.iss" -ForegroundColor White
}

Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host " ✅ Publicação Concluída com Êxito!" -ForegroundColor Green
Write-Host " Binários disponíveis em: $OutputDir" -ForegroundColor White
Write-Host "=================================================================" -ForegroundColor Cyan
