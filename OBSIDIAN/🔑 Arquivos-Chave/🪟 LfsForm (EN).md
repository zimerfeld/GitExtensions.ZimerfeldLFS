---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [arquivo, winforms, ui, janela, abas, i18n, threading]
arquivo: src/GitExtensions.ZimerfeldLFS/LfsForm.cs
---

# 🪟 LfsForm.cs

The main window — non-modal — that guides the LFS workflow in three steps.

**Path:** `src/GitExtensions.ZimerfeldLFS/LfsForm.cs`

---

## 📜 Declaration and window properties

```csharp
public sealed class LfsForm : Form
```

| Property | Value |
|---|---|
| `FormBorderStyle` | `FixedSingle` (not resizable) |
| `MaximizeBox` | `false` (with `MinimizeBox = true`) |
| `StartPosition` | `CenterScreen` |
| `Font` | Segoe UI 9 |
| `Icon` | `PluginIcon.ForForm()` |
| `Size` | `720 × (720 + SponsorBanner.PanelHeight)` |

**Constructor** `LfsForm(workingDir, notifyRepoChanged?, openCommitDialog?, openPushDialog?)`: creates the `LfsService`, stores the delegates, builds the UI (`InitializeComponent`) and populates `cboRepo` (`LoadRepositories`, XML reading only). **No git work in the constructor** → instant window; the first probe runs on `Shown` via `RefreshStateAsync` in a background thread.

---

## 🖼️ Layout (top → bottom)

1. **SponsorBanner** (`DockStyle.Top`, topmost) — clickable GitHub Sponsors + Ko-fi badges + "About" link on the right (`SponsorBanner.cs`).
2. **Top panel** — "Working Directory" label, **`cboRepo`** (independent repository dropdown) and branch label.
3. **TabControl** (`DockStyle.Fill`) — 3 tabs: `tabInstall`, `tabWorkflow`, `tabClone`.
4. **Log panel** (`DockStyle.Bottom`, height 150) — header ("Output:" + Clear button) and `txtLog` (multiline, read-only, `#1E1E1E` background, Consolas font).
5. **Bottom panel** — **Show Debug** checkbox, label + **Language** dropdown, **Close** button (also `CancelButton`).
6. **StatusStrip** — status label (`NoGripRenderer` removes the resize grip).

---

## 🗂️ The 3 tabs

### `tabInstall` (Step 1 · Installation)
- `lblInstallStatus` (colored status: red = missing, gold = available, green = ready).
- Buttons: **Check installation** → `RunAsync(..., () => _svc.GetLfsVersion())`; **`git lfs install`** → `RunAsync("git lfs install", () => _svc.LfsInstall())`.
- `lblInstallHelp` with the help text (Homebrew/Chocolatey/binaries). See [[⚙️ Instalação (EN)|⚙️ Installation]].

### `tabWorkflow` (Step 2 · Basic workflow)
- `txtPattern` (with placeholder) + **Track** button (`DoTrack`; Enter also triggers).
- `lstPatterns` (tracked patterns) + **Untrack** button (`DoUntrack`).
- `lstLfsFiles` (LFS files, `ls-files`) with horizontal scroll.
- `commitRow` (bottom): **Commit…** (`DoCommit`) and **Push…** (`DoPush`). See [[📤 Track Commit Push (EN)|📤 Track Commit Push]].

### `tabClone` (Step 3 · Clone & Pull)
- `lblCloneHelp` + 4 buttons: **`git lfs pull`**, **`git lfs fetch --all`**, **`git lfs checkout`**, **`git lfs status`** (`status` runs with `refreshAfter:false`). See [[⬇️ Clone e Pull (EN)|⬇️ Clone and Pull]].

---

## 📂 Independent working directory

`LoadRepositories()` calls `LfsService.GetRepositoriesFromSettings()` (reads the GitExtensions history from the settings XML), inserts the initial working dir at the top if not in the list, and pre-selects it. `CboRepo_SelectedIndexChanged` swaps `_svc.WorkingDir` and triggers `RefreshStateAsync`. `UpdateWorkingDir(newDir)` (called by the plugin when the window already exists) adds/selects the dir. See [[📂 Diretório de Trabalho Independente (EN)|📂 Independent Working Directory]].

---

## 🪟 Actions that open native dialogs

- **`DoCommit`** — calls `_openCommitDialog?.Invoke(this, dir)`; logs "completed / closed / unavailable" according to the `bool?`; calls `notifyRepoChanged` and `RefreshStateAsync`.
- **`DoPush`** — calls `_openPushDialog`; if `true`, logs and refreshes state; otherwise falls back to plain `git push` via `RunAsync`.

`DoTrack`/`DoUntrack` run `git lfs track/untrack "<glob>"` and, on success, **stage `.gitattributes`** (`_svc.Add(".gitattributes")`).

---

## 🧵 Runner and refresh (threading)

### `RunAsync(title, op, refreshAfter = true)`
Ignores if `_busy` or if `!EnsureRepoSelected()`. Marks busy, logs `$ <title>`, runs `await Task.Run(op)`, logs `Combined` + ✓/✗ (with `ExitCode`), unmarks busy and (optionally) `RefreshStateAsync`.

### `RefreshStateAsync()`
Probes in a `Task.Run`: `IsGitRepo`, branch, lfs version, available, initialized, patterns, files → `StateSnapshot` → `ApplyState` (updates labels, lists and enables/disables buttons). The status becomes "Refreshing…" during the probe.

### `Log(message)`
Appends to `txtLog` with auto-scroll; uses `BeginInvoke` if `InvokeRequired`.

---

## 🌐 Localization and Debug

- **Language:** Automatic/English/Portuguese dropdown → `I18n.SetLanguage` → `ApplyLanguage()` reloads the `Translator` and rewrites all texts. `_suppressLangEvent` avoids re-entrancy when repopulating the combo.
- **Show Debug:** `ApplyControlTooltips(show)` walks the tree and sets the tooltip = each control's `Name`. Persisted at `%APPDATA%\GitExtensions\ZimerfeldLFS.uisettings.json` (`{"showControlIds":…}`).
- **About:** `LinkLabel` in the banner → `MessageBox` with the localized text (`aboutText`).

### 💾 Persisted settings files
| File (`%APPDATA%\GitExtensions\`) | Content |
|---|---|
| `ZimerfeldLFS.language.json` | chosen language (`I18n`) |
| `ZimerfeldLFS.uisettings.json` | `showControlIds` (Debug checkbox) |
| `ZimerfeldLFS.debug.log` | plugin diagnostic log |

---

## 🔗 Related

- [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
- [[📂 Diretório de Trabalho Independente (EN)|📂 Independent Working Directory]]
- [[🗂️ Fluxo em 3 etapas (abas) (EN)|🗂️ 3-step flow (tabs)]]
- [[🏛️ Arquitetura (EN)|🏛️ Architecture]]
