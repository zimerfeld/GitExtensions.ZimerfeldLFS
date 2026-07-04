---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [build, versão, nupkg, deploy]
---

# 🔢 Versioning and Build

## 🔢 Version scheme

`major.minor.build` — only `build` is incremented automatically by `build.ps1`. Major and minor are changed manually.

**Current version:** `1.0.2` *(source of truth: `.nuspec` / `.csproj`)*

> [!note] Embedded language dictionaries (no satellite assemblies)
> The UI strings live in `Resources/ZimerfeldLFS.en-US.json` and `Resources/ZimerfeldLFS.pt-BR.json`,
> embedded in the **main assembly** with `WithCulture="false"` + explicit `LogicalName` — so
> MSBuild does **not** interpret the `.en-US`/`.pt-BR` infix as a culture (which would divert them
> to satellite assemblies), preserving the **single-DLL** deployment. Read at runtime by `I18n`.

## 🔄 build.ps1 cycle

```
build.ps1  [-Force]
  │
  ├─ 1. Reads the current version from the .nuspec
  ├─ 2. Computes newVersion (build +1)
  ├─ 2b. Detects changes (sources/docs newer than the last .nupkg); without -Force and with
  │      no change → keeps the version and exits (build/pack skipped)
  ├─ 2c. Closes GitExtensions if it is running
  ├─ 3. Bumps the .nuspec  ← <version>
  ├─ 4. Bumps the .csproj  ← <Version>
  ├─ 4b. Stamps version + date at the top of the READMEs (README.md / .pt-BR / .en-US)
  ├─ 4c. Stamps version + date in this Obsidian vault (notes that mirror the version)
  ├─ 5. dotnet build -c Release
  ├─ 6. Copies DLL → C:\Program Files\GitExtensions\Plugins\  (requires Admin)
  ├─ 6b. Copies DLL → tools\net9.0-windows\  (for the nupkg)
  ├─ 7. nuget pack .nuspec → .nupkg at the root (filters the NU5101 warning)
  └─ 7b. Removes .nupkg of previous versions
```

> [!warning] Warning **NU5101** is intentional
> The DLL is packaged in the `lib\` **root** ("any" group, which the Plugin Manager extracts), not in
> `lib\net9.0-windows\`. This triggers the NU5101 warning, which `build.ps1` **filters on purpose** in
> `nuget pack`. Details in [[🧱 Dependências (EN)|🧱 Dependencies]] and in the `.nuspec`.

<!-- -->

> Requires the **.NET 9 SDK** (`dotnet`) and, for deployment, **Administrator** permission. Without Admin, the deploy step is skipped with a warning; `nuget` is downloaded to `tools\nuget.exe` if not on the PATH.

## 📄 Versioned files

| File | Updated field |
|---|---|
| `GitExtensions.ZimerfeldLFS.nuspec` | `<version>` |
| `GitExtensions.ZimerfeldLFS.csproj` | `<Version>` (and `**Versão atual**` + NuGet link in README.md) |
| `README.md` / `README.pt-BR.md` / `README.en-US.md` | `**Version/Versão:**` and `**Updated/Atualizado em:**` |

> **Note:** `build.ps1` stamps version/date in the **READMEs** (section 4b) **and** in this Obsidian vault (section 4c) — the notes that mirror the current version (Project, README mirror, Overview, Versioning, HOME) stay automatically in sync, just like the CommitMsg sibling.

## 🔧 Manual install / uninstall

```powershell
tools\install.ps1        # requires Admin — copies the DLL to the Plugins folder (also via PMC)
tools\uninstall.ps1      # requires Admin — removes the DLL (does not affect the rest of GitExtensions)
```

## 🔗 Related

- [[🛠️ build.ps1 (EN)|🛠️ build.ps1]]
- [[🧱 Dependências (EN)|🧱 Dependencies]]
- [[💻 Ambiente Local (Dev) (EN)|💻 Local Environment (Dev)]]
- [[🚀 Deploy em Produção (Prod) (EN)|🚀 Production Deploy (Prod)]]
