# GitExtensions.ZimerfeldLFS

![Icon](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/src/GitExtensions.ZimerfeldLFS/Resources/icon-128.png)

**Version:** 1.0.1 — **Updated:** 2026-07-01

Plugin for [GitExtensions](https://gitextensions.github.io/) that manages **Git Large File Storage (LFS)** in a dedicated, non-modal window.

## What is Git LFS?

Git Large File Storage (LFS) is an open-source extension for Git that replaces large files (like audio, video, or datasets) with lightweight text pointers inside your repository. The actual file content is hosted on a separate remote server, speeding up cloning and preventing repository bloat.

## The three steps

The window mirrors the standard Git LFS workflow as three tabs.

### 1 · Installation

- Windows & macOS: Git LFS is typically included out of the box. If you need to install it manually, use Homebrew (`brew install git-lfs`), Chocolatey (`choco install git-lfs`) or the official binaries from [git-lfs.com](https://git-lfs.com).
- **Check installation** runs `git lfs version` and shows the detected status.
- **`git lfs install`** initializes LFS for your user account (runs once per machine).

### 2 · Basic workflow — track / commit / push

Tell Git LFS which file types to manage using glob patterns:

```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```

- Type a pattern and click **Track** — the plugin runs `git lfs track "<pattern>"` and stages `.gitattributes`.
- The **Tracked patterns** list has an *Untrack* button; the **LFS-managed files** list mirrors `git lfs ls-files`.
- **Commit** and **Push** open the native GitExtensions dialogs for the selected repository.

### 3 · Cloning & pulling

When collaborators or deployment tools clone the repository, Git LFS downloads the heavy files automatically as they check out the branch. To fetch or restore LFS content later:

- `git lfs pull` — download LFS content for the current checkout
- `git lfs fetch --all` — prefetch LFS objects for every ref
- `git lfs checkout` — populate working-tree files from downloaded objects
- `git lfs status` — show the status of LFS objects

## Highlights

- **Independent working directory** — a dropdown populated from the GitExtensions repository history.
- **Three-step guided workflow** — Installation, Basic workflow, Cloning & pulling.
- **Live LFS state** — version, initialization, tracked patterns and LFS-managed files, refreshed automatically.
- **Output console** — every button shows the exact command and its output.
- **Localized (English / Portuguese)** with an automatic mode, plus a **Show Debug** toggle.

## Installation

- **Plugin Manager:** search for *ZimerfeldLFS* (Plugins → Plugin Manager), install, restart, then open **Plugins → ZimerfeldLFS**.
- **Manual:** run `build.ps1` as Administrator, or copy `GitExtensions.Plugins.ZimerfeldLFS.dll` into `C:\Program Files\GitExtensions\Plugins\`, or run `tools\install.ps1` as Administrator.

## Requirements

- GitExtensions 6.x (.NET 9)
- `git` and `git-lfs` on the `PATH`

## License

Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (see `LICENSE.txt`).
