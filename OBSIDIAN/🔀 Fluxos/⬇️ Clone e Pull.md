---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
tags: [fluxo, clone, pull, fetch, checkout, etapa3]
---

# ⬇️ Fluxo: Etapa 3 — Clone & Pull

Terceira aba (`tabClone`). Baixar e restaurar o conteúdo LFS depois de clonar ou trocar de branch.

![[ScreenshotClone.png]]

## 🧭 Contexto

Quando colaboradores ou ferramentas de deploy **clonam** o repositório, o Git LFS baixa os arquivos pesados automaticamente ao fazer checkout da branch. Esta aba serve para **buscar ou restaurar** o conteúdo LFS manualmente quando necessário.

## 🔘 Botões

```
[git lfs pull]        → RunAsync → _svc.LfsPull()       (baixa o conteúdo LFS do checkout atual)
[git lfs fetch --all] → RunAsync → _svc.LfsFetchAll()   (pré-busca objetos LFS de TODAS as refs)
[git lfs checkout]    → RunAsync → _svc.LfsCheckout()   (popula a árvore de trabalho a partir dos objetos baixados)
[git lfs status]      → RunAsync → _svc.LfsStatus()     (mostra o status dos objetos LFS; refreshAfter:false)
```

| Comando | Quando usar |
|---|---|
| `git lfs pull` | trazer o conteúdo LFS que falta para o checkout atual |
| `git lfs fetch --all` | pré-baixar objetos LFS de todas as refs (ex.: antes de ficar offline) |
| `git lfs checkout` | materializar os arquivos de trabalho a partir de objetos já baixados |
| `git lfs status` | inspecionar o estado dos objetos LFS (só consulta — não atualiza a UI depois) |

Cada botão mostra no **console de saída** o comando exato e o stdout/stderr. Os botões ficam habilitados quando há repositório válido **e** o LFS disponível.

## 🔗 Relacionado

- [[📤 Track Commit Push]]
- [[⚡ LfsService]]
- [[🗃️ Git LFS - Conceitos]]
