---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [fluxo, clone, pull, fetch, checkout, etapa3]
---

# ⬇️ Flujo: Etapa 3 — Clone & Pull

Tercera pestaña (`tabClone`). Descargar y restaurar el contenido LFS después de clonar o cambiar de branch.

![[ScreenshotClone.png]]

## 🧭 Contexto

Cuando colaboradores o herramientas de deploy **clonan** el repositorio, Git LFS descarga los archivos pesados automáticamente al hacer checkout de la branch. Esta pestaña sirve para **buscar o restaurar** el contenido LFS manualmente cuando sea necesario.

## 🔘 Botones

```
[git lfs pull]        → RunAsync → _svc.LfsPull()       (descarga el contenido LFS del checkout actual)
[git lfs fetch --all] → RunAsync → _svc.LfsFetchAll()   (pre-busca objetos LFS de TODAS las refs)
[git lfs checkout]    → RunAsync → _svc.LfsCheckout()   (puebla el árbol de trabajo a partir de los objetos descargados)
[git lfs status]      → RunAsync → _svc.LfsStatus()     (muestra el estado de los objetos LFS; refreshAfter:false)
```

| Comando | Cuándo usar |
|---|---|
| `git lfs pull` | traer el contenido LFS que falta para el checkout actual |
| `git lfs fetch --all` | pre-descargar objetos LFS de todas las refs (ej.: antes de quedarse sin conexión) |
| `git lfs checkout` | materializar los archivos de trabajo a partir de objetos ya descargados |
| `git lfs status` | inspeccionar el estado de los objetos LFS (solo consulta — no actualiza la UI después) |

Cada botón muestra en la **consola de salida** el comando exacto y el stdout/stderr. Los botones quedan habilitados cuando hay un repositorio válido **y** LFS disponible.

## 🔗 Relacionado

- [[📤 Track Commit Push (ES)|📤 Track Commit Push]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
