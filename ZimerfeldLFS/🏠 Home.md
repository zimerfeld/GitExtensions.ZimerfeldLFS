---
tipo: moc
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-05
tags: [home, moc, zimerfeld, lfs]
---

# 🏠 GitExtensions.ZimerfeldLFS — Cofre de Neurônios

> 🇺🇸 Read this page in English → [[🏠 Home (EN)]] · 🇪🇸 Lea en español → [[🏠 Home (ES)]]

![[icon-128.png]]

> [!abstract] 🧠 O que é este cofre
> Memória persistente do Claude para o projeto **GitExtensions.ZimerfeldLFS** — um plugin para o **GitExtensions** que gerencia o **Git LFS** numa janela dedicada e não-modal. Cada sessão lê este cofre primeiro e o mantém atualizado.

## ⚡ Resumo executivo
- **O que é:** extensão (plugin MEF) para o **GitExtensions** que expõe o **Git LFS** numa **janela dedicada e não-modal**, guiando o usuário por um fluxo de **3 etapas** — *Instalação* → *Track/Commit/Push* → *Clone/Pull*.
- **Problema que resolve:** o Git LFS é poderoso mas dependente de linha de comando e propenso a erros (`git lfs install`, `track`, `.gitattributes`). O plugin transforma esse fluxo em cliques, com **log visível** de cada comando executado.
- **Diferenciais:** janela **persistente** (não interrompe o host); **diretório de trabalho independente** do repositório ativo; **i18n** (Automático / EN-US / PT-BR / ES-ES); ícone próprio (4 quadrantes + bomba); banner de patrocínio (GitHub Sponsors + Ko-fi).
- **Stack:** C# / WinForms `Library`, alvo **net9.0-windows**, empacotado como **nupkg**; build e versionamento via `build.ps1`.
- **Estado atual:** versão **`1.0.4`** — funcional, com **36 testes unitários (xUnit)** cobrindo o `LfsService`.
- **Público-alvo:** desenvolvedores e times que versionam ativos grandes (jogos, mídia, datasets de ML) e usam GitExtensions no Windows.
- **Ângulo de negócio:** produto **open source** sob o owner `zimerfeld`, reforçando autoridade técnica e servindo de vitrine para adoção e patrocínio.

## 🧭 Navegação por prioridade

### 1️⃣ 🔑 Impacto — Arquivos-Chave
> Arquivos que, se manipulados, têm grande impacto no sistema.
- [[🔌 ZimerfeldLfsPlugin]] — ponto de entrada MEF do plugin
- [[🪟 LfsForm]] — a janela (abas, dropdown de repo, log, i18n)
- [[⚡ LfsService]] — executor de `git`/`git lfs`
- [[🛠️ build.ps1]] — script de build e versionamento
- [[🎨 Generate-LfsIcon]] — gerador GDI+ do ícone (4 quadrantes + bomba)

### 2️⃣ 🧩 Reutilização — Sistemas
> Subsistemas reutilizados por várias partes do projeto.
- [[🔭 Visão Geral]] — o que o plugin faz, stack, versão atual
- [[🏛️ Arquitetura]] — três classes (Plugin → Form → Service) e como se relacionam
- [[🔢 Versionamento]] — ciclo build.ps1 / nuspec / csproj
- [[🧱 Dependências]] — assemblies do GitExtensions + `git`/`git-lfs`

### 3️⃣ 🔀 Uso — Fluxos
> Fluxos de uso passo a passo.
- [[⚙️ Instalação]] — etapa 1: detectar e inicializar o git-lfs
- [[📤 Track Commit Push]] — etapa 2: track → stage `.gitattributes` → ls-files → commit → push
- [[⬇️ Clone e Pull]] — etapa 3: pull / fetch --all / checkout / status
- [[📂 Diretório de Trabalho Independente]] — como o `cboRepo` fica independente do host

## 🚀 Operação
- [[💻 Ambiente Local (Dev)]] — `.\build.ps1` (compila + instala no GitExtensions local)
- [[🚀 Deploy em Produção (Prod)]] — `.\build.ps1 -Force` → `.nupkg` de release + push NuGet

## ⚖️ Decisões
- [[🪟 Janela dedicada não-modal]] — janela persistente em vez de diálogo modal
- [[📁 Diretório de trabalho independente]] — desacoplamento do repositório ativo do host
- [[🗂️ Fluxo em 3 etapas (abas)]] — Instalação / Fluxo básico / Clone & Pull
- [[💣 Ícone 4 quadrantes + bomba]] — racional do design do ícone

## 📚 Conhecimento
- [[🗃️ Git LFS - Conceitos]] — ponteiros, `.gitattributes`, tracking, storage
- [[🧩 Plugin MEF para GitExtensions]] — modelo MEF de plugin
- [[📖 README — Instalação, Uso e Build]] — espelho do README

## 💼 Negócio
- [[📦 GitExtensions.ZimerfeldLFS]] — nota-mãe (objetivo, stack, funcionalidades, ângulo de investimento)

## 🧭 Meta
- [[🔑 Fatos-Chave]] — verdades sempre úteis (paths, nomes, convenções)
- [[🧭 Como usar este cofre]] — protocolo de leitura/escrita do Claude
- [[👤 Renato]] — contexto e preferências · [[🦀 RTK]] — ferramenta · [[📥 Inbox]] — captura rápida

## 📌 Retomada
- [[📌 Backlog]] — **comece por aqui** ao retomar o projeto em outra sessão
