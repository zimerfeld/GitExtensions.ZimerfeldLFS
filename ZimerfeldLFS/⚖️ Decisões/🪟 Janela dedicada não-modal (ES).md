---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, ui, janela, nao-modal]
---

# 🪟 ADR — Ventana dedicada no modal para LFS

## 🧭 Contexto
Git LFS implica varias operaciones distintas (detectar/instalar, rastrear patrones, listar archivos, commit/push, pull/fetch/checkout) que el usuario realiza en **momentos diferentes** de su trabajo. Un diálogo modal bloquearía la ventana principal de GitExtensions y obligaría a abrir/cerrar en cada operación.

## ✅ Decisión
Exponer el plugin como una **ventana `Form` dedicada, no modal y singleton** (`LfsForm`), abierta desde el menú Plugins → ZimerfeldLFS, que permanece disponible junto a GitExtensions mientras el usuario trabaja. `Execute` devuelve `false` (el host no actualiza su propia UI); la ventana gestiona su propio estado.

## 🔀 Alternativas consideradas
- **Diálogo modal** — simple, pero bloquea el host y no se adapta a operaciones intermitentes.
- **Integrar en pantallas existentes del host** (por ejemplo, una pestaña en el commit) — acoplamiento alto con la UI de GitExtensions, frágil entre versiones.
- **Ventana no modal dedicada (elegida)** — persistente, reutilizable, desacoplada; mismo patrón que el hermano [[GitExtensions.ZimerfeldTree]].

## ⚖️ Consecuencias
**Positivas:**
- Flujo de trabajo fluido — la ventana permanece abierta mientras se hace stage/commit normalmente.
- Desacoplamiento total del host (no se suscribe a ningún evento) → robusta entre versiones de GitExtensions.
- Permite el **directorio de trabajo independiente** (ver [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]]).

**Negativas / trade-offs:**
- La ventana tiene que sondear el estado por sí sola (`RefreshStateAsync`) en lugar de reaccionar a eventos del host.
- Commit/Push necesitan delegates (`WithWorkingDirectory`) para reutilizar los diálogos nativos.

## 🔗 Relacionado
- [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]]
- [[🗂️ Fluxo em 3 etapas (abas) (ES)|🗂️ Flujo en 3 etapas (pestañas)]]
- [[🪟 LfsForm (ES)|🪟 LfsForm]]
- [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]]
