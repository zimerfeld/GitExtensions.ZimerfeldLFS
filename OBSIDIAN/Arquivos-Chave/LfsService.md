---
tipo: arquivo
tags: [arquivo, git, git-lfs, subprocesso, service]
arquivo: src/GitExtensions.ZimerfeldLFS/LfsService.cs
atualizado: 2026-07-01
---

# LfsService.cs

Executor de `git` e `git lfs` como subprocessos, com parsing da saída para a [[LfsForm]].

**Caminho:** `src/GitExtensions.ZimerfeldLFS/LfsService.cs`

---

## `GitResult` (record struct)

```csharp
public readonly record struct GitResult(string StdOut, string StdErr, int ExitCode)
```
- `Ok` → `ExitCode == 0`
- `Combined` → `StdOut` + `StdErr` aparados (conveniente para o log)

---

## Runner interno — `RunGit(arguments)`

Roda `git <arguments>` via `ProcessStartInfo` com `WorkingDirectory = WorkingDir`, stdout/stderr redirecionados em UTF-8, `UseShellExecute=false`, `CreateNoWindow=true`. Lê stdout+stderr, aguarda o exit e devolve `GitResult`. Se o `git` não está no `PATH` (ou outra exceção), retorna `GitResult(..., ex.Message, -1)` — **surface como resultado falho, não crash**.

> `WorkingDir` é uma propriedade **mutável** (`get; set;`) — a janela troca de repositório apenas atribuindo-a.

---

## Estado do repositório

| Método | Comando / lógica |
|---|---|
| `IsGitRepo()` | `rev-parse --is-inside-work-tree` == `true` (e a pasta existe) |
| `GetCurrentBranch()` | `rev-parse --abbrev-ref HEAD` |
| `GetPendingChangesCount()` | conta linhas de `status --porcelain` |

---

## Etapa 1 · Instalação

| Método | Comando / lógica |
|---|---|
| `GetLfsVersion()` | `lfs version` (stdout vazio ⇒ não instalado / fora do PATH) |
| `IsLfsAvailable()` | versão ok e stdout começa com `git-lfs` |
| `IsLfsInitializedForUser()` | `config --global --get filter.lfs.clean` não-vazio |
| `LfsInstall()` | `lfs install` (inicializa o LFS para o usuário) |

---

## Etapa 2 · Fluxo básico (track / commit / push)

| Método | Comando / lógica |
|---|---|
| `GetTrackedPatterns()` | parseia `lfs track` — ignora linhas "Listing"/"Tracking" e a anotação ` (.gitattributes)`, devolve os globs crus |
| `TrackPattern(p)` | `lfs track "<p>"` |
| `UntrackPattern(p)` | `lfs untrack "<p>"` |
| `GetLfsFiles()` | `lfs ls-files` (mantém a linha inteira: `<oid> <*\|-> <path>`) |
| `Add(pathspec)` | `add -- <pathspec>` (usado para stagear o `.gitattributes` após track/untrack) |
| `Push()` | `push` (fallback quando não há diálogo nativo de push) |

---

## Etapa 3 · Clone & Pull

| Método | Comando |
|---|---|
| `LfsPull()` | `lfs pull` |
| `LfsFetchAll()` | `lfs fetch --all` |
| `LfsCheckout()` | `lfs checkout` |
| `LfsStatus()` | `lfs status` |

---

## Helper estático — `GetRepositoriesFromSettings()`

Popula o dropdown de repositório **independente do host**. Lê:
```
%APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings
```
Carrega o XML, acha o `item` cuja `key` é `"history"`, cujo `value` é uma **string XML aninhada**; parseia essa string e coleta cada `<Path>`; devolve os caminhos distintos (case-insensitive). Tudo em `try/catch` → lista vazia é aceitável. Ver [[../Fluxos/Diretório de Trabalho Independente]].

---

## Relacionado

- [[LfsForm]]
- [[ZimerfeldLfsPlugin]]
- [[../02 - Conhecimento/Git LFS - Conceitos]]
- [[../Sistema/Arquitetura]]
