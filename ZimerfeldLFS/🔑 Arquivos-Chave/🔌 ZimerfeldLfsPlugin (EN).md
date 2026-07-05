---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [arquivo, plugin, entry-point, mef, winforms]
arquivo: src/GitExtensions.ZimerfeldLFS/ZimerfeldLfsPlugin.cs
---

# 🔌 ZimerfeldLfsPlugin.cs

The plugin's entry point. Exported via MEF to GitExtensions.

**Path:** `src/GitExtensions.ZimerfeldLFS/ZimerfeldLfsPlugin.cs`

---

## 📜 Declaration

```csharp
[Export(typeof(IGitPlugin))]
public sealed class ZimerfeldLfsPlugin : GitPluginBase
```

The `[Export]` attribute is the discovery point for the host's MEF. See [[🧩 Plugin MEF para GitExtensions (EN)|🧩 MEF Plugin for GitExtensions]].

---

## 🏗️ Constructor — `: base(false)`

`base(false)` = the plugin has **no** configurable settings in the GitExtensions dialog (no node appears under Settings → Plugins). Defines:
- `Name = "ZimerfeldLFS"`
- `Description` — bilingual text explaining LFS and the 3 steps
- `Icon = PluginIcon.ForMenu()` — embedded 16×16 icon

---

## 🧾 Instance fields

| Field | Type | Purpose |
|---|---|---|
| `_form` | `LfsForm?` | **Singleton** window — one per GitExtensions session |
| `_commands` | `IGitUICommands?` | Current commands; updated by `Register`/`Unregister`. Used to open the native dialogs on the `cboRepo` repository |

---

## ⚙️ Methods (IGitPlugin)

### `Execute(GitUIEventArgs)` ← menu Plugins → ZimerfeldLFS
- Reads `args.GitModule?.WorkingDir` (only as the initial value).
- If the window does not exist (or was disposed), **creates** `LfsForm(workDir, notifyChanged, openCommit, openPush)` and subscribes to `FormClosed` to reset `_form`. Otherwise, calls `UpdateWorkingDir(workDir)`.
- `Show()` + `BringToFront()`.
- **Returns `false`** — the host should **not** refresh its own UI; the window manages its own state.

**Delegates passed to the window:**
- `notifyChanged` = `() => args.GitUICommands?.RepoChangedNotifier?.Notify()` — asks the host to refresh after commit/checkout.
- `openCommit(owner, workingDir)` → resolves `_commands.WithWorkingDirectory(workingDir)` and calls `StartCommitDialog(owner, "", false)`. Returns `bool?` (`null` = unavailable). Wrapped in `try/catch`.
- `openPush(owner, workingDir)` → `WithWorkingDirectory` + `StartPushDialog(owner, pushOnShow:true, forceWithLease:false, out pushCompleted)`. Returns `pushCompleted`. Wrapped in `try/catch`.

### `Register(IGitUICommands)`
- `base.Register(commands)` + captures `_commands = commands`.
- **Subscribes to no host events.** The comment in the code is explicit: the window's repository comes **exclusively** from `cboRepo`; the host's working dir is used only as the pre-selected value on opening. See [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]].

### `Unregister(IGitUICommands)`
- `_commands = null` + `base.Unregister(commands)`.

---

## 🩺 Diagnostic logging

Each lifecycle event (`Execute`/`Register`/`Unregister`) writes a line with a timestamp and an incremental `_instanceId` to `%APPDATA%\GitExtensions\ZimerfeldLFS.debug.log`. Best-effort, wrapped in `try/catch` — it **never** brings the plugin down.

---

## 🛡️ Crash protection

All delegates and the log are wrapped in `try/catch` — exceptions in the plugin never bring GitExtensions down.

---

## 🔗 Related

- [[🪟 LfsForm (EN)|🪟 LfsForm]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
- [[🏛️ Arquitetura (EN)|🏛️ Architecture]]
- [[🪟 Janela dedicada não-modal (EN)|🪟 Dedicated non-modal window]]
- [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]]
