#Requires -Version 5.1
<#
.SYNOPSIS
    Compila o plugin ZimerfeldLFS, incrementa a versao (major.minor.build) e gera o .nupkg.
    Executar como Administrador para tambem fazer o deploy em GitExtensions.
.PARAMETER Force
    Ignora a deteccao de mudancas e sempre incrementa a versao, recompila e empacota.
#>
[CmdletBinding()]
param(
    [switch]$Force
)

$ErrorActionPreference = "Stop"

$nuspec  = "$PSScriptRoot\src\GitExtensions.ZimerfeldLFS\GitExtensions.ZimerfeldLFS.nuspec"
$csproj  = "$PSScriptRoot\src\GitExtensions.ZimerfeldLFS\GitExtensions.ZimerfeldLFS.csproj"
$outDir  = $PSScriptRoot

# -- 1. Ler versao atual do nuspec ---------------------------------------------
[xml]$spec = Get-Content $nuspec -Encoding UTF8
$current   = $spec.package.metadata.version
$parts     = $current -split '\.'
if ($parts.Count -ne 3) {
    Write-Error "Versao '$current' nao esta no formato major.minor.build"
    exit 1
}
$major      = [int]$parts[0]
$minor      = [int]$parts[1]
$build      = [int]$parts[2] + 1
$newVersion = "$major.$minor.$build"

# -- 1b. Detectar mudancas -----------------------------------------------------
# So' incrementa a versao (e recompila/empacota) se houver fonte OU texto/documento
# empacotado mais novo que o ultimo .nupkg gerado. Use -Force para sempre empacotar.
$srcRoot  = "$PSScriptRoot\src\GitExtensions.ZimerfeldLFS"
$srcFiles = Get-ChildItem $srcRoot -Recurse -File -Include *.cs,*.csproj,*.nuspec,*.json,*.png |
            Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
$mdFiles  = Get-ChildItem $PSScriptRoot -Recurse -File -Filter *.md |
            Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
$otherDocs = @(
    "$PSScriptRoot\LICENSE.txt",
    "$PSScriptRoot\tools\install.ps1",
    "$PSScriptRoot\tools\uninstall.ps1"
) | Where-Object { Test-Path $_ } | ForEach-Object { Get-Item $_ }
$inputFiles = @($srcFiles) + @($mdFiles) + @($otherDocs)
$newestSrc  = ($inputFiles | Measure-Object -Property LastWriteTimeUtc -Maximum).Maximum

$lastPkg = Get-ChildItem "$outDir\GitExtensions.ZimerfeldLFS.*.nupkg" -ErrorAction SilentlyContinue |
           Sort-Object LastWriteTimeUtc -Descending | Select-Object -First 1

if (-not $Force -and $lastPkg -and $newestSrc -le $lastPkg.LastWriteTimeUtc) {
    Write-Host ""
    Write-Host "Nenhuma mudanca detectada em fontes ou textos -- versao mantida em $current (build/pack ignorados)." -ForegroundColor Cyan
    Write-Host "Use -Force para empacotar mesmo assim." -ForegroundColor DarkGray
    exit 0
}

Write-Host "Versao: $current  ->  " -NoNewline
Write-Host $newVersion -ForegroundColor Green

# -- 1c. Fechar GitExtensions antes de compilar --------------------------------
$geProcs = Get-Process -Name GitExtensions -ErrorAction SilentlyContinue
if ($geProcs) {
    Write-Host "Fechando GitExtensions e plugins..."
    $geProcs | Stop-Process -Force
    Start-Sleep -Milliseconds 800
    Write-Host "GitExtensions encerrado."
} else {
    Write-Host "GitExtensions nao esta em execucao."
}

# -- 2. Atualizar nuspec -------------------------------------------------------
$spec.package.metadata.version = $newVersion
$spec.Save($nuspec)

# -- 3. Atualizar csproj -------------------------------------------------------
$csprojContent = Get-Content $csproj -Raw -Encoding UTF8
$csprojContent = $csprojContent -replace '<Version>[^<]+</Version>', "<Version>$newVersion</Version>"
[System.IO.File]::WriteAllText($csproj, $csprojContent, [System.Text.Encoding]::UTF8)

# -- 4. Atualizar link do nuget no README.md -----------------------------------
$readmeDoc = "$PSScriptRoot\README.md"
if (Test-Path $readmeDoc) {
    $content = Get-Content $readmeDoc -Raw -Encoding UTF8
    $content = $content -replace '\*\*Versão atual: [^\*]+\*\*', "**Versão atual: $newVersion**"
    $content = $content -replace 'https://www\.nuget\.org/packages/GitExtensions\.ZimerfeldLFS/[\d\.]+', "https://www.nuget.org/packages/GitExtensions.ZimerfeldLFS/$newVersion"
    [System.IO.File]::WriteAllText($readmeDoc, $content, [System.Text.Encoding]::UTF8)
}

# -- 4b. Atualizar cabecalho (Versao/Atualizado) no topo dos READMEs -----------
$today = (Get-Date).ToString('yyyy-MM-dd')
foreach ($doc in @("$PSScriptRoot\README.md", "$PSScriptRoot\README.pt-BR.md", "$PSScriptRoot\README.en-US.md")) {
    if (Test-Path $doc) {
        $c = Get-Content $doc -Raw -Encoding UTF8
        $c = $c -replace '(?m)^\*\*(Versão|Version):\*\*\s+[\d\.]+',                             "**`$1:** $newVersion"
        $c = $c -replace '(?m)^\*\*(Atualizado em|Updated on|Updated):\*\*\s+\d{4}-\d{2}-\d{2}', "**`$1:** $today"
        [System.IO.File]::WriteAllText($doc, $c, [System.Text.Encoding]::UTF8)
        Write-Host "$([System.IO.Path]::GetFileName($doc)) atualizado para $newVersion ($today)"
    }
}

# -- 4c. Carimbar a versao no cofre Obsidian (o vault espelha o README) --------
# O bump tambem reflete no cofre, para o vault nao ficar defasado em relacao ao
# README -- mesma versao em README/csproj/nuspec/vault, sem sync manual. Atualiza o
# 'versao:'/'atualizado:' do frontmatter e as variantes de 'Versao atual' (bold,
# tabela e rotulo+crase) das notas que espelham a versao ATUAL do projeto.
# Roda ANTES do pack, entao o .nupkg permanece o arquivo mais novo e a deteccao de
# mudancas (secao 1b) nao dispara em loop. Uma linha de log por nota atualizada.
$vault = "$PSScriptRoot\OBSIDIAN\CLAUDE"
$obsidianDocs = @(
    "$vault\01 - Projetos\GitExtensions.ZimerfeldLFS.md",
    "$vault\02 - Conhecimento\README — Instalação, Uso e Build.md",
    "$vault\Sistema\Visão Geral.md",
    "$vault\Sistema\Versionamento.md",
    "$vault\HOME.md"
)
foreach ($obsDoc in $obsidianDocs) {
    if (Test-Path $obsDoc) {
        $v = Get-Content $obsDoc -Raw -Encoding UTF8
        # Frontmatter
        $v = $v -replace '(?m)^versao:\s+[\d\.]+',                   "versao: $newVersion"
        $v = $v -replace '(?m)^atualizado:\s+\d{4}-\d{2}-\d{2}',     "atualizado: $today"
        # "Versao atual: **X**" (texto/negrito)
        $v = $v -replace 'Versão atual: \*\*[\d\.]+\*\*',            "Versão atual: **$newVersion**"
        # "| Versao atual | `X` |" (tabela)  e  "**Versao atual:** `X`" (rotulo+crase)
        $v = $v -replace '(\|\s*Versão atual\s*\|\s*`)[\d\.]+(`)',   ('${1}' + $newVersion + '${2}')
        $v = $v -replace '(\*\*Versão atual:\*\*\s*`)[\d\.]+(`)',    ('${1}' + $newVersion + '${2}')
        # HOME.md: linha "`X` — compilada ..." (comeca com a versao entre crases)
        $v = $v -replace '(?m)^`[\d\.]+`(\s+—\s+compilada)',        ('`' + $newVersion + '`' + '${1}')
        [System.IO.File]::WriteAllText($obsDoc, $v, [System.Text.Encoding]::UTF8)
        Write-Host "Obsidian: $([System.IO.Path]::GetFileName($obsDoc)) atualizado para $newVersion ($today)"
    }
}

# -- 5. Build ------------------------------------------------------------------
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet.exe nao encontrado no PATH. Instale o .NET 9 SDK e tente novamente."
    exit 1
}
Write-Host "Compilando..."
$buildOutput = & dotnet build $csproj -c Release --nologo -v minimal 2>&1
$buildExit   = $LASTEXITCODE
$buildOutput | ForEach-Object {
    $line = "$_"
    if ($line -match 'GitExtensions\.Extensibility') { return }
    if     ($line -match '^\s*Build succeeded\.')  { Write-Host $line -ForegroundColor Green }
    elseif ($line -match '^\s*\d+\s+Warning\(s\)') { Write-Host $line -ForegroundColor Yellow }
    elseif ($line -match '^\s*\d+\s+Error\(s\)')   { Write-Host $line -ForegroundColor Red }
    else { Write-Host $line }
}

$buildText    = $buildOutput | Out-String
$errorCount   = ([regex]::Matches($buildText, '(?im):\s*error\s')).Count
$warningCount = ([regex]::Matches($buildText, '(?im):\s*warning\s')).Count

if ($buildExit -ne 0 -or $errorCount -gt 0) {
    Write-Host "Build falhou: $errorCount erro(s)." -ForegroundColor Red
    exit 1
}
elseif ($warningCount -gt 0) {
    Write-Host "Build concluido com $warningCount aviso(s)." -ForegroundColor Yellow
}
else {
    Write-Host "Build concluido com sucesso (nenhum erro ou aviso)." -ForegroundColor Green
}

# -- 6. Deploy (requer Admin) --------------------------------------------------
$pluginsDir = "C:\Program Files\GitExtensions\Plugins"
if (-not (Test-Path $pluginsDir)) {
    $pluginsDir = "C:\Program Files (x86)\GitExtensions\Plugins"
}
$dll = "$PSScriptRoot\src\GitExtensions.ZimerfeldLFS\bin\Release\net9.0-windows\GitExtensions.Plugins.ZimerfeldLFS.dll"

$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(
    [Security.Principal.WindowsBuiltInRole]::Administrator)

if ($isAdmin -and (Test-Path $pluginsDir)) {
    Copy-Item $dll $pluginsDir -Force
    Write-Host "Plugin instalado em: $pluginsDir"
} else {
    Write-Warning "Sem permissao de Admin ou pasta nao encontrada -- deploy pulado."
    Write-Host "  Copie manualmente: $dll"
    Write-Host "  Para: $pluginsDir"
}

# Atualiza copia na pasta tools (usada pelo nupkg)
$toolsTarget = "$PSScriptRoot\tools\net9.0-windows"
if (-not (Test-Path $toolsTarget)) { New-Item -ItemType Directory $toolsTarget | Out-Null }
Copy-Item $dll $toolsTarget -Force

# -- 7. Pack -------------------------------------------------------------------
Write-Host "Gerando pacote $newVersion..."

$nugetCmd = Get-Command nuget -ErrorAction SilentlyContinue
$nugetExe = if ($nugetCmd) { $nugetCmd.Source } else { $null }
if (-not $nugetExe) {
    $nugetExe = Join-Path $PSScriptRoot "tools\nuget.exe"
    if (-not (Test-Path $nugetExe)) {
        Write-Host "nuget.exe nao encontrado - baixando para tools\nuget.exe..."
        Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" `
                          -OutFile $nugetExe -UseBasicParsing
        Write-Host "Download concluido."
    }
}

# NU5101 (DLL diretamente em lib\) e' INTENCIONAL (ver nuspec) -> filtramos esse aviso.
& $nugetExe pack $nuspec -OutputDirectory $outDir 2>&1 |
    Where-Object { $_ -notmatch 'NU5101' } |
    ForEach-Object { Write-Host $_ }
if ($LASTEXITCODE -ne 0) { Write-Error "nuget pack falhou."; exit 1 }

# Remove pacotes de versoes anteriores
Get-ChildItem "$outDir\GitExtensions.ZimerfeldLFS.*.nupkg" |
    Where-Object { $_.Name -notlike "*$newVersion*" } |
    Remove-Item -Force

Write-Host ""
Write-Host "Concluido: GitExtensions.ZimerfeldLFS.$newVersion.nupkg"
