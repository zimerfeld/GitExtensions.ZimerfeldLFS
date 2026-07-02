# GitExtensions.ZimerfeldLFS — Technical Operation Guide (English)

![Icon](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/src/GitExtensions.ZimerfeldLFS/Resources/icon-128.png)

**Version:** 1.0.2 — **Updated:** 2026-07-01

> This document is the detailed, step-by-step manual for **operating** the plugin window.
> For a high-level overview see the [main README](README.md) · Portuguese version: [README.pt-BR.md](README.pt-BR.md).

---

## Window anatomy

When you open **Plugins → ZimerfeldLFS**, a non-modal, fixed-size window (720 px wide) appears with the following regions, top to bottom:

| Region | What it does |
| --- | --- |
| **Sponsor banner** | Links to GitHub Sponsors / Ko-fi and the *About* dialog. |
| **Working Directory** dropdown | Selects the repository every command runs against — chosen **independently** of the GitExtensions host. |
| **Branch** label | Shows the currently checked-out branch of the selected repo (or *not a git repository*). |
| **Tabs** | The three-step workflow: *Installation*, *Basic workflow*, *Cloning & pulling*. |
| **Output** console | A dark, read-only, monospaced log echoing every command and its raw output. |
| **Bottom bar** | *Show Debug* toggle, **language** selector, and **Close**. |
| **Status strip** | *Ready* / *Running <command>…* / *Refreshing…* feedback. |

The window does **no** git work while opening, so it appears instantly. The first probe (LFS version, tracked patterns, files, branch) runs on a background thread right after the window is shown.

## Choosing the working directory

The **Working Directory** dropdown is populated from the GitExtensions repository history (read from `GitExtensions.settings`), so any repo you have opened in the host is available here — regardless of which repo the host window currently shows.

- Selecting a different entry immediately re-probes that repository and refreshes every tab.
- If the host switches repositories while the window is open, the dropdown follows along.
- If the selected path is not a git work tree, the **Branch** label reads *not a git repository* and the workflow buttons are disabled.

## Reading the Output console

Every button routes through a single runner that:

1. Echoes the command with a `$ ` prefix (e.g. `$ git lfs pull`).
2. Prints the combined stdout + stderr of the process.
3. Ends with `✓ Done.` on success (exit code 0) or `✗ Failed (exit code N).` on failure.

While a command runs, the working-directory panel and tabs are disabled, the cursor shows the wait cursor, and the status strip reads *Running <command>…*. Concurrent clicks are ignored until the current command finishes. Use **Clear** to empty the log.

---

## Step 1 · Installation

![ZimerfeldLFS — Installation tab](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotInstallation.png)

This tab confirms Git LFS is present and initialized for your user account.

**Status line (top, colored):**

| Color | Meaning |
| --- | --- |
| 🟢 Dark green | LFS **ready** — detected (e.g. `git-lfs/3.7.1`) *and* initialized for your user. |
| 🟡 Dark goldenrod | LFS **available** but not yet initialized — click `git lfs install`. |
| 🔴 Dark red | LFS **missing** — not found on the `PATH`. |

"Initialized for your user" is detected by checking that `filter.lfs.clean` exists in your global git config (what `git lfs install` writes).

**Buttons:**

- **Check installation** → runs `git lfs version` and updates the status line.
- **`git lfs install`** → runs `git lfs install`, configuring the LFS filters for your user account. Runs **once per machine**; it is disabled when LFS is missing.

**Installing Git LFS manually** (only if the status is red):

- **Windows / macOS** — usually bundled with Git already.
- **macOS / Linux** — `brew install git-lfs`
- **Windows** — `choco install git-lfs`
- **Any OS** — official binaries at [git-lfs.com](https://git-lfs.com).

---

## Step 2 · Basic workflow — track / commit / push

![ZimerfeldLFS — Basic Workflow tab](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotBasicWorkflow.png)

This tab is the day-to-day loop: tell LFS which files to manage, then commit and push.

### Track a pattern

Type a **glob pattern** in the text box and press **Track** (or hit <kbd>Enter</kbd>). The plugin runs:

```bash
git lfs track "<pattern>"
git add .gitattributes   # staged automatically on success
```

Common patterns: `*.psd`, `*.mp4`, `*.zip`, `*.bin`, `assets/**`. An empty pattern is rejected with a prompt.

### Tracked patterns list + Untrack

The **Tracked patterns** list mirrors `git lfs track` (with the trailing `(.gitattributes)` annotation stripped). Select an entry and press **Untrack** to run:

```bash
git lfs untrack "<pattern>"
git add .gitattributes   # staged automatically on success
```

Untrack with no selection is rejected with a prompt.

### LFS-managed files

The **LFS-managed files** list mirrors `git lfs ls-files` — each line is `<oid> <*|-> <path>` (where `*` means the file is checked out locally, `-` means it is a pointer only). The label shows the count and refreshes after every command.

### Commit & Push

- **Commit…** opens the **native GitExtensions commit dialog** in-process (so all commit plugins load) for the selected repo. The console logs whether commits were made, the dialog was closed without committing, or the dialog was unavailable.
- **Push…** opens the **native GitExtensions push dialog**. If no host push dialog is available (or it is cancelled), the plugin falls back to a plain `git push` on the current branch and logs the fallback.

The full manual equivalent of this loop:

```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```

All *track / untrack / commit / push* controls are disabled unless the working directory is a git repo **and** LFS is available.

---

## Step 3 · Cloning & pulling

![ZimerfeldLFS — Cloning & Pulling tab](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotCloningPulling.png)

When collaborators or deployment tools clone the repository, Git LFS downloads the heavy files automatically as the branch is checked out. To fetch or restore LFS content later, each button maps to exactly one command, always run against the repo in the **Working Directory** dropdown:

| Button | Command | Purpose |
| --- | --- | --- |
| **git lfs pull** | `git lfs pull` | Download LFS content for the current checkout. |
| **git lfs fetch --all** | `git lfs fetch --all` | Prefetch LFS objects for **every** ref (does not touch the work tree). |
| **git lfs checkout** | `git lfs checkout` | Populate working-tree files from already-downloaded objects. |
| **git lfs status** | `git lfs status` | Show the status of LFS objects (does **not** trigger a state refresh). |

A typical restore sequence after a fresh clone that skipped LFS objects: **git lfs fetch --all** → **git lfs checkout**, or simply **git lfs pull**.

---

## Bottom bar

- **Show Debug** — reveals each control's internal `Name` as a tooltip (a diagnostics aid); the choice is persisted to `ZimerfeldLFS.uisettings.json`.
- **Language** — *Automatic* (follows the OS), *English*, or *Portuguese*. Changing it re-labels the entire window immediately.
- **Close** — closes the window (also the <kbd>Esc</kbd> / Cancel action). The window is non-modal, so GitExtensions stays usable while it is open.

## Troubleshooting

| Symptom | Likely cause & fix |
| --- | --- |
| Status line is red / *missing* | Git LFS not on `PATH`. Install it (see Step 1), reopen the tab, click **Check installation**. |
| Workflow buttons greyed out | The selected path is not a git repo, or LFS is unavailable. Pick a valid repo in the dropdown. |
| **Branch** reads *not a git repository* | The dropdown points at a folder that is not a git work tree. |
| Commit says *unavailable* | The native GitExtensions commit dialog could not be hosted; commit from the main GitExtensions window. |
| Command shows `✗ Failed (exit code N)` | Read the raw output above the failure line — it is the verbatim `git`/`git lfs` error. |

## License

Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (see `LICENSE.txt`).
