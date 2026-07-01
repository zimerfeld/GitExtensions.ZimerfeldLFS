---
tipo: decisao
tags: [decisao, adr, working-dir, desacoplamento, repositorio]
status: aceita
criado: 2026-07-01
---

# ADR — Diretório de trabalho independente do host

## Contexto
Cada janela do GitExtensions está vinculada a **um** repositório ativo. Amarrar a janela do LFS a esse repositório significaria que trocar de repo no host mudaria (ou quebraria) o alvo da janela — exatamente o tipo de bug de "parou de funcionar ao trocar de repositório" que o irmão CommitMsg teve de contornar.

## Decisão
Dar à janela um **dropdown próprio de repositório** (`cboRepo`), populado a partir do **histórico de repositórios do GitExtensions** (lido do arquivo `GitExtensions.settings`), e usar o working dir do host **apenas uma vez** como valor pré-selecionado. O plugin **não assina nenhum evento** do host; o repositório-alvo é escolhido exclusivamente pelo dropdown.

## Alternativas consideradas
- **Seguir o repositório ativo do host** (ouvir `PostRepositoryChanged`) — acoplamento e fragilidade; o alvo mudaria sob os pés do usuário.
- **Pedir o caminho manualmente** — sem descoberta; pior UX.
- **Dropdown do histórico (escolhida)** — descobre repositórios automaticamente, mantém a janela estável e reutilizável.

## Implementação
- `LfsService.GetRepositoriesFromSettings()` lê `%APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings`, extrai o item `key="history"` (cujo value é uma string XML aninhada) e coleta cada `<Path>`.
- `LfsForm.LoadRepositories()` popula o combo, insere o working dir inicial no topo se ausente e pré-seleciona.
- `CboRepo_SelectedIndexChanged` troca `_svc.WorkingDir` e reprova o estado. Ver [[../Fluxos/Diretório de Trabalho Independente]].

## Consequências
**Positivas:**
- Janela persistente que gerencia o LFS de **qualquer** repo do histórico.
- Desacoplamento total → robustez entre versões e ausência de bugs de "repo errado".

**Negativas / trade-offs:**
- Depende do formato do XML de settings do GitExtensions (parsing best-effort, tolerante a falhas → lista vazia).
- Repositórios fora do histórico não aparecem (só o inicial é inserido manualmente).

## 🔗 Relacionado
- [[Janela dedicada não-modal]]
- [[../Fluxos/Diretório de Trabalho Independente]]
- [[../Arquivos-Chave/LfsService]]
