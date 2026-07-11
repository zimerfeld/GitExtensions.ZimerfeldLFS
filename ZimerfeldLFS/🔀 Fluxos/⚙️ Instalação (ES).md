---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [fluxo, instalação, git-lfs, etapa1]
---

# ⚙️ Flujo: Etapa 1 — Instalación

Primera pestaña de la ventana (`tabInstall`). Detecta Git LFS y lo inicializa para la cuenta del usuario.

![[ScreenshotInstall.png]]

## 👣 Pasos

```
El usuario abre la pestaña "1 · Instalación"
        │
        ▼
[Verificar instalación]  → RunAsync → _svc.GetLfsVersion()   (git lfs version)
        │
        ▼
RefreshStateAsync sondea: IsLfsAvailable() / IsLfsInitializedForUser()
        │
        ├─ NO disponible → estado rojo "Git LFS NO está instalado o fuera del PATH"
        │                    (botón "git lfs install" deshabilitado)
        ├─ disponible, no inicializado → estado dorado "… Haga clic en git lfs install"
        └─ listo → estado verde "Git LFS listo — <versión> (inicializado para este usuario)"
        │
        ▼
[git lfs install]  → RunAsync("git lfs install") → _svc.LfsInstall()   (se ejecuta 1× por máquina)
```

## 🔍 Detalles

- **`GetLfsVersion()`** = `git lfs version`; stdout vacío ⇒ no instalado / fuera del PATH.
- **`IsLfsAvailable()`** = versión ok y stdout comienza con `git-lfs`.
- **`IsLfsInitializedForUser()`** = `git config --global --get filter.lfs.clean` no vacío (`git lfs install` escribe los filtros LFS en la configuración global).
- **Ayuda en la pestaña** (`step1Help`): en Windows y macOS, Git LFS normalmente ya viene incluido; instalación manual mediante **Homebrew** (`brew install git-lfs`), **Chocolatey** (`choco install git-lfs`) o los binarios oficiales en [git-lfs.com](https://git-lfs.com).

## ✅ Estado después de la etapa

Con LFS listo y un repositorio válido seleccionado, los botones de las etapas 2 y 3 quedan habilitados (`workflowEnabled = IsRepo && LfsAvailable`).

## 🔗 Relacionado

- [[📤 Track Commit Push (ES)|📤 Track Commit Push]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
