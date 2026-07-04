---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [fluxo, working-dir, repositorio, dropdown, desacoplamento]
---

# 📂 Flow: Independent Working Directory

How the `cboRepo` dropdown is populated and why the window stays **independent of the host's active repository**.

## 🌱 Where the items come from

```
LfsForm (constructor) → LoadRepositories()
        │
        ▼
LfsService.GetRepositoriesFromSettings()   (static helper)
        │  reads:  %APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings
        │  finds the item key="history" → value is a NESTED XML string
        │  parses that string → collects each <Path>
        ▼
list of distinct paths (case-insensitive)
        │
        ▼
cboRepo.Items  (+ the initial WorkingDir inserted at the top, if not in the list)
        │
        ▼
pre-selects the initial WorkingDir (or index 0)
```

## 🔗 Why it is independent

> [!important] No host events
> The plugin does **not** subscribe to `PostRepositoryChanged` or any other GitExtensions event. The host's working dir (`args.GitModule.WorkingDir`) is used **only once**, merely as the pre-selected value of `cboRepo` when the window opens. From then on, the repository is chosen **exclusively** through the dropdown. See [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]].

## 🔄 Switching repositories

- **`CboRepo_SelectedIndexChanged`** — when another item is chosen, sets `_svc.WorkingDir = dir` and triggers `RefreshStateAsync` (re-probes the lfs version, branch, patterns and files of the new repo).
- **`UpdateWorkingDir(newDir)`** — when the window is already open and the user re-triggers the Plugins menu on a different repo, the plugin calls this: adds the dir to the combo if needed and selects it.

## ✨ Advantage

The window is **persistent and reusable**: you can leave it open and manage LFS for **any** repository in the history, without switching the active repository in GitExtensions' main window.

## 🔗 Related

- [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]]
- [[⚡ LfsService (EN)|⚡ LfsService]] — `GetRepositoriesFromSettings`
- [[🪟 LfsForm (EN)|🪟 LfsForm]] — `LoadRepositories` / `cboRepo`
