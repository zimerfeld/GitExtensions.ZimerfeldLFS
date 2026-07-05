---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, ui, abas, fluxo, git-lfs]
---

# 🗂️ ADR — 3-step flow (tabs)

## 🧭 Context
Git LFS has a learning curve: install/initialize, define tracking patterns (and remember to stage the `.gitattributes`), commit/push, and later download/restore content in clones. Presenting all buttons together would confuse; hiding steps in menus would scatter the flow.

## ✅ Decision
Organize the window into a **three-tab `TabControl`** that mirrors the standard Git LFS flow, in the order the user goes through it:
1. **Installation** — `git lfs version` + `git lfs install`.
2. **Basic flow** — track (glob) → stage `.gitattributes` → `ls-files` → Commit + Push.
3. **Clone & Pull** — `git lfs pull` / `fetch --all` / `checkout` / `status`.

## 🔀 Alternatives considered
- **A single screen with all buttons** — cognitive overload; does not convey the order.
- **A linear wizard** — too rigid for a tool that is also used ad hoc.
- **Three tabs (chosen)** — groups by step, conveys the progression and lets you jump straight to the desired step.

## ⚖️ Consequences
**Positive:**
- Teaches the LFS flow through the UI structure itself.
- Each tab has contextual help text (localized).
- A shared output console shows the exact command of any button.

**Negative / trade-offs:**
- Some commands could belong to more than one step (e.g.: `status`); the split is a didactic simplification.

## 🔗 Related
- [[🪟 Janela dedicada não-modal (EN)|🪟 Dedicated non-modal window]]
- [[⚙️ Instalação (EN)|⚙️ Installation]] · [[📤 Track Commit Push (EN)|📤 Track Commit Push]] · [[⬇️ Clone e Pull (EN)|⬇️ Clone & Pull]]
- [[🗃️ Git LFS - Conceitos (EN)|🗃️ Git LFS — Concepts]]
