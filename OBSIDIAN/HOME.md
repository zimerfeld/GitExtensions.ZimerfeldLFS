---
tipo: moc
tags: [home, moc, zimerfeld, lfs]
atualizado: 2026-07-01
---

# ZimerfeldLFS — Mapa do Cofre

![[ScreenShots/icon-128.png]]

Plugin para **GitExtensions** que gerencia o **Git Large File Storage (LFS)** em uma janela dedicada e não-modal. O Git LFS troca arquivos grandes (áudio, vídeo, datasets) por ponteiros de texto leves; o conteúdo real fica num servidor remoto separado, **acelerando o clone e evitando o inchaço do repositório**. A janela conduz o fluxo de LFS em **três etapas** e usa um **diretório de trabalho escolhido de forma independente do host**.

---

## ⚡ Resumo executivo

- **O que é:** extensão (plugin MEF) para o **GitExtensions** que expõe o **Git LFS** numa **janela dedicada e não-modal**, guiando o usuário por um fluxo de **3 etapas** — *Instalação* → *Track/Commit/Push* → *Clone/Pull*.
- **Problema que resolve:** o Git LFS é poderoso mas dependente de linha de comando e propenso a erros de configuração (`git lfs install`, `track`, `.gitattributes`). O plugin transforma esse fluxo em cliques, com **log visível** de cada comando `git`/`git lfs` executado.
- **Diferenciais:** janela **persistente** (não interrompe o trabalho no host); **diretório de trabalho independente** do repositório ativo do GitExtensions; **i18n** (Automático / EN-US / PT-BR); ícone próprio (4 quadrantes + bomba); banner de patrocínio (GitHub Sponsors + Ko-fi).
- **Stack:** C# / WinForms `Library`, alvo **net9.0-windows**, empacotado como **nupkg**; build e versionamento automatizados via `build.ps1`.
- **Estado atual:** versão **`1.0.2`** — funcional, com **36 testes unitários (xUnit)** cobrindo o `LfsService`.
- **Público-alvo:** desenvolvedores e times que versionam ativos grandes (jogos, mídia, datasets de ML) e já usam GitExtensions no Windows.
- **Ângulo de negócio/portfólio:** produto **open source** sob o owner `zimerfeld`, reforçando autoridade técnica e servindo de vitrine para adoção (clones/downloads) e captação de patrocínio.

---

## Navegação

### Sistema
- [[Sistema/Visão Geral]] — o que o plugin faz, stack, versão atual
- [[Sistema/Arquitetura]] — três classes (Plugin → Form → Service), como se relacionam
- [[Sistema/Versionamento]] — ciclo build.ps1 / nuspec / csproj
- [[Sistema/Dependências]] — assemblies do GitExtensions + `git`/`git-lfs`

### Fluxos
- [[Fluxos/1 - Instalação]] — etapa 1: detectar e inicializar o git-lfs
- [[Fluxos/2 - Track Commit Push]] — etapa 2: track → stage `.gitattributes` → ls-files → commit → push
- [[Fluxos/3 - Clone e Pull]] — etapa 3: pull / fetch --all / checkout / status
- [[Fluxos/Diretório de Trabalho Independente]] — como o `cboRepo` é populado e fica independente do host

### Arquivos-Chave
- [[Arquivos-Chave/ZimerfeldLfsPlugin]] — ponto de entrada MEF do plugin
- [[Arquivos-Chave/LfsForm]] — a janela (abas, dropdown de repo, log, i18n)
- [[Arquivos-Chave/LfsService]] — executor de `git`/`git lfs`
- [[Arquivos-Chave/build.ps1]] — script de build e versionamento
- [[Arquivos-Chave/Generate-LfsIcon]] — gerador GDI+ do ícone (4 quadrantes + bomba)

### Decisões Arquiteturais
- [[Decisoes/Janela dedicada não-modal]] — janela persistente em vez de diálogo modal
- [[Decisoes/Diretório de trabalho independente]] — desacoplamento do repositório ativo do host
- [[Decisoes/Fluxo em 3 etapas (abas)]] — Instalação / Fluxo básico / Clone & Pull
- [[Decisoes/Ícone 4 quadrantes + bomba]] — racional do design do ícone

### Conhecimento
- [[02 - Conhecimento/Git LFS - Conceitos]] — ponteiros, `.gitattributes`, tracking, storage
- [[02 - Conhecimento/Plugin MEF para GitExtensions]] — modelo MEF de plugin
- [[02 - Conhecimento/README — Instalação, Uso e Build]] — espelho do README

---

## Estrutura de Pastas do Repo

```
GitExtensions.ZimerfeldLFS/
├── src/GitExtensions.ZimerfeldLFS/
│   ├── ZimerfeldLfsPlugin.cs   ← entry point MEF
│   ├── LfsForm.cs              ← a janela (3 abas + log + i18n)
│   ├── LfsService.cs           ← runner git / git lfs
│   ├── Localization.cs         ← I18n + Translator (JSON embutido)
│   ├── SponsorBanner.cs        ← banner GitHub Sponsors + Ko-fi
│   ├── PluginIcon.cs           ← carregador do ícone embutido
│   ├── Resources/              ← ico.png, icon-128.png, badges, *.json (i18n)
│   ├── *.csproj / *.nuspec
├── refs/                       ← DLLs do host versionadas (Private=false)
├── tools/
│   ├── install.ps1 / uninstall.ps1
│   ├── net9.0-windows/         ← DLL para o nupkg
│   └── icon-generator/Generate-LfsIcon.ps1
├── OBSIDIAN/CLAUDE/            ← 🧠 este cofre de memória
├── build.ps1                   ← incrementa versão + build + deploy + nupkg
└── README.md / README.pt-BR.md / README.en-US.md
```

---

## Versão Atual

`1.0.2` — compilada em `net9.0-windows`, WinForms `Library`
