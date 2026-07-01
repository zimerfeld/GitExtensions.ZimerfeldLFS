# GitExtensions.ZimerfeldLFS

![Icon](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/src/GitExtensions.ZimerfeldLFS/Resources/icon-128.png)

[![NuGet version](https://img.shields.io/nuget/v/GitExtensions.ZimerfeldLFS?style=for-the-badge&logo=nuget&label=NuGet)](https://www.nuget.org/packages/GitExtensions.ZimerfeldLFS/) &nbsp; [![NuGet downloads](https://img.shields.io/nuget/dt/GitExtensions.ZimerfeldLFS?style=for-the-badge&logo=nuget&label=Downloads)](https://www.nuget.org/packages/GitExtensions.ZimerfeldLFS/)

[![GitHub Sponsor](https://img.shields.io/badge/Sponsor-zimerfeld-EA4AAA?style=for-the-badge&logo=githubsponsors&logoColor=white)](https://github.com/sponsors/zimerfeld) &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [![Ko-fi](https://img.shields.io/badge/Ko--fi-Buy%20me%20a%20coffee-FF5E2B?style=for-the-badge&logo=ko-fi&logoColor=white)](https://ko-fi.com/C0D621FCGD)

> 🇬🇧 **Version:** 1.0.0 — **Updated:** 2026-07-01

> 🇧🇷 **Versão:** 1.0.0 — **Atualizado em:** 2026-07-01

> **Versão atual: 1.0.0**

---

> 🇬🇧 Plugin for [GitExtensions](https://gitextensions.github.io/) that manages **Git Large File Storage (LFS)** in a dedicated, non-modal window. Git LFS replaces large files (audio, video, datasets) with lightweight text pointers inside your repository; the real content lives on a separate remote server, **speeding up cloning and preventing repository bloat**. The window walks you through the LFS workflow in three steps and targets a working directory chosen **independently of the GitExtensions host**.

> 🇧🇷 Plugin para o [GitExtensions](https://gitextensions.github.io/) que gerencia o **Git Large File Storage (LFS)** em uma janela dedicada e não-modal. O Git LFS substitui arquivos grandes (áudio, vídeo, datasets) por ponteiros de texto leves no repositório; o conteúdo real fica em um servidor remoto separado, **acelerando o clone e evitando o inchaço do repositório**. A janela conduz o fluxo de LFS em três etapas e usa um diretório de trabalho escolhido **de forma independente do GitExtensions**.

## What is Git LFS? / O que é o Git LFS?

> 🇬🇧 Git Large File Storage (LFS) is an open-source extension for Git that replaces large files (like audio, video, or datasets) with lightweight text pointers inside your repository. The actual file content is hosted on a separate remote server, speeding up cloning and preventing repository bloat.

> 🇧🇷 O Git Large File Storage (LFS) é uma extensão open-source do Git que substitui arquivos grandes (como áudio, vídeo ou datasets) por ponteiros de texto leves dentro do repositório. O conteúdo real do arquivo é hospedado em um servidor remoto separado, acelerando o clone e evitando o inchaço do repositório.

## The three steps / As três etapas

The window mirrors the standard Git LFS workflow as three tabs / A janela espelha o fluxo padrão do Git LFS em três abas:

### 1 · Installation / Instalação

> 🇬🇧 On Windows & macOS, Git LFS is typically included out of the box. If you need to install it manually, use Homebrew (`brew install git-lfs`), Chocolatey (`choco install git-lfs`) or the official binaries from [git-lfs.com](https://git-lfs.com). Then initialize LFS for your user account by clicking **`git lfs install`** (runs once per machine). The **Check installation** button runs `git lfs version` and shows the detected status.

> 🇧🇷 No Windows e no macOS, o Git LFS normalmente já vem incluído. Se precisar instalar manualmente, use o Homebrew (`brew install git-lfs`), o Chocolatey (`choco install git-lfs`) ou os binários oficiais em [git-lfs.com](https://git-lfs.com). Depois, inicialize o LFS para a sua conta clicando em **`git lfs install`** (executa uma vez por máquina). O botão **Verificar instalação** roda `git lfs version` e mostra o status detectado.

### 2 · Basic workflow — track / commit / push / Fluxo básico

> 🇬🇧 Tell Git LFS which file types to manage using **glob patterns** (e.g. `*.psd`, `*.mp4`, `*.zip`). Type a pattern and click **Track** — the plugin runs `git lfs track "<pattern>"` and stages the updated `.gitattributes`. The window lists the **tracked patterns** (with an *Untrack* button) and the **LFS-managed files** (`git lfs ls-files`). Then **Commit** and **Push** open the native GitExtensions dialogs for the selected repository.

```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```

> 🇧🇷 Informe ao Git LFS quais tipos de arquivo gerenciar usando **padrões glob** (ex.: `*.psd`, `*.mp4`, `*.zip`). Digite um padrão e clique em **Rastrear** — o plugin executa `git lfs track "<padrão>"` e adiciona o `.gitattributes` ao stage. A janela lista os **padrões rastreados** (com botão *Deixar de rastrear*) e os **arquivos gerenciados pelo LFS** (`git lfs ls-files`). Em seguida, **Commit** e **Push** abrem os diálogos nativos do GitExtensions para o repositório selecionado.

### 3 · Cloning & pulling / Clone & Pull

> 🇬🇧 When collaborators or deployment tools clone the repository, Git LFS downloads the heavy files automatically as they check out the branch. To fetch or restore LFS content later, use the buttons: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout` and `git lfs status`.

> 🇧🇷 Quando colaboradores ou ferramentas de deploy clonam o repositório, o Git LFS baixa os arquivos pesados automaticamente ao fazer checkout da branch. Para buscar ou restaurar o conteúdo LFS depois, use os botões: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout` e `git lfs status`.

## Highlights / Destaques

**🇬🇧 English**

- **Independent working directory** — a dropdown populated from the GitExtensions repository history lets you point the window at any repo, regardless of what the host window is doing.
- **Three-step guided workflow** — Installation, Basic workflow (track/commit/push) and Cloning & pulling, each as its own tab.
- **Live LFS state** — detected `git lfs` version, whether it is initialized for your user, the tracked glob patterns and the LFS-managed files, refreshed automatically.
- **Output console** — every button shows the exact `git`/`git lfs` command and its output, so nothing is hidden.
- **Localized (English / Portuguese)** with an automatic mode following the OS, plus a **Show Debug** toggle that reveals control IDs.

**🇧🇷 Português**

- **Diretório de trabalho independente** — um dropdown preenchido com o histórico de repositórios do GitExtensions permite apontar a janela para qualquer repo, sem depender da janela do host.
- **Fluxo guiado em três etapas** — Instalação, Fluxo básico (track/commit/push) e Clone & Pull, cada um em sua aba.
- **Estado do LFS ao vivo** — versão do `git lfs` detectada, se está inicializado para o usuário, os padrões glob rastreados e os arquivos gerenciados pelo LFS, atualizados automaticamente.
- **Console de saída** — cada botão mostra o comando `git`/`git lfs` exato e sua saída, sem esconder nada.
- **Localizado (Inglês / Português)** com modo automático seguindo o SO, além do **Mostrar Debug** que revela os IDs dos controles.

## Installation / Instalação

> 🇬🇧 **Via GitExtensions Plugin Manager:** search for *ZimerfeldLFS* in the in-app Plugin Manager (Plugins → Plugin Manager) and install. Restart GitExtensions and open **Plugins → ZimerfeldLFS**.

> 🇧🇷 **Pelo Gerenciador de Plugins do GitExtensions:** procure por *ZimerfeldLFS* no Plugin Manager interno (Plugins → Plugin Manager) e instale. Reinicie o GitExtensions e abra **Plugins → ZimerfeldLFS**.

> 🇬🇧 **Manual:** run `build.ps1` (as Administrator to auto-deploy) or copy `GitExtensions.Plugins.ZimerfeldLFS.dll` into `C:\Program Files\GitExtensions\Plugins\`, or run `tools\install.ps1` as Administrator.

> 🇧🇷 **Manual:** execute o `build.ps1` (como Administrador para deploy automático) ou copie `GitExtensions.Plugins.ZimerfeldLFS.dll` para `C:\Program Files\GitExtensions\Plugins\`, ou execute `tools\install.ps1` como Administrador.

## Requirements / Requisitos

- GitExtensions 6.x (.NET 9) / GitExtensions 6.x (.NET 9)
- `git` and `git-lfs` on the `PATH` / `git` e `git-lfs` no `PATH`

## Build

```powershell
pwsh .\build.ps1          # increments version, builds Release, packs the .nupkg
pwsh .\build.ps1 -Force   # always rebuild/pack
```

## License / Licença

Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (see `LICENSE.txt`).
