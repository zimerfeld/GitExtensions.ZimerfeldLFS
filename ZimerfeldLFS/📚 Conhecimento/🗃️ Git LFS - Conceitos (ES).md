---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, git, git-lfs, ponteiros, gitattributes]
---

# 🗃️ Git LFS — Conceptos

## Resumen
**Git Large File Storage (LFS)** es una extensión open-source de Git que sustituye **archivos grandes** (audio, video, datasets, PSDs, binarios) por **punteros de texto ligeros** dentro del repositorio. El contenido real de los archivos se aloja en un **servidor remoto separado**, lo que **acelera el clonado** y **evita el hinchamiento** del historial del repositorio.

## Puntero vs. contenido real
En el commit, Git almacena solo un pequeño archivo puntero (algunas líneas: versión, oid SHA-256, tamaño) en lugar del archivo pesado. El blob binario va al **LFS store** del remoto. Al hacer checkout, Git LFS cambia el puntero por el contenido real (smudge); al commitear, cambia el contenido por el puntero (clean) — los **filtros `filter.lfs.clean`/`smudge`** son instalados por `git lfs install`.

## `.gitattributes` y tracking
Los patrones rastreados quedan en el archivo **`.gitattributes`** (versionado). Rastrear un tipo:
```bash
git lfs track "*.psd"     # graba "*.psd filter=lfs diff=lfs merge=lfs -text" en el .gitattributes
git add .gitattributes    # el .gitattributes DEBE commitearse
```
> [!important] Recuerde commitear el `.gitattributes`
> Sin el `.gitattributes` versionado, los colaboradores no saben que esos archivos son LFS. Por eso la ventana **stagea el `.gitattributes` automáticamente** tras track/untrack — ver [[📤 Track Commit Push (ES)|📤 Track Commit Push]].

## Comandos por etapa

| Etapa | Comando | Efecto |
|---|---|---|
| Instalación | `git lfs install` | graba los filtros LFS en la config global (1× por máquina) |
| | `git lfs version` | verifica si `git-lfs` está disponible |
| Tracking | `git lfs track "<glob>"` | añade el patrón al `.gitattributes` |
| | `git lfs untrack "<glob>"` | elimina el patrón |
| | `git lfs track` | lista los patrones rastreados |
| | `git lfs ls-files` | lista los archivos gestionados por LFS |
| Clone/Pull | `git lfs pull` | descarga el contenido LFS del checkout actual |
| | `git lfs fetch --all` | pre-descarga los objetos LFS de todas las refs |
| | `git lfs checkout` | puebla el árbol de trabajo a partir de los objetos descargados |
| | `git lfs status` | muestra el estado de los objetos LFS |

## Storage y clone
El `git clone` de un repo con LFS descarga los archivos pesados **bajo demanda** al hacer checkout de la rama (vía smudge). `fetch --all` predescarga todo (útil antes de quedarse sin conexión). Como el historial guarda solo punteros, clonar no arrastra todas las versiones antiguas de los binarios — de ahí la ganancia en velocidad y en tamaño.

## Dónde aparece esto en el plugin
Cada método de [[⚡ LfsService (ES)|⚡ LfsService]] se mapea directamente a uno de estos comandos; las tres pestañas de [[🪟 LfsForm (ES)|🪟 LfsForm]] los agrupan por etapa. Ver [[🗂️ Fluxo em 3 etapas (abas) (ES)|🗂️ Flujo en 3 etapas (pestañas)]].

## 🔗 Relacionado
- [[📦 GitExtensions.ZimerfeldLFS (ES)|📦 GitExtensions.ZimerfeldLFS]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
- [[📤 Track Commit Push (ES)|📤 Track Commit Push]]
