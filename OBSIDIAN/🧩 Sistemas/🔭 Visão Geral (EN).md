---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [sistema, overview, plugin, gitextensions, git-lfs, i18n]
---

# 🔭 Overview

## 🎯 What it is

Plugin for **GitExtensions** (Windows) that manages **Git Large File Storage (LFS)** in a **dedicated, non-modal window**. It guides the user through the standard LFS workflow in **three steps** (tabs) and operates on a **repository chosen from the window's own dropdown**, independent of the host. See [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]] and [[🪟 Janela dedicada não-modal (EN)|🪟 Dedicated non-modal window]].

## ⚙️ Stack

| Item | Value |
|---|---|
| Language | C# (.NET 9) |
| Target | `net9.0-windows` |
| UI Framework | Windows Forms (`UseWindowsForms`) |
| Output type | `Library` (DLL loaded by GitExtensions) |
| Output assembly | `GitExtensions.Plugins.ZimerfeldLFS.dll` |
| Namespace | `GitExtensions.ZimerfeldLFS` |
| Current version | `1.0.2` |
| Languages | Portuguese-BR / English (auto via OS + override) |
| License | CC BY-NC-ND 4.0 © 2026 Zimerfeld |
| Author | Zimerfeld |

> The plugin displays an **icon** (PNG embedded at `Resources/ico.png`, 16×16) in the Plugins menu and the window title bar. Loaded by `PluginIcon` (lazy cache). See [[🎨 Generate-LfsIcon (EN)|🎨 Generate-LfsIcon]].

## 🗂️ The three steps (tabs)

### 1 · Installation
Detects `git lfs` (`git lfs version`) and initializes it for the user's account (`git lfs install`, runs once per machine). The help text mentions Homebrew, Chocolatey and the official binaries from [git-lfs.com](https://git-lfs.com). See [[⚙️ Instalação (EN)|⚙️ Installation]].

### 2 · Basic workflow (track / commit / push)
Tracks file types by **glob patterns** (`git lfs track "*.psd"`) and **stages `.gitattributes`**; lists the tracked patterns (with *Untrack*) and the LFS files (`git lfs ls-files`); **Commit** and **Push** open GitExtensions' **native** dialogs for the selected repository. See [[📤 Track Commit Push (EN)|📤 Track Commit Push]].

### 3 · Clone & Pull
Downloads/restores LFS content: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`. See [[⬇️ Clone e Pull (EN)|⬇️ Clone and Pull]].

## 🔗 Cross-cutting features

| Feature | Detail |
|---|---|
| Independent directory | `cboRepo` populated from the GitExtensions history — see [[📂 Diretório de Trabalho Independente (EN)|📂 Independent Working Directory]] |
| Output console | shows the exact command + stdout/stderr (dark background, Consolas font) |
| Localization | embedded JSON per language; choice persisted at `%APPDATA%\GitExtensions\ZimerfeldLFS.language.json` |
| Show Debug | tooltip with each control's `Name`; persisted in `ZimerfeldLFS.uisettings.json` |
| Threading | window opens instantly; first probe on `Shown` in a background thread |

## ✅ Runtime requirements

- **GitExtensions 6.x** (.NET 9)
- **`git`** and **`git-lfs`** on the `PATH`

## 🔗 Related

- [[🏛️ Arquitetura (EN)|🏛️ Architecture]]
- [[🔢 Versionamento (EN)|🔢 Versioning]]
- [[🧱 Dependências (EN)|🧱 Dependencies]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
