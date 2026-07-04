---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [arquivo, git, git-lfs, subprocesso, service]
arquivo: src/GitExtensions.ZimerfeldLFS/LfsService.cs
---

# ⚡ LfsService.cs

Runner of `git` and `git lfs` as subprocesses, with output parsing for the [[🪟 LfsForm (EN)|🪟 LfsForm]].

**Path:** `src/GitExtensions.ZimerfeldLFS/LfsService.cs`

---

## 📦 `GitResult` (record struct)

```csharp
public readonly record struct GitResult(string StdOut, string StdErr, int ExitCode)
```
- `Ok` → `ExitCode == 0`
- `Combined` → trimmed `StdOut` + `StdErr` (convenient for the log)

---

## ⚙️ Internal runner — `RunGit(arguments)`

Runs `git <arguments>` via `ProcessStartInfo` with `WorkingDirectory = WorkingDir`, stdout/stderr redirected in UTF-8, `UseShellExecute=false`, `CreateNoWindow=true`. Reads stdout+stderr, awaits exit and returns a `GitResult`. If `git` is not on the `PATH` (or another exception occurs), returns `GitResult(..., ex.Message, -1)` — **surfaced as a failed result, not a crash**.

> `WorkingDir` is a **mutable** property (`get; set;`) — the window switches repositories simply by assigning it.

---

## 🩺 Repository state

| Method | Command / logic |
|---|---|
| `IsGitRepo()` | `rev-parse --is-inside-work-tree` == `true` (and the folder exists) |
| `GetCurrentBranch()` | `rev-parse --abbrev-ref HEAD` |
| `GetPendingChangesCount()` | counts lines of `status --porcelain` |

---

## ⚙️ Step 1 · Installation

| Method | Command / logic |
|---|---|
| `GetLfsVersion()` | `lfs version` (empty stdout ⇒ not installed / off the PATH) |
| `IsLfsAvailable()` | version ok and stdout starts with `git-lfs` |
| `IsLfsInitializedForUser()` | `config --global --get filter.lfs.clean` non-empty |
| `LfsInstall()` | `lfs install` (initializes LFS for the user) |

---

## 📤 Step 2 · Basic workflow (track / commit / push)

| Method | Command / logic |
|---|---|
| `GetTrackedPatterns()` | parses `lfs track` — ignores "Listing"/"Tracking" lines and the ` (.gitattributes)` annotation, returns the raw globs |
| `TrackPattern(p)` | `lfs track "<p>"` |
| `UntrackPattern(p)` | `lfs untrack "<p>"` |
| `GetLfsFiles()` | `lfs ls-files` (keeps the whole line: `<oid> <*\|-> <path>`) |
| `Add(pathspec)` | `add -- <pathspec>` (used to stage `.gitattributes` after track/untrack) |
| `Push()` | `push` (fallback when there is no native push dialog) |

---

## ⬇️ Step 3 · Clone & Pull

| Method | Command |
|---|---|
| `LfsPull()` | `lfs pull` |
| `LfsFetchAll()` | `lfs fetch --all` |
| `LfsCheckout()` | `lfs checkout` |
| `LfsStatus()` | `lfs status` |

---

## 📂 Static helper — `GetRepositoriesFromSettings()`

Populates the **host-independent** repository dropdown. Reads:
```
%APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings
```
Loads the XML, finds the `item` whose `key` is `"history"`, whose `value` is a **nested XML string**; parses that string and collects each `<Path>`; returns the distinct paths (case-insensitive). All in `try/catch` → an empty list is acceptable. See [[📂 Diretório de Trabalho Independente (EN)|📂 Independent Working Directory]].

---

## 🔗 Related

- [[🪟 LfsForm (EN)|🪟 LfsForm]]
- [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
- [[🏛️ Arquitetura (EN)|🏛️ Architecture]]
