---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [dependências, assemblies, gitextensions, git-lfs, nuget]
---

# 🧱 Dependencies

## 📦 GitExtensions assemblies (compile references)

Both referenced with `<Private>false</Private>` — **not** copied to the output (the host already provides them at runtime).

| Assembly | Path | Usage |
|---|---|---|
| `GitExtensions.Extensibility.dll` | `refs\` (versioned in the repo) | `IGitPlugin`, `GitPluginBase`, `IGitUICommands`, `GitUIEventArgs`, `IGitModule` |
| `System.ComponentModel.Composition.dll` | `refs\` (versioned in the repo) | MEF — `[Export(typeof(IGitPlugin))]` |

> **Deterministic build (any Windows machine):** the reference assemblies are **versioned in `refs\`** (pointed to by `$(GitExtensionsRefPath)` in the `.csproj`), **not** downloaded in a prebuild step. This guarantees reproducible and **offline** compilation. The `.csproj` demotes the `MSB3277` warning (the host's `WindowsBase` is 8.0 while the net9 ref pack brings 4.0 — the runtime resolves the correct one at load time).

## 🏷️ NuGet package dependency (Plugin Manager marker)

```xml
<dependency id="GitExtensions.Extensibility" version="[0.4.0, 0.5.0)" />
```

> [!important] Why the marker dependency exists
> The GitExtensions Plugin Manager filters the nuget.org feed for packages that **depend** on
> `GitExtensions.Extensibility`. **Without** this dependency, the package is published but **never appears**
> in the internal Plugin Manager. In addition, the filter matches the dependency's **version range** against
> the version the **manager advertises** for the running host (**not** the installed runtime): the
> v3.x manager of GitExtensions 6.x advertises `0.4.0`, so the range must **contain** 0.4.0 → `[0.4.0, 0.5.0)`,
> just like the other plugins that work on GE6 (AITools, BundleBackuper, Gerrit, SolutionRunner…).
> A loose value like `1.0.0.129` means `>= 1.0.0.129`, which does **not** include 0.4.0 — and the package
> was **silently filtered out** of the Plugin Manager. For GitExtensions 7 (the manager
> advertises `7.0.0`), use `[7.0.0, 8.0.0)`.

## 📦 Packaging (nuspec)

- DLL in the **`lib\` root** ("any" group, which the Plugin Manager extracts) — triggers the **NU5101** warning, intentional and filtered in `build.ps1`. See [[🔢 Versionamento (EN)|🔢 Versioning]].
- The same DLL also in `tools\net9.0-windows\` for install via the **Package Manager Console** (`install.ps1`).
- `LICENSE.txt` (CC BY-NC-ND 4.0, `type="file"`), `README.md`/`README.pt-BR.md`/`README.en-US.md`/`README.es-ES.md`, and `icon.png` (from `Resources\icon-128.png`) at the package root.

## 🔑 Key interfaces used

### `IGitPlugin` (via `GitPluginBase`)
- `Register(IGitUICommands)` / `Unregister(IGitUICommands)` — captures/clears `_commands`
- `Execute(GitUIEventArgs)` — called via menu Plugins → ZimerfeldLFS

### `IGitUICommands`
- `WithWorkingDirectory(dir)` — obtains an `IGitUICommands` pointed at the `cboRepo` repository
- `StartCommitDialog(owner, message, …)` / `StartPushDialog(owner, …, out pushCompleted)` — native dialogs
- `RepoChangedNotifier?.Notify()` — asks the host to refresh its own UI after commit/checkout
- `Module.WorkingDir` — the host's working dir (used only as the dropdown's initial value)

## ✅ Runtime (what the user needs)

| Requirement | Value |
|---|---|
| GitExtensions | 6.x (.NET 9) |
| .NET | 9.0 (Windows) — provided by the host |
| `git` | on the `PATH` (`LfsService` runs subprocesses) |
| `git-lfs` | on the `PATH` (Git LFS extension) |
| PowerShell | 5.1+ (build/deploy scripts) |
| .NET 9 SDK + nuget | to compile and package |

## 🔗 Related

- [[🏛️ Arquitetura (EN)|🏛️ Architecture]]
- [[🔢 Versionamento (EN)|🔢 Versioning]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
