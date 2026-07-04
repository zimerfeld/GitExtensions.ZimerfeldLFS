---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [fluxo, instalação, git-lfs, etapa1]
---

# ⚙️ Flow: Step 1 — Installation

First tab of the window (`tabInstall`). Detects Git LFS and initializes it for the user's account.

![[ScreenshotInstall.png]]

## 👣 Steps

```
User opens the "1 · Installation" tab
        │
        ▼
[Check installation]  → RunAsync → _svc.GetLfsVersion()   (git lfs version)
        │
        ▼
RefreshStateAsync probes: IsLfsAvailable() / IsLfsInitializedForUser()
        │
        ├─ NOT available → red status "Git LFS is NOT installed or off the PATH"
        │                    ("git lfs install" button disabled)
        ├─ available, not initialized → gold status "… Click git lfs install"
        └─ ready → green status "Git LFS ready — <version> (initialized for this user)"
        │
        ▼
[git lfs install]  → RunAsync("git lfs install") → _svc.LfsInstall()   (runs once per machine)
```

## 🔍 Details

- **`GetLfsVersion()`** = `git lfs version`; empty stdout ⇒ not installed / off the PATH.
- **`IsLfsAvailable()`** = version ok and stdout starts with `git-lfs`.
- **`IsLfsInitializedForUser()`** = `git config --global --get filter.lfs.clean` non-empty (`git lfs install` writes the LFS filters to the global config).
- **Help on the tab** (`step1Help`): on Windows and macOS Git LFS usually comes bundled; manual installation via **Homebrew** (`brew install git-lfs`), **Chocolatey** (`choco install git-lfs`) or the official binaries at [git-lfs.com](https://git-lfs.com).

## ✅ State after the step

With LFS ready and a valid repository selected, the buttons of steps 2 and 3 become enabled (`workflowEnabled = IsRepo && LfsAvailable`).

## 🔗 Related

- [[📤 Track Commit Push (EN)|📤 Track Commit Push]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
