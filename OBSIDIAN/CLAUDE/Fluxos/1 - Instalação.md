---
tipo: fluxo
tags: [fluxo, instalação, git-lfs, etapa1]
atualizado: 2026-07-01
---

# Fluxo: Etapa 1 — Instalação

Primeira aba da janela (`tabInstall`). Detecta o Git LFS e o inicializa para a conta do usuário.

![[ScreenShots/ScreenshotInstall.png]]

## Passos

```
Usuário abre a aba "1 · Instalação"
        │
        ▼
[Verificar instalação]  → RunAsync → _svc.GetLfsVersion()   (git lfs version)
        │
        ▼
RefreshStateAsync sonda: IsLfsAvailable() / IsLfsInitializedForUser()
        │
        ├─ NÃO disponível → status vermelho "Git LFS NÃO está instalado ou fora do PATH"
        │                    (botão "git lfs install" desabilitado)
        ├─ disponível, não inicializado → status dourado "… Clique em git lfs install"
        └─ pronto → status verde "Git LFS pronto — <versão> (inicializado para este usuário)"
        │
        ▼
[git lfs install]  → RunAsync("git lfs install") → _svc.LfsInstall()   (roda 1× por máquina)
```

## Detalhes

- **`GetLfsVersion()`** = `git lfs version`; stdout vazio ⇒ não instalado / fora do PATH.
- **`IsLfsAvailable()`** = versão ok e stdout começa com `git-lfs`.
- **`IsLfsInitializedForUser()`** = `git config --global --get filter.lfs.clean` não-vazio (o `git lfs install` grava os filtros LFS no config global).
- **Ajuda na aba** (`step1Help`): no Windows e macOS o Git LFS normalmente já vem incluído; instalação manual via **Homebrew** (`brew install git-lfs`), **Chocolatey** (`choco install git-lfs`) ou binários oficiais em [git-lfs.com](https://git-lfs.com).

## Estado após a etapa

Com o LFS pronto e um repositório válido selecionado, os botões das etapas 2 e 3 ficam habilitados (`workflowEnabled = IsRepo && LfsAvailable`).

## Relacionado

- [[2 - Track Commit Push]]
- [[../Arquivos-Chave/LfsService]]
- [[../02 - Conhecimento/Git LFS - Conceitos]]
