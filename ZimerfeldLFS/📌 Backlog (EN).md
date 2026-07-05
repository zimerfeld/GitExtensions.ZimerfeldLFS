---
tipo: backlog
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [backlog, retomada, roadmap]
---

# 📌 Backlog

> 🇧🇷 Leia esta página em português → [[📌 Backlog]]

> [!tip] Start here when resuming
> Resume point for the **ZimerfeldLFS** project in another session. Also read [[🏠 Home (EN)|🏠 Home]] and the mother note [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]].

## ✅ Current state
- **Version:** `1.0.2` (`major.minor.BUILD`, source of truth: `.nuspec` / `.csproj`).
- **Tests:** **36 unit tests (xUnit)** covering the `LfsService`.
- **Build:** `net9.0-windows`, WinForms `Library`, packed as `.nupkg` via [[🛠️ build.ps1 (EN)|🛠️ build.ps1]].
- **Functional:** guided 3-step flow (Installation / Basic flow / Clone & Pull), independent working directory, i18n (Automatic / EN-US / PT-BR), sponsorship banner.
- **Vault:** restructured to the "Cofre de Neurônios v2" standard on 2026-07-04 (bilingual pairs, sortspec, custom-sort).

## 🔜 Next steps (derived from the notes)
- [ ] **Cover more test cases** beyond the `LfsService` (e.g.: `GetRepositoriesFromSettings` parsing, i18n). See [[⚡ LfsService (EN)|⚡ LfsService]].
- [ ] **Publish/update on NuGet** the current version following the runbook [[🚀 Deploy em Produção (Prod) (EN)|🚀 Production Deploy (Prod)]] (GitFlow: release → tag → push).
- [ ] **Track adoption** (clones + NuGet downloads) of the `zimerfeld/GitExtensions.ZimerfeldLFS` repo, per the portfolio routine.
- [ ] **Settings-parsing robustness** — `GetRepositoriesFromSettings` depends on the GitExtensions XML format (best-effort). Watch for breakage across host versions. See [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]].
- [ ] **16×16 icon** — many details compete at menu size; consider simplification. See [[💣 Ícone 4 quadrantes + bomba (EN)|💣 Icon: 4 quadrants + bomb]].

## 💡 Ideas / to evaluate
- [ ] Document/expand the window operation manual (`README.en-US.md` / `README.pt-BR.md`) linked from the "The three steps" section.
- [ ] Reinforce the cohesive portfolio with the siblings [[GitExtensions.ZimerfeldTree]] and [[GitExtensions.ZimerfeldCommitMsg]].

## 🔗 Links
- [[🏠 Home (EN)|🏠 Home]] · [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]] · [[🔢 Versionamento (EN)|🔢 Versioning]] · [[🚀 Deploy em Produção (Prod) (EN)|🚀 Production Deploy (Prod)]]
