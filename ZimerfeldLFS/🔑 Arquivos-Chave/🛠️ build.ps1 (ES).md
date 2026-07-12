---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [arquivo, build, powershell, versionamento, nupkg]
arquivo: build.ps1
---

# 🛠️ build.ps1

Script principal de build, versionado y empaquetado.

**Ruta:** `build.ps1` (raíz del repositorio)

**Requiere:** PowerShell 5.1+, .NET 9 SDK (`dotnet`), `nuget` (descargado a `tools\nuget.exe` si está ausente), Admin para el deploy.

---

## 🎛️ Parámetro

- **`-Force`** — ignora la detección de cambios y siempre incrementa/recompila/empaqueta.

---

## 👣 Pasos detallados

### 1. Lee la versión actual del `.nuspec`
`[xml]$spec = Get-Content $nuspec` → `$current` (valida formato `major.minor.build`).

### 2. Calcula newVersion
`$build = [int]$parts[2] + 1` → `$newVersion = "$major.$minor.$build"`.

### 2b. Detecta cambios
Compara el timestamp más reciente entre las fuentes (`*.cs`/`*.csproj`/`*.nuspec`/`*.json`/`*.png`, fuera de `bin`/`obj`), `*.md` y documentación (`LICENSE.txt`, `install/uninstall.ps1`) con el último `.nupkg`. Sin `-Force` y sin cambios → **mantiene la versión y sale** (build/pack se omiten).

### 2c. Cierra GitExtensions
`Stop-Process -Force` sobre los procesos `GitExtensions` (libera la DLL para el deploy).

### 3–4. Bump en el `.nuspec` y `.csproj`
`$spec.package.metadata.version = $newVersion` (Save); regex `<Version>…</Version>` → nueva versión en el csproj.

### 4b. Sella los READMEs
- `README.md`: actualiza `**Versão atual: …**` y el enlace de NuGet a la nueva versión.
- `README.md` / `README.pt-BR.md` / `README.en-US.md` / `README.es-ES.md`: actualiza `**Version/Versão:**` y `**Updated/Atualizado em:**` (fecha de hoy).

### 4c. Sella la bóveda de Obsidian
Sella `versao:`/`atualizado:` del frontmatter y las variantes de `Versão atual` (negrita, tabla y etiqueta+comilla invertida) en las notas que reflejan la versión actual del proyecto (Proyecto, README espejo, Visión General, Versionado, HOME). Se ejecuta **antes** del pack, así el `.nupkg` sigue siendo el archivo más nuevo (la detección de cambios no entra en bucle).

### 5. Build
`dotnet build $csproj -c Release --nologo -v minimal`. Cuenta errores/avisos por regex; falla si `buildExit != 0` o hay algún error.

### 6. Deploy (Admin)
Copia la DLL a `C:\Program Files\GitExtensions\Plugins\` (fallback x86). Se omite con un aviso si no hay Admin o falta la carpeta.

### 6b. Actualiza `tools\net9.0-windows\`
Copia la DLL a la carpeta usada por el nupkg.

### 7. Pack
`nuget pack $nuspec -OutputDirectory $outDir`, **filtrando el aviso NU5101** (DLL en `lib\` raíz es intencional). Elimina `.nupkg` de versiones anteriores.

---

## 📦 Salidas

| Artefacto | Ubicación |
|---|---|
| DLL compilada | `src\...\bin\Release\net9.0-windows\GitExtensions.Plugins.ZimerfeldLFS.dll` |
| DLL instalada | `C:\Program Files\GitExtensions\Plugins\` |
| DLL en el nupkg | `tools\net9.0-windows\` |
| Paquete NuGet | `.\GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` |

---

## ▶️ Cómo ejecutar

```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1            # incrementa versión, build Release, deploy (Admin), nupkg
.\build.ps1 -Force    # empaqueta aunque no haya cambios

# Sin Admin (Bash tool / Git Bash):
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "build.ps1"
```

---

## 🔗 Relacionado

- [[🔢 Versionamento (ES)|🔢 Versionado]]
- [[🧱 Dependências (ES)|🧱 Dependencias]]
- [[🎨 Generate-LfsIcon (ES)|🎨 Generate-LfsIcon]]
- [[💻 Ambiente Local (Dev) (ES)|💻 Entorno Local (Dev)]]
- [[🚀 Deploy em Produção (Prod) (ES)|🚀 Deploy en Producción (Prod)]]
