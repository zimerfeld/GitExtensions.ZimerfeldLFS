---
tipo: backlog
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-07
tags: [backlog, retomada, roadmap]
---

# 📌 Backlog

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

## ✅ Recently done
- [x] **Landing-page fix — PT title/subtitle line break** — 2026-07-07: the landing page (`index.html`, served at **lfs.zimerfeld.com** via GitHub Pages) shares an i18n template with the rule `html[data-lang="pt"] .lang-pt{display:inline}`, which forced **every** Portuguese element to `inline` — including `h2`/`h3` — making the title/subtitle collapse into the following text when the site opens in PT (EN was fine, since `h2`/`h3` are `block` by default). **1-line CSS fix:** `html[data-lang="pt"] h2.lang-pt,html[data-lang="pt"] h3.lang-pt{display:block}` — restores the break only on PT titles/subtitles, with no effect on EN. Shipped via GitFlow (`feature/pt-heading-break` → `develop` → release → `main`) + tag **`202607071915pt-heading-break`**; `CNAME` preserved; deploy verified live.

## 🔗 Links
- [[🏠 Home (EN)|🏠 Home]] · [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]] · [[🔢 Versionamento (EN)|🔢 Versioning]] · [[🚀 Deploy em Produção (Prod) (EN)|🚀 Production Deploy (Prod)]]
