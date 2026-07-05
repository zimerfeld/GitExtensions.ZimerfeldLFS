---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, working-dir, desacoplamento, repositorio]
---

# 📁 ADR — Independent working directory from the host

## 🧭 Context
Each GitExtensions window is bound to **one** active repository. Tying the LFS window to that repository would mean that switching repos in the host would change (or break) the window's target — exactly the kind of "stopped working after switching repository" bug that the sibling CommitMsg had to work around.

## ✅ Decision
Give the window its **own repository dropdown** (`cboRepo`), populated from the **GitExtensions repository history** (read from the `GitExtensions.settings` file), and use the host working dir **only once** as a pre-selected value. The plugin **subscribes to no host events**; the target repository is chosen exclusively via the dropdown.

## 🔀 Alternatives considered
- **Follow the host's active repository** (listen to `PostRepositoryChanged`) — coupling and fragility; the target would change under the user's feet.
- **Ask for the path manually** — no discovery; worse UX.
- **History dropdown (chosen)** — discovers repositories automatically, keeps the window stable and reusable.

## 🛠️ Implementation
- `LfsService.GetRepositoriesFromSettings()` reads `%APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings`, extracts the `key="history"` item (whose value is a nested XML string) and collects each `<Path>`.
- `LfsForm.LoadRepositories()` populates the combo, inserts the initial working dir at the top if missing and pre-selects it.
- `CboRepo_SelectedIndexChanged` swaps `_svc.WorkingDir` and re-probes the state. See [[📂 Diretório de Trabalho Independente (EN)|📂 Independent working directory]].

## ⚖️ Consequences
**Positive:**
- Persistent window that manages LFS for **any** repo in the history.
- Total decoupling → robustness across versions and no "wrong repo" bugs.

**Negative / trade-offs:**
- Depends on the GitExtensions settings XML format (best-effort, fault-tolerant parsing → empty list).
- Repositories outside the history do not appear (only the initial one is inserted manually).

## 🔗 Related
- [[🪟 Janela dedicada não-modal (EN)|🪟 Dedicated non-modal window]]
- [[📂 Diretório de Trabalho Independente (EN)|📂 Independent working directory]]
- [[⚡ LfsService (EN)|⚡ LfsService]]
