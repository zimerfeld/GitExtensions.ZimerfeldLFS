---
tipo: backlog
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-07
tags: [backlog, retomada, roadmap]
---

# 📌 Backlog

> 🇺🇸 Read this page in English → [[📌 Backlog (EN)]] · 🇪🇸 Lea en español → [[📌 Backlog (ES)]]

> [!tip] Comece por aqui ao retomar
> Ponto de retomada do projeto **ZimerfeldLFS** em outra sessão. Leia também [[🏠 Home]] e a nota-mãe [[📦 GitExtensions.ZimerfeldLFS]].

## ✅ Estado atual
- **Versão:** `1.0.2` (`major.minor.BUILD`, fonte da verdade: `.nuspec` / `.csproj`).
- **Testes:** **36 testes unitários (xUnit)** cobrindo o `LfsService`.
- **Build:** `net9.0-windows`, WinForms `Library`, empacotado como `.nupkg` via [[🛠️ build.ps1]].
- **Funcional:** fluxo guiado em 3 etapas (Instalação / Fluxo básico / Clone & Pull), diretório de trabalho independente, i18n (Automático / EN-US / PT-BR / ES-ES), banner de patrocínio.
- **Cofre:** reestruturado no padrão "Cofre de Neurônios v2" em 2026-07-04 (pares bilíngues, sortspec, custom-sort).

## 🔜 Próximos passos (derivados das notas)
- [ ] **Cobrir mais casos de teste** além do `LfsService` (ex.: parsing de `GetRepositoriesFromSettings`, i18n). Ver [[⚡ LfsService]].
- [ ] **Publicar/atualizar no NuGet** a versão atual seguindo o runbook [[🚀 Deploy em Produção (Prod)]] (GitFlow: release → tag → push).
- [ ] **Acompanhar adoção** (clones + downloads NuGet) do repositório `zimerfeld/GitExtensions.ZimerfeldLFS`, conforme a rotina de portfólio.
- [ ] **Robustez do parsing de settings** — o `GetRepositoriesFromSettings` depende do formato XML do GitExtensions (best-effort). Monitorar quebras entre versões do host. Ver [[📁 Diretório de trabalho independente]].
- [ ] **Ícone em 16×16** — muitos detalhes competem no tamanho de menu; avaliar simplificação. Ver [[💣 Ícone 4 quadrantes + bomba]].

## 💡 Ideias / a avaliar
- [ ] Documentar/expandir o manual de operação da janela (`README.en-US.md` / `README.pt-BR.md`) linkado da seção "As três etapas".
- [ ] Reforçar o portfólio coeso com os irmãos [[GitExtensions.ZimerfeldTree]] e [[GitExtensions.ZimerfeldCommitMsg]].

## ✅ Feito recente
- [x] **Fix na landing page — quebra de linha de títulos/subtítulos em PT** — 2026-07-07: a landing page (`index.html`, publicada em **lfs.zimerfeld.com** via GitHub Pages) compartilha um template i18n com a regra `html[data-lang="pt"] .lang-pt{display:inline}`, que tornava **todo** elemento em português `inline` — incluindo `h2`/`h3` — fazendo título/subtítulo colarem no texto seguinte quando o site abre em PT (em EN funcionava, pois `h2`/`h3` já são `block` por padrão). **Correção de 1 linha de CSS:** `html[data-lang="pt"] h2.lang-pt,html[data-lang="pt"] h3.lang-pt{display:block}` — restaura a quebra apenas nos títulos/subtítulos em PT, sem afetar o EN. Publicado via GitFlow (`feature/pt-heading-break` → `develop` → release → `main`) + tag **`202607071915pt-heading-break`**; `CNAME` preservado; deploy confirmado ao vivo.

## 🔗 Ligações
- [[🏠 Home]] · [[📦 GitExtensions.ZimerfeldLFS]] · [[🔢 Versionamento]] · [[🚀 Deploy em Produção (Prod)]]
