---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, ui, abas, fluxo, git-lfs]
---

# 🗂️ ADR — Flujo en 3 etapas (pestañas)

## 🧭 Contexto
Git LFS tiene una curva de aprendizaje: instalar/inicializar, definir patrones de tracking (y recordar hacer stage del `.gitattributes`), hacer commit/push, y luego descargar/restaurar contenido en clones. Presentar todos los botones juntos confundiría; ocultar pasos en menús dispersaría el flujo.

## ✅ Decisión
Organizar la ventana en un **`TabControl` de tres pestañas** que reflejan el flujo estándar de Git LFS, en el orden en que el usuario lo recorre:
1. **Instalación** — `git lfs version` + `git lfs install`.
2. **Flujo básico** — track (glob) → stage de `.gitattributes` → `ls-files` → Commit + Push.
3. **Clone & Pull** — `git lfs pull` / `fetch --all` / `checkout` / `status`.

## 🔀 Alternativas consideradas
- **Una única pantalla con todos los botones** — sobrecarga cognitiva; no comunica el orden.
- **Asistente (wizard) lineal** — demasiado rígido para una herramienta que también se usa de forma puntual.
- **Tres pestañas (elegida)** — agrupa por etapa, comunica la progresión y permite saltar directo al paso deseado.

## ⚖️ Consecuencias
**Positivas:**
- Enseña el flujo de LFS mediante la propia estructura de la UI.
- Cada pestaña tiene un texto de ayuda contextual (localizado).
- La consola de salida compartida muestra el comando exacto de cualquier botón.

**Negativas / trade-offs:**
- Algunos comandos podrían pertenecer a más de una etapa (por ejemplo, `status`); la división es una simplificación didáctica.

## 🔗 Relacionado
- [[🪟 Janela dedicada não-modal (ES)|🪟 Ventana dedicada no modal]]
- [[⚙️ Instalação (ES)|⚙️ Instalación]] · [[📤 Track Commit Push (ES)|📤 Track Commit Push]] · [[⬇️ Clone e Pull (ES)|⬇️ Clone y Pull]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
