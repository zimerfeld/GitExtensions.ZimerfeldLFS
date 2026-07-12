# CLAUDE.md — Instruções do projeto

Guia para o Claude Code ao trabalhar neste repositório.

## Fluxo de publicação (gitflow) — NÃO criar nem aprovar PRs

O processo de publicação deste projeto já está estabelecido e é baseado em
**gitflow com GitHub Actions**: a branch `main` é publicada em produção
automaticamente pelo GitHub (Actions) ou via **wrangler** pelo terminal.

Por isso, ao concluir uma alteração:

- **NÃO** pergunte se deve criar um Pull Request, e **não** crie PRs.
- **NÃO** pergunte se deve aprovar nem tente aprovar PRs.
- **NÃO** faça merge de PRs nem push direto para `main`.

Em vez disso: faça commit na branch de trabalho designada e faça push dela.
O merge para `main` e a publicação em produção são conduzidos pelo mantenedor
através do gitflow/Actions estabelecido. Só crie/aprove/mescle PRs se o
usuário pedir explicitamente naquela conversa.
