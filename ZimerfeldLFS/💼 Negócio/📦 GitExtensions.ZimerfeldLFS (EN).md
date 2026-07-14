---
tipo: negocio
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
tags: [projeto, negocio, csharp, gitextensions, plugin, winforms, git-lfs, i18n]
status: ativo
linguagem: C#
versao: 1.0.4
repo: C:\GitExtensions\GitExtensions.ZimerfeldLFS
---

# 📦 GitExtensions.ZimerfeldLFS

## 🎯 Goal
Plugin for **[GitExtensions](https://gitextensions.github.io/)** that manages **Git Large File Storage (LFS)** in a **dedicated, non-modal window**. Git LFS replaces large files (audio, video, datasets) with **lightweight text pointers** inside the repository; the real content lives on a separate remote server — **speeding up cloning and avoiding repository bloat**. The window walks the user through the standard LFS flow in **three steps** (tabs) and points to a **working directory chosen independently of GitExtensions**. See [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]].

## 💜 Funding / Sponsorship
Configured donation channels (badges at the top of the README + **clickable banner at the top of the window**, see [[🪟 LfsForm (EN)|🪟 LfsForm]] / `SponsorBanner.cs`):
- **GitHub Sponsors:** `@zimerfeld` → https://github.com/sponsors/zimerfeld
- **Ko-fi:** `C0D621FCGD` → https://ko-fi.com/C0D621FCGD
- **Native "Sponsor this project" button on GitHub:** enabled by `.github/FUNDING.yml` (`github: zimerfeld` + `ko_fi: C0D621FCGD`) — same pattern as `GitExtensions.ZimerfeldCommitMsg`. Shows the button at the top of the repository page.
- **Sponsorship badges across all READMEs:** bilingual invitation sentence + GitHub Sponsor/Ko-fi badges present in `README.md`, `README.en-US.md` and `README.pt-BR.md`.
- **Social proof in the README:** version badges and **NuGet downloads** (`shields.io/nuget/v` and `/dt`).

## 📂 Repository structure
```
C:\GitExtensions\GitExtensions.ZimerfeldLFS\
├─ src\GitExtensions.ZimerfeldLFS\        # plugin code (.csproj)
│   ├─ ZimerfeldLfsPlugin.cs             # MEF entry point (Execute/Register/Unregister)
│   ├─ LfsForm.cs                        # the window: 3 tabs + repo dropdown + log + i18n
│   ├─ LfsService.cs                     # git / git lfs runner (subprocesses)
│   ├─ Localization.cs                   # I18n + Translator (embedded JSON dictionaries)
│   ├─ SponsorBanner.cs                  # GitHub Sponsors + Ko-fi banner (top of the window)
│   ├─ PluginIcon.cs                     # loads the embedded icon (ico.png)
│   ├─ Resources\                        # ico.png, icon-128.png, badges, ZimerfeldLFS.<culture>.json
│   ├─ *.csproj / *.nuspec               # build + NuGet manifest
├─ refs\                                 # versioned host DLLs (deterministic build)
├─ tools\                                # install/uninstall .ps1, nuget.exe, icon-generator
│   ├─ icon-generator\Generate-LfsIcon.ps1  # GDI+ icon generator
│   └─ net9.0-windows\                   # build output (DLL) used by the nupkg
├─ ZimerfeldLFS\                         # 🧠 this memory vault
├─ build.ps1                             # bumps version + build + deploy + nupkg
├─ README.md / README.pt-BR.md / README.en-US.md  # mirrored in [[📖 README — Instalação, Uso e Build (EN)|📖 README]]
└─ GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg
```

## ⚙️ Technical stack
- **Language:** C# (`net9.0-windows`), `Nullable` + `ImplicitUsings` enabled, `LangVersion=latest`
- **UI:** WinForms (`UseWindowsForms`) — its own window (`LfsForm`); does not use host screens beyond the native commit/push dialogs
- **Output type:** `Library` (DLL loaded by GitExtensions)
- **AssemblyName:** `GitExtensions.Plugins.ZimerfeldLFS`
- **Root namespace:** `GitExtensions.ZimerfeldLFS`
- **NeutralLanguage:** `pt-BR`
- **Plugin model:** MEF (`[Export(typeof(IGitPlugin))]`) — see [[🧩 Plugin MEF para GitExtensions (EN)|🧩 MEF plugin for GitExtensions]]
- **External references** (from `refs\`, `Private=false`): `GitExtensions.Extensibility.dll`, `System.ComponentModel.Composition.dll`

## ✨ Main features
- **Guided three-step flow (tabs):** see [[⚙️ Instalação (EN)|⚙️ Installation]], [[📤 Track Commit Push (EN)|📤 Track Commit Push]], [[⬇️ Clone e Pull (EN)|⬇️ Clone & Pull]].
  1. **Installation** — `git lfs version` (detect) + `git lfs install` (initialize for the user); help mentions Homebrew/Chocolatey/official binaries.
  2. **Basic flow** — track glob patterns (`git lfs track "*.psd"`) and **stage the `.gitattributes`**; list tracked patterns (+ untrack); list LFS files (`git lfs ls-files`); **Commit** and **Push** via the host's native dialogs.
  3. **Clone & Pull** — `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`.
- **Independent working directory:** `cboRepo` dropdown populated from the **GitExtensions repository history** (read from the settings file), without depending on the host window. See [[📂 Diretório de Trabalho Independente (EN)|📂 Independent working directory]].
- **Output console (log):** each button shows the **exact `git`/`git lfs` command** and its output in a dark console — nothing is hidden.
- **Live LFS state:** detected version, whether it is initialized for the user, tracked patterns and LFS files — refreshed automatically after each operation and when switching repos.
- **Localization (PT-BR / EN-US / ES-ES):** automatic mode following the OS + manual override (Automatic/Portuguese/English/Spanish dropdown). See [[Localization.cs|I18n]].
- **Show Debug:** a toggle that displays each control's `Name` as a tooltip (development aid inherited from ZimerfeldTree).

## 🏗️ Architecture (Plugin → Form → Service)
Three classes, each with a single responsibility (see [[🏛️ Arquitetura (EN)|🏛️ Architecture]]):
```
GitExtensions (host)
    │ MEF
    ▼
ZimerfeldLfsPlugin   ← [Export(IGitPlugin)], base(false) = no settings dialog
    │ opens singleton, passes openCommit/openPush delegates (WithWorkingDirectory)
    ▼
LfsForm (the window)  ── uses ──►  LfsService (git / git lfs subprocesses → GitResult)
```
The plugin **subscribes to no host event** — it is fully decoupled. The host working dir is used **only once**, as the pre-selected value of `cboRepo` when opening the window.

## 🛠️ Build / installation
```powershell
# Build: bumps build, compiles Release, deploys (Admin), generates nupkg
.\build.ps1
.\build.ps1 -Force   # always recompiles/packs

# Helper scripts in tools\ (Admin for Program Files)
tools\install.ps1      # installs the plugin (also via Package Manager Console)
tools\uninstall.ps1    # removes it (does not affect the rest of GitExtensions)
```
> Via the **GitExtensions Plugin Manager**: search for *ZimerfeldLFS* and install. Step by step in [[📖 README — Instalação, Uso e Build (EN)|📖 README]] and [[🔢 Versionamento (EN)|🔢 Versioning]].

## 🎨 Icon
Area split into **4 equal quadrants**, each a "file card" of a large-file type: **arcade joystick** (blue), **musical note** with a double beam for audio (purple), movie **play button** (red) and **database cylinders** (teal); at the center, a small **bomb with a lit fuse** (Super Mario Bros style). Generated 100% via GDI+ in `Generate-LfsIcon.ps1` → `icon-128.png` (package/window) and `ico.png` (16×16, menu). See [[🎨 Generate-LfsIcon (EN)|🎨 Generate-LfsIcon]] and [[💣 Ícone 4 quadrantes + bomba (EN)|💣 Icon: 4 quadrants + bomb]].

## 💰 Investment angle
- **Underserved niche:** GitExtensions has no guided LFS screen; users fall back to the command line. This plugin turns an intimidating flow (`track` / `.gitattributes` / `ls-files` / `pull`/`fetch`/`checkout`) into a 3-click experience.
- **Real audience:** game, design, audio/video and data-science teams — exactly those who suffer from bloated repositories and are candidates to adopt LFS.
- **Low marginal cost:** shares the infrastructure (build, i18n, banner, GDI+ icon, versioned refs) of its siblings [[GitExtensions.ZimerfeldTree]] and [[GitExtensions.ZimerfeldCommitMsg]] — a cohesive portfolio reinforcing the **Zimerfeld** brand in the GitExtensions/NuGet ecosystem.
- **Distribution ready:** published on NuGet and visible in the internal Plugin Manager (marker dependency `GitExtensions.Extensibility`).

## 🐛 Known pitfalls
> [!warning] `git` and `git-lfs` must be on the PATH
> `LfsService` runs `git`/`git lfs` as subprocesses. If they are not on the `PATH`, the commands return a `GitResult` with `ExitCode -1` and the Installation tab shows "Git LFS is NOT installed". It is not a crash — it is surfaced as a failed result.

<!-- -->

> [!warning] DLL in the ROOT `lib\` in the nuspec (intentional NU5101 warning)
> The GitExtensions Plugin Manager only extracts a `lib` group whose target framework is in the moniker list (which includes `any`). The DLL goes in the **root** `lib\` (the "any" group); a `lib\net9.0-windows\` subfolder would **not** be extracted. That is why the `nuget pack` **NU5101** warning is **filtered on purpose** in `build.ps1`. See [[🔢 Versionamento (EN)|🔢 Versioning]] and [[🧱 Dependências (EN)|🧱 Dependencies]].

## 🔢 Versioning
- Current version: **1.0.4** (csproj + nuspec synchronized by `build.ps1`)
- Scheme: `major.minor.BUILD`, BUILD auto-incremented on every build
- On every build, `build.ps1` stamps version + date into the **READMEs** (section 4b) **and** into this Obsidian vault (section 4c), keeping everything in sync

## 🔗 Related
- [[📖 README — Instalação, Uso e Build (EN)|📖 README — Installation, Usage and Build]] — mirror of `README.md`
- [[🧩 Plugin MEF para GitExtensions (EN)|🧩 MEF plugin for GitExtensions]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
- [[🏛️ Arquitetura (EN)|🏛️ Architecture]] · [[🔭 Visão Geral (EN)|🔭 Overview]] · [[🔢 Versionamento (EN)|🔢 Versioning]] · [[🧱 Dependências (EN)|🧱 Dependencies]]
- [[GitExtensions.ZimerfeldTree]] — sibling (branch tree)
- [[GitExtensions.ZimerfeldCommitMsg]] — sibling (commit messages)
- [[🔑 Fatos-Chave (EN)|🔑 Key Facts]]
