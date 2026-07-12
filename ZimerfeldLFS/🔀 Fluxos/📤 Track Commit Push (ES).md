---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [fluxo, track, commit, push, gitattributes, etapa2]
---

# 📤 Flujo: Etapa 2 — Track / Commit / Push

Segunda pestaña (`tabWorkflow`). El flujo básico de LFS: elegir tipos de archivo, versionar y enviar.

![[ScreenshotWorkflow.png]]

## 👣 Pasos

```
1. Escribir un patrón glob (ej.: *.psd, *.mp4, *.zip)  →  [Track]
        │  DoTrack → RunAsync("git lfs track \"*.psd\"")
        │     _svc.TrackPattern(p)             → git lfs track "*.psd"
        │     si Ok → _svc.Add(".gitattributes") → stagea el .gitattributes
        ▼
2. RefreshStateAsync vuelve a poblar:
        ├─ lstPatterns ← GetTrackedPatterns()   (git lfs track, parseado)
        └─ lstLfsFiles ← GetLfsFiles()          (git lfs ls-files)
        │
        │  (Untrack: seleccionar un patrón → [Untrack] → git lfs untrack + stage .gitattributes)
        ▼
3. [Commit…]  → DoCommit → openCommitDialog(this, dir)
        │     abre el StartCommitDialog NATIVO del host en el repo de cboRepo
        │     registra completado / cerrado / no disponible; notifyRepoChanged()
        ▼
4. [Push…]    → DoPush → openPushDialog(this, dir)
        │     abre el StartPushDialog NATIVO; si no está disponible/se cancela →
        │     fallback RunAsync("git push", _svc.Push())
```

Equivalente en línea de comandos:
```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```

## 🔍 Detalles

- **`.gitattributes` en stage automáticamente:** rastrear/dejar de rastrear modifica el `.gitattributes`; la ventana ya lo pone en stage (`_svc.Add(".gitattributes")`) para que quede listo para el commit — esa es la razón por la que track es un paso separado del commit.
- **Diálogos nativos:** Commit y Push usan los diálogos del propio GitExtensions (mediante los delegates `openCommit`/`openPush` que resuelven `WithWorkingDirectory(dir)`), por lo que todos los plugins de commit se cargan con normalidad. Ver [[🔌 ZimerfeldLfsPlugin (ES)|🔌 ZimerfeldLfsPlugin]].
- **Enter en el campo de patrón** también dispara el Track; patrón vacío → `MessageBox` de aviso.

## 🔗 Relacionado

- [[⚙️ Instalação (ES)|⚙️ Instalación]]
- [[⬇️ Clone e Pull (ES)|⬇️ Clone y Pull]]
- [[🪟 LfsForm (ES)|🪟 LfsForm]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
