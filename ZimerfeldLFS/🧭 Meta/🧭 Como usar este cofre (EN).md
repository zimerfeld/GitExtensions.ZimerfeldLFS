---
tipo: meta
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-06-01
tags: [meta, protocolo]
---

# 🧭 How to use this vault (Claude's protocol)

> [!important] Memory protocol
> At the **start** of each session, read: [[🏠 Home (EN)|🏠 Home]] (priority map), [[📌 Backlog (EN)|📌 Backlog]] (resume point), [[🔑 Fatos-Chave (EN)|🔑 Key Facts]] and the mother note [[📦 GitExtensions.ZimerfeldLFS (EN)|📦 GitExtensions.ZimerfeldLFS]].
> At the **end** of each session, update [[📌 Backlog (EN)|📌 Backlog]] and the affected notes (content + frontmatter `atualizado:`).

## ✍️ When to write memory
| Situation | Where to write |
|-----------|----------------|
| High-impact source file | `🔑 Arquivos-Chave/` |
| Reused subsystem/service | `🧩 Sistemas/` |
| Mapped a usage flow | `🔀 Fluxos/` |
| How to run/publish (runbook) | `🚀 Operação/` |
| Made an architecture decision | `⚖️ Decisões/` |
| Reusable concept or pattern | `📚 Conhecimento/` |
| Goal, stack, monetization, adoption | `💼 Negócio/` |
| Key facts, Renato's context, tools, inbox | `🧭 Meta/` |
| Resume point / next steps | `📌 Backlog.md` (root) |

## 🔗 Writing rules
1. **Standard frontmatter** (`tipo`, `projeto`, `lang`, `atualizado`) on every note — see [[🏠 Home (EN)|🏠 Home]].
2. **Bilingual pair:** each PT note `<emoji> Name.md` has its `<emoji> Name (EN).md` pair (full translation).
3. **Emoji + space** at the start of every folder and file, and in `#`/`##`/`###` headings.
4. **Interlink** with `[[wikilinks]]` — PT notes link PT names; EN notes link the `(EN)` pairs.
5. **Atomicity**: one idea per note when possible. **ISO dates** `YYYY-MM-DD`.
6. Use **callouts** (`> [!note]`, `> [!warning]`) for highlights.

> [!note] Priority ordering
> The folder order in the explorer is defined by [[sortspec]] (the **custom-sort** plugin): impact → reuse → usage → operation. Do not delete.

## 🧩 Recommended plugins (optional)
- **Custom File Explorer sorting** (`custom-sort`) — **already installed**; applies the [[sortspec]] order.
- **Dataview** — dynamic lists (optional).
- **Templater** — advanced templates (optional).
Without the optional ones, the vault works normally.

## 🔗 Related
- [[🏠 Home (EN)|🏠 Home]] · [[📌 Backlog (EN)|📌 Backlog]] · [[🔑 Fatos-Chave (EN)|🔑 Key Facts]]
