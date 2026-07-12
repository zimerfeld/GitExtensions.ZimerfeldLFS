---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [dependências, assemblies, gitextensions, git-lfs, nuget]
---

# 🧱 Dependencias

## 📦 Assemblies de GitExtensions (referencias de compilación)

Ambas referenciadas con `<Private>false</Private>` — **no** se copian al output (el host ya las proporciona en tiempo de ejecución).

| Assembly | Ruta | Uso |
|---|---|---|
| `GitExtensions.Extensibility.dll` | `refs\` (versionado en el repo) | `IGitPlugin`, `GitPluginBase`, `IGitUICommands`, `GitUIEventArgs`, `IGitModule` |
| `System.ComponentModel.Composition.dll` | `refs\` (versionado en el repo) | MEF — `[Export(typeof(IGitPlugin))]` |

> **Build determinista (cualquier máquina Windows):** los assemblies de referencia están **versionados en `refs\`** (apuntados por `$(GitExtensionsRefPath)` en el `.csproj`), **no** se descargan en un paso prebuild. Garantiza una compilación reproducible y **offline**. El `.csproj` reduce la severidad del aviso `MSB3277` (el `WindowsBase` del host es 8.0 mientras que el ref pack net9 trae 4.0 — el runtime resuelve el correcto al cargar).

## 🏷️ Dependencia del paquete NuGet (marcador del Plugin Manager)

```xml
<dependency id="GitExtensions.Extensibility" version="[0.4.0, 0.5.0)" />
```

> [!important] Por qué existe la dependencia marcadora
> El Plugin Manager de GitExtensions filtra el feed de nuget.org por paquetes que **dependen** de
> `GitExtensions.Extensibility`. **Sin** esta dependencia, el paquete se publica pero **nunca aparece**
> en el Plugin Manager interno. Además, el filtro compara el **rango de versión** de la dependencia con la
> versión que el **manager anuncia** para el host en ejecución (**no** el runtime instalado): el manager
> v3.x de GitExtensions 6.x anuncia `0.4.0`, así que el rango debe **contener** 0.4.0 → `[0.4.0, 0.5.0)`,
> igual que los demás plugins que funcionan en GE6 (AITools, BundleBackuper, Gerrit, SolutionRunner…).
> Un valor suelto como `1.0.0.129` significa `>= 1.0.0.129`, que **no** incluye 0.4.0 — y el paquete
> era **filtrado silenciosamente fuera** del Plugin Manager. Para GitExtensions 7 (el manager
> anuncia `7.0.0`), usar `[7.0.0, 8.0.0)`.

## 📦 Empaquetado (nuspec)

- DLL en la **raíz de `lib\`** (grupo "any" que el Plugin Manager extrae) — genera el aviso **NU5101**, intencional y filtrado en `build.ps1`. Ver [[🔢 Versionamento (ES)|🔢 Versionado]].
- La misma DLL también en `tools\net9.0-windows\` para la instalación vía **Package Manager Console** (`install.ps1`).
- `LICENSE.txt` (CC BY-NC-ND 4.0, `type="file"`), `README.md`/`README.pt-BR.md`/`README.en-US.md`/`README.es-ES.md`, e `icon.png` (de `Resources\icon-128.png`) en la raíz del paquete.

## 🔑 Interfaces clave utilizadas

### `IGitPlugin` (vía `GitPluginBase`)
- `Register(IGitUICommands)` / `Unregister(IGitUICommands)` — captura/limpia `_commands`
- `Execute(GitUIEventArgs)` — llamado desde el menú Plugins → ZimerfeldLFS

### `IGitUICommands`
- `WithWorkingDirectory(dir)` — obtiene un `IGitUICommands` apuntando al repositorio de `cboRepo`
- `StartCommitDialog(owner, message, …)` / `StartPushDialog(owner, …, out pushCompleted)` — diálogos nativos
- `RepoChangedNotifier?.Notify()` — pide al host que actualice su propia UI después de un commit/checkout
- `Module.WorkingDir` — directorio de trabajo del host (usado solo como valor inicial del dropdown)

## ✅ Runtime (lo que el usuario necesita tener)

| Requisito | Valor |
|---|---|
| GitExtensions | 6.x (.NET 9) |
| .NET | 9.0 (Windows) — proporcionado por el host |
| `git` | en el `PATH` (`LfsService` ejecuta subprocesos) |
| `git-lfs` | en el `PATH` (extensión Git LFS) |
| PowerShell | 5.1+ (scripts de build/deploy) |
| .NET 9 SDK + nuget | para compilar y empaquetar |

## 🔗 Relacionado

- [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]]
- [[🔢 Versionamento (ES)|🔢 Versionado]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
