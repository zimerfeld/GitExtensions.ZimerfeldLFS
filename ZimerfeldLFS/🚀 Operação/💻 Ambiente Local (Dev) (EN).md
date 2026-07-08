---
tipo: procedimento
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [operacao, runbook, dev, build, gitextensions, powershell]
---

# 💻 Local Environment (Dev)

How to build the plugin, install it into the local GitExtensions and iterate during development.

## ⚡ TL;DR — the single command
```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1            # bumps build, compiles Release, deploys into the local GitExtensions, generates nupkg
```
Then open GitExtensions and go to **Plugins → ZimerfeldLFS**. To force even without changes: `.\build.ps1 -Force`.

## ⚙️ What the script does (in order)
`build.ps1` (detailed in [[🛠️ build.ps1 (EN)|🛠️ build.ps1]]):
1. Reads the current version from the `.nuspec` and computes the next one (`build +1`).
2. **Detects changes** — without `-Force` and with no sources/docs newer than the last `.nupkg`, it keeps the version and exits.
3. **Closes any running GitExtensions** (releases the DLL for deploy).
4. Applies the version bump in the `.nuspec` and `.csproj`; stamps version + date into the READMEs and this vault.
5. `dotnet build -c Release` — fails on error; counts errors/warnings.
6. **Deploy (requires Admin):** copies the DLL to `C:\Program Files\GitExtensions\Plugins\`.
7. Copies the DLL to `tools\net9.0-windows\` and packs the `.nupkg` (filtering the intentional **NU5101** warning).

## 🧩 Alternative manual install (tools/)
```powershell
tools\install.ps1      # requires Admin — copies the DLL to the Plugins folder (also via Package Manager Console)
tools\uninstall.ps1    # requires Admin — removes the DLL (does not affect the rest of GitExtensions)
```

## 🎛️ Parameters / flags
- **`-Force`** — ignores change detection and always bumps/recompiles/packs.

## 🔐 Requirements
- **.NET 9 SDK** (`dotnet`) on the PATH.
- **Administrator** for the deploy step (otherwise it is skipped with a warning).
- `nuget` (auto-downloaded to `tools\nuget.exe` if missing).
- **Without Admin** (e.g.: via the Bash tool): `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "build.ps1"` — compiles and packs; only the deploy is skipped.

## 📏 Rules it respects
- **GitFlow:** develop on the current feature branch; `build.ps1` only builds/packs, it does not commit or push.
- **Do not commit/push without an explicit request.**

## 🩺 Troubleshooting
- **"Git LFS is NOT installed"** in the window → `git` / `git-lfs` off the `PATH` (the `LfsService` runs as a subprocess). See [[🧱 Dependências (EN)|🧱 Dependencies]].
- **Deploy skipped** → run PowerShell as Administrator or use `tools\install.ps1`.
- **DLL locked** → `build.ps1` closes GitExtensions before deploy; if it fails, close it manually.

## 🔗 Links
- [[🛠️ build.ps1 (EN)|🛠️ build.ps1]]
- [[🔢 Versionamento (EN)|🔢 Versioning]]
- [[🧱 Dependências (EN)|🧱 Dependencies]]
- [[🚀 Deploy em Produção (Prod) (EN)|🚀 Production Deploy (Prod)]]
