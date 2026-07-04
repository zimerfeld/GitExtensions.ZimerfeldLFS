---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [arquitetura, classes, design, i18n, threading]
---

# 🏛️ Architecture

## 🗺️ Class diagram

```
GitExtensions (host)
    │
    │  MEF (System.ComponentModel.Composition)
    ▼
ZimerfeldLfsPlugin        ← [Export(IGitPlugin)] : GitPluginBase, base(false)
    │  Execute()  → opens/brings-to-front the singleton window
    │  captures _commands (Register/Unregister)
    │  passes delegates: openCommit / openPush  (via WithWorkingDirectory)
    ▼
LfsForm (the window)      ← FixedSingle, non-modal, 3 tabs + log + i18n
    │  cboRepo (independent dropdown)  →  WorkingDir
    │  RunAsync(title, op)  →  Task.Run
    ▼
LfsService                ← git / git lfs subprocesses
    │  RunGit(args) → GitResult(StdOut, StdErr, ExitCode)
    ▼
git / git lfs (PATH)
```

## 🧱 The three classes

### 🔌 `ZimerfeldLfsPlugin` — entry point
Inherits from `GitPluginBase`, exported via MEF. Constructor `: base(false)` → **no** settings dialog under Settings → Plugins. See [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]].
- **`Execute`** — opens (or brings to front) the **singleton window** `LfsForm`; passes the `openCommit`/`openPush` delegates that open the host's native dialogs **on the working dir selected in `cboRepo`** (`_commands.WithWorkingDirectory(dir)` → `StartCommitDialog`/`StartPushDialog`). Returns `false` (the host should **not** refresh its own UI).
- **`Register`/`Unregister`** — capture/clear `_commands` (`IGitUICommands`).

### 🪟 `LfsForm` — the window
WinForms `FixedSingle`, non-maximizable, `CenterScreen`, Segoe UI 9. Top to bottom: sponsor banner, independent repo dropdown + branch, 3-tab `TabControl`, dark log console, bottom panel (Debug + Language + Close), status strip. See [[🪟 LfsForm (EN)|🪟 LfsForm]].

### ⚡ `LfsService` — git/git-lfs runner
`RunGit(args)` runs `git` in a `ProcessStartInfo` (redirecting stdout/stderr, UTF-8, no window) and returns a `GitResult(StdOut, StdErr, ExitCode)` (record struct; `Ok` = exit 0; `Combined` = trimmed stdout+stderr). State methods, the 3-step methods, and the static helper `GetRepositoriesFromSettings`. See [[⚡ LfsService (EN)|⚡ LfsService]].

## 🔗 Decoupling from the host

> [!important] The plugin subscribes to **NO** host events
> Unlike its sibling CommitMsg (which listens to `PostRepositoryChanged`), ZimerfeldLFS is **fully decoupled**: the window chooses the repository **exclusively** through its own `cboRepo`. The host's working dir is used **only once**, as the pre-selected value when the window opens. `Register` only stores `_commands` so it can open the native dialogs; there is no re-binding nor `Application.Idle`. See [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]].

The only touchpoint with the host is optional: the `openCommit`/`openPush` delegates, which resolve `IGitUICommands.WithWorkingDirectory(dir)` to open the native dialogs on the chosen repo. If unavailable, the log records the fallback (Push falls back to plain `git push`).

## 🌐 Localization (i18n)

`I18n` (static) resolves the language (`Automatic` follows the OS via `CultureInfo.CurrentUICulture`; otherwise `en-US`/`pt-BR`) and loads the dictionary from the **embedded JSON** `Resources\ZimerfeldLFS.<culture>.json` via `GetManifestResourceStream`, with en-US fallback → empty map. A `Translator` returns the string by key (or the key itself if missing) and has `F(key, args)` for `string.Format`. The choice is read from disk **once** at start and afterwards driven in memory by the dropdown; persisted at `%APPDATA%\GitExtensions\ZimerfeldLFS.language.json`. See [[🪟 LfsForm (EN)|🪟 LfsForm]] and `Localization.cs`.

## 🧵 Threading

> The window **opens instantly**: the constructor does **zero git work** (it only populates `cboRepo` by reading the settings XML). The first probe runs after the `Shown` event.

- **`RefreshStateAsync`** — probes the repository in a background `Task.Run` (lfs version, available?, initialized?, branch, patterns, files → `StateSnapshot`) and applies the result to the UI (`ApplyState`). Called on `Shown`, when switching repos in `cboRepo`, and after operations.
- **`RunAsync(title, op, refreshAfter)`** — runs a git/git-lfs operation in `Task.Run`, logs the command and its output, and (optionally) calls `RefreshStateAsync`. Concurrent calls are ignored while `_busy`.
- **`Log`** marshals to the UI thread with `BeginInvoke` when `InvokeRequired`.

## 🔗 Related

- [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]]
- [[🪟 LfsForm (EN)|🪟 LfsForm]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
- [[🪟 Janela dedicada não-modal (EN)|🪟 Dedicated non-modal window]]
- [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]]
- [[🧱 Dependências (EN)|🧱 Dependencies]]
