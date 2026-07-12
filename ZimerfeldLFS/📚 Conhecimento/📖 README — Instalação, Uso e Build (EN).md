---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, readme, instalacao, build, uso, git-lfs, i18n]
fonte: README.md
versao: 1.0.4
---

# 📖 README — Installation, Usage and Build

> Mirror of the repository root `README.md` (bilingual EN/PT), reconciled with the code on 2026-07-01.
> Project note: [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]]. Concepts in [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]].
> `build.ps1` stamps version + date into the READMEs **and** into this vault on every build (sections 4b/4c) — see [[🔢 Versionamento (EN)|🔢 Versioning]].

Plugin for **[GitExtensions](https://gitextensions.github.io/)** that manages **Git Large File Storage (LFS)** in a **dedicated, non-modal window**. Git LFS replaces large files (audio, video, datasets) with **lightweight text pointers** in the repository; the real content lives on a separate remote server, **speeding up cloning and avoiding repository bloat**. The window drives the flow in **three steps** and uses a working directory chosen **independently of GitExtensions**.

## ✨ High-level features
- **Guided three-step flow** — Installation, Basic flow (track/commit/push) and Clone & Pull, each in its own tab.
- **Independent working directory** — a dropdown populated with the GitExtensions repository history; points the window at any repo without depending on the host. See [[📂 Diretório de Trabalho Independente (EN)|📂 Independent working directory]].
- **Live LFS state** — `git lfs` version, whether it is initialized, tracked glob patterns and LFS files, refreshed automatically.
- **Output console** — each button shows the exact `git`/`git lfs` command and its output.
- **Localized (English / Portuguese / Spanish)** with an automatic mode following the OS, plus **Show Debug** which reveals the control IDs.

## 🧩 What is Git LFS?
An open-source Git extension that swaps large files for lightweight text pointers; the real content lives on a remote server, speeding up cloning and avoiding bloat. Details in [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]].

## 🔢 The three steps

> **Technical operation guides (new in 2026-07-01):** the root `README.md` remains a high-level bilingual summary; the details of **how to operate the window** were split into two per-language manuals — `README.en-US.md` (English) and `README.pt-BR.md` (Portuguese) — **linked from the "The three steps" section** of the root README. Each manual covers: window anatomy, choosing the working directory, reading the output console, each button/status per tab, and troubleshooting.

### 1 · Installation
On Windows and macOS Git LFS usually comes bundled. Manual installation via **Homebrew** (`brew install git-lfs`), **Chocolatey** (`choco install git-lfs`) or the official binaries at [git-lfs.com](https://git-lfs.com). Then **`git lfs install`** initializes LFS for the account (once per machine). **Check installation** runs `git lfs version`. See [[⚙️ Instalação (EN)|⚙️ Installation]].

### 2 · Basic flow — track / commit / push
Track types by **glob patterns** (`*.psd`, `*.mp4`, `*.zip`): type the pattern and click **Track** — the plugin runs `git lfs track "<pattern>"` and **stages the `.gitattributes`**. The window lists the tracked patterns (with *Untrack*) and the LFS files (`git lfs ls-files`). **Commit** and **Push** open the native GitExtensions dialogs.
```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```
See [[📤 Track Commit Push (EN)|📤 Track Commit Push]].

### 3 · Clone & Pull
When cloning, Git LFS downloads the heavy files automatically on checkout. To fetch/restore later: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`. See [[⬇️ Clone e Pull (EN)|⬇️ Clone & Pull]].

## 📦 Installation
**Via GitExtensions Plugin Manager:** search for *ZimerfeldLFS* (Plugins → Plugin Manager), install, restart and open **Plugins → ZimerfeldLFS**.

**Manual:** run `build.ps1` (as Administrator for automatic deploy) or copy `GitExtensions.Plugins.ZimerfeldLFS.dll` into `C:\Program Files\GitExtensions\Plugins\`, or run `tools\install.ps1` as Administrator.

## ✅ Requirements
- GitExtensions 6.x (.NET 9)
- `git` and `git-lfs` on the `PATH`

## 🛠️ Build
```powershell
pwsh .\build.ps1          # bumps version, builds Release, packs the .nupkg
pwsh .\build.ps1 -Force   # always recompiles/packs
```
See [[🔢 Versionamento (EN)|🔢 Versioning]] and [[🛠️ build.ps1 (EN)|🛠️ build.ps1]].

## 💜 Support the project
**GitHub Sponsors:** [github.com/sponsors/zimerfeld](https://github.com/sponsors/zimerfeld) · **Ko-fi:** [ko-fi.com/C0D621FCGD](https://ko-fi.com/C0D621FCGD). Badges at the top of the README and a **clickable banner at the top of the window** (`SponsorBanner.cs`).

## 🧩 Integrated plugins
Other GitExtensions plugins from the same author, referenced in the footer of the READMEs:
- **[GitExtensions.ZimerfeldTree](https://github.com/zimerfeld/GitExtensions.ZimerfeldTree)** — see [[GitExtensions.ZimerfeldTree]]
- **[GitExtensions.ZimerfeldCommitMsg](https://github.com/zimerfeld/GitExtensions.ZimerfeldCommitMsg)** — see [[GitExtensions.ZimerfeldCommitMsg]]

## 📄 License
Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (`LICENSE.txt`).

## 🔗 Related
- [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
