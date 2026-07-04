---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [arquivo, build, powershell, versionamento, nupkg]
arquivo: build.ps1
---

# 🛠️ build.ps1

Main build, versioning and packaging script.

**Path:** `build.ps1` (repository root)

**Requires:** PowerShell 5.1+, .NET 9 SDK (`dotnet`), `nuget` (downloaded to `tools\nuget.exe` if missing), Admin for deploy.

---

## 🎛️ Parameter

- **`-Force`** — ignores change detection and always increments/rebuilds/repacks.

---

## 👣 Detailed steps

### 1. Reads the current version from the `.nuspec`
`[xml]$spec = Get-Content $nuspec` → `$current` (validates `major.minor.build` format).

### 2. Computes newVersion
`$build = [int]$parts[2] + 1` → `$newVersion = "$major.$minor.$build"`.

### 2b. Detects changes
Compares the newest timestamp among sources (`*.cs`/`*.csproj`/`*.nuspec`/`*.json`/`*.png`, outside `bin`/`obj`), `*.md` and docs (`LICENSE.txt`, `install/uninstall.ps1`) against the last `.nupkg`. Without `-Force` and with no change → **keeps the version and exits** (build/pack skipped).

### 2c. Closes GitExtensions
`Stop-Process -Force` on the `GitExtensions` processes (frees the DLL for deployment).

### 3–4. Bumps the `.nuspec` and `.csproj`
`$spec.package.metadata.version = $newVersion` (Save); regex `<Version>…</Version>` → new version in the csproj.

### 4b. Stamps the READMEs
- `README.md`: updates `**Versão atual: …**` and the NuGet link to the new version.
- `README.md` / `README.pt-BR.md` / `README.en-US.md`: updates `**Version/Versão:**` and `**Updated/Atualizado em:**` (today's date).

### 4c. Stamps the Obsidian vault
Stamps the frontmatter `versao:`/`atualizado:` and the `Versão atual` variants (bold, table and label+backtick) in the notes that mirror the project's current version (Project, README mirror, Overview, Versioning, HOME). Runs **before** the pack, so the `.nupkg` remains the newest file (change detection does not loop).

### 5. Build
`dotnet build $csproj -c Release --nologo -v minimal`. Counts errors/warnings via regex; fails if `buildExit != 0` or there is an error.

### 6. Deploy (Admin)
Copies the DLL to `C:\Program Files\GitExtensions\Plugins\` (x86 fallback). Skipped with a warning if not Admin or the folder is missing.

### 6b. Updates `tools\net9.0-windows\`
Copies the DLL to the folder used by the nupkg.

### 7. Pack
`nuget pack $nuspec -OutputDirectory $outDir`, **filtering the NU5101 warning** (DLL in the `lib\` root is intentional). Removes `.nupkg` of previous versions.

---

## 📦 Outputs

| Artifact | Location |
|---|---|
| Compiled DLL | `src\...\bin\Release\net9.0-windows\GitExtensions.Plugins.ZimerfeldLFS.dll` |
| Installed DLL | `C:\Program Files\GitExtensions\Plugins\` |
| DLL in the nupkg | `tools\net9.0-windows\` |
| NuGet package | `.\GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` |

---

## ▶️ How to run

```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1            # bumps version, Release build, deploy (Admin), nupkg
.\build.ps1 -Force    # packs even without changes

# Without Admin (Bash tool / Git Bash):
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "build.ps1"
```

---

## 🔗 Related

- [[🔢 Versionamento (EN)|🔢 Versioning]]
- [[🧱 Dependências (EN)|🧱 Dependencies]]
- [[🎨 Generate-LfsIcon (EN)|🎨 Generate-LfsIcon]]
- [[💻 Ambiente Local (Dev) (EN)|💻 Local Environment (Dev)]]
- [[🚀 Deploy em Produção (Prod) (EN)|🚀 Production Deploy (Prod)]]
