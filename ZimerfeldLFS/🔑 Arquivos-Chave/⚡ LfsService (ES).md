---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [arquivo, git, git-lfs, subprocesso, service]
arquivo: src/GitExtensions.ZimerfeldLFS/LfsService.cs
---

# ⚡ LfsService.cs

Ejecutor de `git` y `git lfs` como subprocesos, con parsing de la salida para la [[🪟 LfsForm (ES)|🪟 LfsForm]].

**Ruta:** `src/GitExtensions.ZimerfeldLFS/LfsService.cs`

---

## 📦 `GitResult` (record struct)

```csharp
public readonly record struct GitResult(string StdOut, string StdErr, int ExitCode)
```
- `Ok` → `ExitCode == 0`
- `Combined` → `StdOut` + `StdErr` recortados (conveniente para el log)

---

## ⚙️ Runner interno — `RunGit(arguments)`

Ejecuta `git <arguments>` vía `ProcessStartInfo` con `WorkingDirectory = WorkingDir`, stdout/stderr redirigidos en UTF-8, `UseShellExecute=false`, `CreateNoWindow=true`. Lee stdout+stderr, espera la salida y devuelve un `GitResult`. Si `git` no está en el `PATH` (u ocurre otra excepción), devuelve `GitResult(..., ex.Message, -1)` — **se expone como resultado fallido, no como un crash**.

> `WorkingDir` es una propiedad **mutable** (`get; set;`) — la ventana cambia de repositorio simplemente asignándola.

---

## 🩺 Estado del repositorio

| Método | Comando / lógica |
|---|---|
| `IsGitRepo()` | `rev-parse --is-inside-work-tree` == `true` (y la carpeta existe) |
| `GetCurrentBranch()` | `rev-parse --abbrev-ref HEAD` |
| `GetPendingChangesCount()` | cuenta líneas de `status --porcelain` |

---

## ⚙️ Etapa 1 · Instalación

| Método | Comando / lógica |
|---|---|
| `GetLfsVersion()` | `lfs version` (stdout vacío ⇒ no instalado / fuera del PATH) |
| `IsLfsAvailable()` | versión ok y stdout empieza con `git-lfs` |
| `IsLfsInitializedForUser()` | `config --global --get filter.lfs.clean` no vacío |
| `LfsInstall()` | `lfs install` (inicializa LFS para el usuario) |

---

## 📤 Etapa 2 · Flujo básico (track / commit / push)

| Método | Comando / lógica |
|---|---|
| `GetTrackedPatterns()` | parsea `lfs track` — ignora líneas "Listing"/"Tracking" y la anotación ` (.gitattributes)`, devuelve los globs en crudo |
| `TrackPattern(p)` | `lfs track "<p>"` |
| `UntrackPattern(p)` | `lfs untrack "<p>"` |
| `GetLfsFiles()` | `lfs ls-files` (conserva la línea completa: `<oid> <*\|-> <path>`) |
| `Add(pathspec)` | `add -- <pathspec>` (se usa para stagear el `.gitattributes` tras track/untrack) |
| `Push()` | `push` (fallback cuando no hay diálogo nativo de push) |

---

## ⬇️ Etapa 3 · Clone & Pull

| Método | Comando |
|---|---|
| `LfsPull()` | `lfs pull` |
| `LfsFetchAll()` | `lfs fetch --all` |
| `LfsCheckout()` | `lfs checkout` |
| `LfsStatus()` | `lfs status` |

---

## 📂 Helper estático — `GetRepositoriesFromSettings()`

Puebla el dropdown de repositorio **independiente del host**. Lee:
```
%APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings
```
Carga el XML, busca el `item` cuya `key` es `"history"`, cuyo `value` es una **cadena XML anidada**; parsea esa cadena y recolecta cada `<Path>`; devuelve las rutas distintas (sin distinguir mayúsculas/minúsculas). Todo en `try/catch` → una lista vacía es aceptable. Ver [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de Trabajo Independiente]].

---

## 🔗 Relacionado

- [[🪟 LfsForm (ES)|🪟 LfsForm]]
- [[🔌 ZimerfeldLfsPlugin (ES)|🔌 ZimerfeldLfsPlugin]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
- [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]]
