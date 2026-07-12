---
tipo: backlog
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
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

## 🔗 Ligações
- [[🏠 Home]] · [[📦 GitExtensions.ZimerfeldLFS]] · [[🔢 Versionamento]] · [[🚀 Deploy em Produção (Prod)]]
