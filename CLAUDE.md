# CLAUDE.md — Instruções do projeto

Guia para o Claude Code ao trabalhar neste repositório.

## Idioma do chat — sempre Português (BR)

Responda **sempre em Português do Brasil (pt-BR)** nas mensagens do chat,
independentemente do idioma em que a pergunta for feita. (Isto vale para a
comunicação com o usuário; o conteúdo de código, commits e documentos segue as
convenções de cada arquivo/idioma.)

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

## Rito pós-publicação — sincronizar `develop` e abrir nova feature

Sempre que a `main` for publicada (release/deploy), execute o rito gitflow
para que `main` e `develop` **não divirjam**:

1. **Sincronize `develop` com `main`** com um *back-merge* (`git checkout develop`
   → `git merge --no-ff main` → `git push origin develop`), seguindo a
   convenção de mensagem do repositório (`back-merge main into develop`).
   Assim tudo o que foi publicado na `main` volta para a `develop`.
2. **Crie a próxima feature a partir da `develop`** (`git checkout -b
   feature/<nome-sugestivo> develop`), com um **nome sugestivo** que descreva a
   próxima demanda. Todo trabalho novo começa a partir da `develop` já
   sincronizada, nunca a partir da `main` nem de uma `develop` defasada.
