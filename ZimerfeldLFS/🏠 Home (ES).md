---
tipo: moc
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-05
tags: [home, moc, zimerfeld, lfs]
---

# 🏠 GitExtensions.ZimerfeldLFS — Cofre de Neuronas

> 🇧🇷 Leia em português → [[🏠 Home]] · 🇺🇸 Read in English → [[🏠 Home (EN)]]

![[icon-128.png]]

> [!abstract] 🧠 Qué es este cofre
> Memoria persistente de Claude para el proyecto **GitExtensions.ZimerfeldLFS** — un plugin para **GitExtensions** que gestiona **Git LFS** en una ventana dedicada y no modal. Cada sesión lee este cofre primero y lo mantiene actualizado.

## ⚡ Resumen ejecutivo
- **Qué es:** una extensión (plugin MEF) para **GitExtensions** que expone **Git LFS** en una **ventana dedicada y no modal**, guiando al usuario por un flujo de **3 etapas** — *Instalación* → *Track/Commit/Push* → *Clone/Pull*.
- **Problema que resuelve:** Git LFS es potente pero depende de la línea de comandos y es propenso a errores (`git lfs install`, `track`, `.gitattributes`). El plugin convierte ese flujo en clics, con un **registro visible** de cada comando ejecutado.
- **Diferenciales:** ventana **persistente** (no interrumpe al host); **directorio de trabajo independiente** del repositorio activo; **i18n** (Automático / EN-US / PT-BR / ES-ES); icono propio (4 cuadrantes + bomba); banner de patrocinio (GitHub Sponsors + Ko-fi).
- **Stack:** C# / WinForms `Library`, dirigido a **net9.0-windows**, empaquetado como **nupkg**; build y versionado vía `build.ps1`.
- **Estado actual:** versión **`1.0.4`** — funcional, con **36 pruebas unitarias (xUnit)** cubriendo el `LfsService`.
- **Público objetivo:** desarrolladores y equipos que versionan activos grandes (juegos, medios, datasets de ML) y usan GitExtensions en Windows.
- **Ángulo de negocio:** producto **open source** bajo el owner `zimerfeld`, reforzando autoridad técnica y sirviendo de vitrina para adopción y patrocinio.

## 🧭 Navegación por prioridad

### 1️⃣ 🔑 Impacto — Archivos Clave
> Archivos que, si se manipulan, tienen gran impacto en el sistema.
- [[🔌 ZimerfeldLfsPlugin]] — punto de entrada MEF del plugin
- [[🪟 LfsForm]] — la ventana (pestañas, dropdown de repo, log, i18n)
- [[⚡ LfsService]] — ejecutor de `git`/`git lfs`
- [[🛠️ build.ps1]] — script de build y versionado
- [[🎨 Generate-LfsIcon]] — generador GDI+ del icono (4 cuadrantes + bomba)

### 2️⃣ 🧩 Reutilización — Sistemas
> Subsistemas reutilizados por varias partes del proyecto.
- [[🔭 Visão Geral|🔭 Resumen General]] — qué hace el plugin, stack, versión actual
- [[🏛️ Arquitetura|🏛️ Arquitectura]] — tres clases (Plugin → Form → Service) y cómo se relacionan
- [[🔢 Versionamento|🔢 Versionado]] — ciclo build.ps1 / nuspec / csproj
- [[🧱 Dependências|🧱 Dependencias]] — assemblies de GitExtensions + `git`/`git-lfs`

### 3️⃣ 🔀 Uso — Flujos
> Flujos de uso paso a paso.
- [[⚙️ Instalação|⚙️ Instalación]] — etapa 1: detectar e inicializar el git-lfs
- [[📤 Track Commit Push]] — etapa 2: track → stage `.gitattributes` → ls-files → commit → push
- [[⬇️ Clone e Pull|⬇️ Clone y Pull]] — etapa 3: pull / fetch --all / checkout / status
- [[📂 Diretório de Trabalho Independente|📂 Directorio de Trabajo Independiente]] — cómo el `cboRepo` queda independiente del host

## 🚀 Operación
- [[💻 Ambiente Local (Dev)|💻 Entorno Local (Dev)]] — `.\build.ps1` (compila + instala en GitExtensions local)
- [[🚀 Deploy em Produção (Prod)|🚀 Despliegue en Producción (Prod)]] — `.\build.ps1 -Force` → `.nupkg` de release + push a NuGet

## ⚖️ Decisiones
- [[🪟 Janela dedicada não-modal|🪟 Ventana dedicada no modal]] — ventana persistente en lugar de diálogo modal
- [[📁 Diretório de trabalho independente|📁 Directorio de trabajo independiente]] — desacoplamiento del repositorio activo del host
- [[🗂️ Fluxo em 3 etapas (abas)|🗂️ Flujo en 3 etapas (pestañas)]] — Instalación / Flujo básico / Clone & Pull
- [[💣 Ícone 4 quadrantes + bomba|💣 Icono: 4 cuadrantes + bomba]] — razón del diseño del icono

## 📚 Conocimiento
- [[🗃️ Git LFS - Conceitos|🗃️ Git LFS — Conceptos]] — punteros, `.gitattributes`, tracking, storage
- [[🧩 Plugin MEF para GitExtensions]] — modelo de plugin MEF
- [[📖 README — Instalação, Uso e Build|📖 README — Instalación, Uso y Build]] — espejo del README

## 💼 Negocio
- [[📦 GitExtensions.ZimerfeldLFS]] — nota madre (objetivo, stack, funcionalidades, ángulo de inversión)

## 🧭 Meta
- [[🔑 Fatos-Chave (ES)|🔑 Hechos Clave]] — verdades siempre útiles (rutas, nombres, convenciones)
- [[🧭 Como usar este cofre (ES)|🧭 Cómo usar este cofre]] — protocolo de lectura/escritura de Claude
- [[👤 Renato (ES)|👤 Renato]] — contexto y preferencias · [[🦀 RTK (ES)|🦀 RTK]] — herramienta · [[📥 Inbox (ES)|📥 Inbox]] — captura rápida

## 📌 Reanudación
- [[📌 Backlog (ES)|📌 Backlog]] — **empieza por aquí** al retomar el proyecto en otra sesión
