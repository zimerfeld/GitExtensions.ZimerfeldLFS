---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [sistema, overview, plugin, gitextensions, git-lfs, i18n]
---

# 🔭 Visión general

## 🎯 Qué es

Plugin para **GitExtensions** (Windows) que gestiona **Git Large File Storage (LFS)** en una **ventana dedicada y no modal**. Guía al usuario por el flujo estándar de LFS en **tres etapas** (pestañas) y opera sobre un **repositorio elegido en el propio dropdown de la ventana**, independiente del host. Ver [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]] y [[🪟 Janela dedicada não-modal (ES)|🪟 Ventana dedicada no modal]].

## ⚙️ Stack

| Item | Valor |
|---|---|
| Lenguaje | C# (.NET 9) |
| Target | `net9.0-windows` |
| Framework de UI | Windows Forms (`UseWindowsForms`) |
| Tipo de salida | `Library` (DLL cargada por GitExtensions) |
| Assembly de salida | `GitExtensions.Plugins.ZimerfeldLFS.dll` |
| Namespace | `GitExtensions.ZimerfeldLFS` |
| Versión actual | `1.0.4` |
| Idiomas | Portugués-BR / Inglés (automático según el SO + override) |
| Licencia | CC BY-NC-ND 4.0 © 2026 Zimerfeld |
| Autor | Zimerfeld |

> El plugin muestra un **ícono** (PNG embebido en `Resources/ico.png`, 16×16) en el menú Plugins y en la barra de título de la ventana. Cargado por `PluginIcon` (cache lazy). Ver [[🎨 Generate-LfsIcon (ES)|🎨 Generate-LfsIcon]].

## 🗂️ Las tres etapas (pestañas)

### 1 · Instalación
Detecta `git lfs` (`git lfs version`) y lo inicializa para la cuenta del usuario (`git lfs install`, se ejecuta una vez por máquina). La ayuda menciona Homebrew, Chocolatey y los binarios oficiales de [git-lfs.com](https://git-lfs.com). Ver [[⚙️ Instalação (ES)|⚙️ Instalación]].

### 2 · Flujo básico (track / commit / push)
Rastrea tipos de archivo mediante **patrones glob** (`git lfs track "*.psd"`) y **stagea el `.gitattributes`**; lista los patrones rastreados (con *Untrack*) y los archivos LFS (`git lfs ls-files`); **Commit** y **Push** abren los diálogos **nativos** de GitExtensions para el repositorio seleccionado. Ver [[📤 Track Commit Push (ES)|📤 Track Commit Push]].

### 3 · Clone & Pull
Descarga/restaura contenido LFS: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`. Ver [[⬇️ Clone e Pull (ES)|⬇️ Clone y Pull]].

## 🔗 Funcionalidades transversales

| Funcionalidad | Detalle |
|---|---|
| Directorio independiente | `cboRepo` poblado con el historial de GitExtensions — ver [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de trabajo independiente]] |
| Consola de salida | muestra el comando exacto + stdout/stderr (fondo oscuro, fuente Consolas) |
| Localización | JSON embebido por idioma; elección persistida en `%APPDATA%\GitExtensions\ZimerfeldLFS.language.json` |
| Mostrar Debug | tooltip con el `Name` de cada control; persistido en `ZimerfeldLFS.uisettings.json` |
| Threading | la ventana se abre instantáneamente; primer sondeo en `Shown` en un hilo de fondo |

## ✅ Requisitos de runtime

- **GitExtensions 6.x** (.NET 9)
- **`git`** y **`git-lfs`** en el `PATH`

## 🔗 Relacionado

- [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]]
- [[🔢 Versionamento (ES)|🔢 Versionado]]
- [[🧱 Dependências (ES)|🧱 Dependencias]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
