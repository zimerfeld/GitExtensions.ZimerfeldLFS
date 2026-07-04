---
tipo: moc
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
tags: [home, moc, zimerfeld, lfs]
---

# 🏠 GitExtensions.ZimerfeldLFS — Neuron Vault

> 🇧🇷 Leia esta página em português → [[🏠 Home]]

![[icon-128.png]]

> [!abstract] 🧠 What this vault is
> Claude's persistent memory for the **GitExtensions.ZimerfeldLFS** project — a plugin for **GitExtensions** that manages **Git LFS** in a dedicated, non-modal window. Each session reads this vault first and keeps it up to date.

## ⚡ Executive summary
- **What it is:** an extension (MEF plugin) for **GitExtensions** exposing **Git LFS** in a **dedicated, non-modal window**, guiding the user through a **3-step** flow — *Installation* → *Track/Commit/Push* → *Clone/Pull*.
- **Problem it solves:** Git LFS is powerful but command-line dependent and error-prone (`git lfs install`, `track`, `.gitattributes`). The plugin turns that flow into clicks, with a **visible log** of every command run.
- **Differentiators:** a **persistent** window (does not interrupt the host); a **working directory independent** of the active repository; **i18n** (Automatic / EN-US / PT-BR); its own icon (4 quadrants + bomb); a sponsorship banner (GitHub Sponsors + Ko-fi).
- **Stack:** C# / WinForms `Library`, targeting **net9.0-windows**, packed as a **nupkg**; build and versioning via `build.ps1`.
- **Current state:** version **`1.0.4`** — functional, with **36 unit tests (xUnit)** covering the `LfsService`.
- **Audience:** developers and teams versioning large assets (games, media, ML datasets) using GitExtensions on Windows.
- **Business angle:** an **open-source** product under the `zimerfeld` owner, reinforcing technical authority and serving as a showcase for adoption and sponsorship.

## 🧭 Navigation by priority

### 1️⃣ 🔑 Impact — Key Files
> Files that, if manipulated, have a large impact on the system.
- [[🔌 ZimerfeldLfsPlugin (EN)|🔌 ZimerfeldLfsPlugin]] — the plugin's MEF entry point
- [[🪟 LfsForm (EN)|🪟 LfsForm]] — the window (tabs, repo dropdown, log, i18n)
- [[⚡ LfsService (EN)|⚡ LfsService]] — `git`/`git lfs` runner
- [[🛠️ build.ps1 (EN)|🛠️ build.ps1]] — build and versioning script
- [[🎨 Generate-LfsIcon (EN)|🎨 Generate-LfsIcon]] — GDI+ icon generator (4 quadrants + bomb)

### 2️⃣ 🧩 Reuse — Systems
> Subsystems reused across the project.
- [[🔭 Visão Geral (EN)|🔭 Overview]] — what the plugin does, stack, current version
- [[🏛️ Arquitetura (EN)|🏛️ Architecture]] — three classes (Plugin → Form → Service) and how they relate
- [[🔢 Versionamento (EN)|🔢 Versioning]] — build.ps1 / nuspec / csproj cycle
- [[🧱 Dependências (EN)|🧱 Dependencies]] — GitExtensions assemblies + `git`/`git-lfs`

### 3️⃣ 🔀 Usage — Flows
> Step-by-step usage flows.
- [[⚙️ Instalação (EN)|⚙️ Installation]] — step 1: detect and initialize git-lfs
- [[📤 Track Commit Push (EN)|📤 Track Commit Push]] — step 2: track → stage `.gitattributes` → ls-files → commit → push
- [[⬇️ Clone e Pull (EN)|⬇️ Clone & Pull]] — step 3: pull / fetch --all / checkout / status
- [[📂 Diretório de Trabalho Independente (EN)|📂 Independent working directory]] — how `cboRepo` stays independent of the host

## 🚀 Operation
- [[💻 Ambiente Local (Dev) (EN)|💻 Local Environment (Dev)]] — `.\build.ps1` (build + install into local GitExtensions)
- [[🚀 Deploy em Produção (Prod) (EN)|🚀 Production Deploy (Prod)]] — `.\build.ps1 -Force` → release `.nupkg` + NuGet push

## ⚖️ Decisions
- [[🪟 Janela dedicada não-modal (EN)|🪟 Dedicated non-modal window]] — a persistent window instead of a modal dialog
- [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]] — decoupling from the host's active repository
- [[🗂️ Fluxo em 3 etapas (abas) (EN)|🗂️ 3-step flow (tabs)]] — Installation / Basic flow / Clone & Pull
- [[💣 Ícone 4 quadrantes + bomba (EN)|💣 Icon: 4 quadrants + bomb]] — icon design rationale

## 📚 Knowledge
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]] — pointers, `.gitattributes`, tracking, storage
- [[🧩 Plugin MEF para GitExtensions (EN)|🧩 MEF plugin for GitExtensions]] — the MEF plugin model
- [[📖 README — Instalação, Uso e Build (EN)|📖 README — Installation, Usage and Build]] — README mirror

## 💼 Business
- [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]] — mother note (goal, stack, features, investment angle)

## 🧭 Meta
- [[🔑 Fatos-Chave (EN)|🔑 Key Facts]] — always-useful truths (paths, names, conventions)
- [[🧭 Como usar este cofre (EN)|🧭 How to use this vault]] — Claude's read/write protocol
- [[👤 Renato (EN)|👤 Renato]] — context and preferences · [[🦀 RTK (EN)|🦀 RTK]] — tool · [[📥 Inbox (EN)|📥 Inbox]] — quick capture

## 📌 Resume
- [[📌 Backlog (EN)|📌 Backlog]] — **start here** when resuming the project in another session
