---
tipo: meta
projeto: GitExtensions.ZimerfeldLFS
lang: en-US
atualizado: 2026-07-04
criado: 2026-06-01
tags: [meta, ferramenta, cli, rtk]
---

# 🦀 RTK — Rust Token Killer

> 🇧🇷 Leia esta página em português → [[🦀 RTK]]

## 📌 Summary
A CLI proxy that saves **60–90% of tokens** in development operations. It rewrites commands automatically via a Claude Code hook (e.g.: `git status` → `rtk git status`, transparent, 0 tokens of overhead).

## 🧰 Meta-commands (use rtk directly)
```bash
rtk gain              # Shows token-saving analytics
rtk gain --history    # Command usage history with savings
rtk discover          # Analyzes Claude Code history for missed opportunities
rtk proxy <cmd>       # Runs a raw command without filtering (debug)
```

## ✅ Installation check
```bash
rtk --version         # rtk X.Y.Z
rtk gain              # Should work (not "command not found")
which rtk             # Verify the correct binary
```

> [!warning] Name collision
> If `rtk gain` fails, the reachingforthejack/rtk (Rust Type Kit) may be installed instead of the correct one.

## 🔗 Related
- [[🔑 Fatos-Chave (EN)|🔑 Key Facts]]
- [[👤 Renato (EN)|👤 Renato]]
