---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, csharp, gitextensions, mef, plugin]
---

# 🧩 MEF plugin for GitExtensions

## 📌 Summary
GitExtensions loads plugins via **MEF** (Managed Extensibility Framework). The entry point is an exported class that implements `IGitPlugin` (usually inheriting from `GitPluginBase`).

## 🔑 Key points
- Export with `[Export(typeof(IGitPlugin))]` using `System.ComponentModel.Composition`.
- The project compiles as a **`Library`** (DLL), `net9.0-windows`, with WinForms enabled.
- Reference the host assemblies with **`<Private>false</Private>`** (do not copy to output — the host already has them). In ZimerfeldLFS they are **versioned under `refs\`** (deterministic, offline build):
  - `GitExtensions.Extensibility.dll`
  - `System.ComponentModel.Composition.dll`
- The `AssemblyName` must match what `install.ps1` / nuspec expect (`GitExtensions.Plugins.<Name>`).
- To appear in the internal **Plugin Manager**, the NuGet package must **depend** on `GitExtensions.Extensibility` (marker dependency). See [[🧱 Dependências (EN)|🧱 Dependencies]].

## ♻️ Plugin lifecycle
- `Register(IGitUICommands)` — called on load. A good place to **capture the `IGitUICommands`** (used later to open native dialogs) and, if desired, subscribe to events (`PostRepositoryChanged`).
- `Unregister(IGitUICommands)` — unsubscribe events / clear the captured commands.
- `Execute(GitUIEventArgs)` — triggered by the **Plugins → \<name\>** menu. Access to `GitModule.WorkingDir`, `StartCommitDialog`, `StartPushDialog`, etc.

## 🧭 How ZimerfeldLFS uses this model
- `: base(false)` in the constructor → **no** settings dialog (does not implement `GetSettings()`).
- `Execute` opens a **non-modal singleton window** (`LfsForm`) instead of a modal dialog, and returns `false` (host does not refresh its own UI). See [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]] and [[🪟 Janela dedicada não-modal (EN)|🪟 Dedicated non-modal window]].
- **Decoupled:** `Register` only **captures** the `IGitUICommands`; the plugin **subscribes to no host events**. The target repository comes from the window's own dropdown. See [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]].
- The native commit/push dialogs are opened via `IGitUICommands.WithWorkingDirectory(dir)` → `StartCommitDialog` / `StartPushDialog`, on the selected repo.

## 🔗 Related
- [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]]
- [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]]
- [[GitExtensions.ZimerfeldTree]]
- [[GitExtensions.ZimerfeldCommitMsg]]
