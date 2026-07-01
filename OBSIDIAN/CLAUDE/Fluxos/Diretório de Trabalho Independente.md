---
tipo: fluxo
tags: [fluxo, working-dir, repositorio, dropdown, desacoplamento]
atualizado: 2026-07-01
---

# Fluxo: Diretório de Trabalho Independente

Como o dropdown `cboRepo` é populado e por que a janela fica **independente do repositório ativo do host**.

## Origem dos itens

```
LfsForm (construtor) → LoadRepositories()
        │
        ▼
LfsService.GetRepositoriesFromSettings()   (helper estático)
        │  lê:  %APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings
        │  acha o item key="history" → value é uma STRING XML aninhada
        │  parseia essa string → coleta cada <Path>
        ▼
lista de caminhos distintos (case-insensitive)
        │
        ▼
cboRepo.Items  (+ o WorkingDir inicial inserido no topo, se não estiver na lista)
        │
        ▼
pré-seleciona o WorkingDir inicial (ou o índice 0)
```

## Por que é independente

> [!important] Nenhum evento do host
> O plugin **não** assina `PostRepositoryChanged` nem qualquer outro evento do GitExtensions. O working dir do host (`args.GitModule.WorkingDir`) é usado **uma única vez**, apenas como valor pré-selecionado do `cboRepo` quando a janela abre. A partir daí, o repositório é escolhido **exclusivamente** pelo dropdown. Ver [[../Decisoes/Diretório de trabalho independente]].

## Troca de repositório

- **`CboRepo_SelectedIndexChanged`** — ao escolher outro item, seta `_svc.WorkingDir = dir` e dispara `RefreshStateAsync` (reprova versão do lfs, branch, padrões e arquivos do novo repo).
- **`UpdateWorkingDir(newDir)`** — quando a janela já está aberta e o usuário reaciona o menu Plugins num repo diferente, o plugin chama isto: adiciona o dir ao combo se necessário e o seleciona.

## Vantagem

A janela é **persistente e reutilizável**: dá para deixá-la aberta e gerenciar o LFS de **qualquer** repositório do histórico, sem precisar trocar o repositório ativo na janela principal do GitExtensions.

## Relacionado

- [[../Decisoes/Diretório de trabalho independente]]
- [[../Arquivos-Chave/LfsService]] — `GetRepositoriesFromSettings`
- [[../Arquivos-Chave/LfsForm]] — `LoadRepositories` / `cboRepo`
