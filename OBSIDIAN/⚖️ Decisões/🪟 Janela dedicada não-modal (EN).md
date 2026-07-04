---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, ui, janela, nao-modal]
---

# 🪟 ADR — Dedicated non-modal window for LFS

## 🧭 Context
Git LFS involves several distinct operations (detect/install, track patterns, list files, commit/push, pull/fetch/checkout) that the user performs at **different moments** of their work. A modal dialog would block GitExtensions' main window and force opening/closing on every operation.

## ✅ Decision
Expose the plugin as a **dedicated, non-modal, singleton `Form` window** (`LfsForm`), opened from the Plugins → ZimerfeldLFS menu, which remains available alongside GitExtensions while the user works. `Execute` returns `false` (the host does not refresh its own UI); the window manages its own state.

## 🔀 Alternatives considered
- **Modal dialog** — simple, but blocks the host and does not suit intermittent operations.
- **Integrating into existing host screens** (e.g.: a tab in the commit dialog) — high coupling with the GitExtensions UI, fragile across versions.
- **Dedicated non-modal window (chosen)** — persistent, reusable, decoupled; same pattern as the sibling [[GitExtensions.ZimerfeldTree]].

## ⚖️ Consequences
**Positive:**
- Smooth workflow — the window stays open while the user stages/commits normally.
- Total decoupling from the host (subscribes to zero events) → robust across GitExtensions versions.
- Enables the **independent working directory** (see [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]]).

**Negative / trade-offs:**
- The window has to probe state on its own (`RefreshStateAsync`) instead of reacting to host events.
- Commit/Push need delegates (`WithWorkingDirectory`) to reuse the native dialogs.

## 🔗 Related
- [[📁 Diretório de trabalho independente (EN)|📁 Independent working directory]]
- [[🗂️ Fluxo em 3 etapas (abas) (EN)|🗂️ 3-step flow (tabs)]]
- [[🪟 LfsForm (EN)|🪟 LfsForm]]
- [[🏛️ Arquitetura (EN)|🏛️ Architecture]]
