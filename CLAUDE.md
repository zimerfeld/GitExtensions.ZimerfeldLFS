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

## Paridade de idiomas — PT, EN e ES sempre juntos

Este projeto é **trilíngue**: Português (`pt-BR`), Inglês (`en-US`) e Espanhol
(`es-ES`). Ao criar ou alterar qualquer conteúdo com variantes por idioma,
**sempre crie/atualize as três versões na mesma alteração** — nunca deixe um
idioma para trás. A paridade entre PT, EN e ES deve ser mantida em:

- **READMEs:** `README.md` (resumo bilíngue), `README.pt-BR.md`,
  `README.en-US.md` e `README.es-ES.md`.
- **Cofre Obsidian:** cada nota tem as variantes base (PT), `(EN)` e `(ES)`; ao
  criar ou editar uma nota, replique a mudança nas três (incluindo frontmatter
  `lang:` e os wikilinks apontando para os irmãos do mesmo idioma).
- **Dicionários de UI:** `Resources/ZimerfeldLFS.pt-BR.json`,
  `ZimerfeldLFS.en-US.json` e `ZimerfeldLFS.es-ES.json` devem ter **exatamente
  as mesmas chaves** (nenhuma chave só em um idioma).
- **Landing page** (`index.html`) e qualquer outro texto com variantes por idioma.

Ao acrescentar uma nova string, seção ou documento em um idioma, traduza e
atualize **imediatamente** os outros dois idiomas para não quebrar a paridade.

## Pílula de idioma — sempre AUTO / PT / EN / ES (AUTO pré-selecionado)

O seletor (pílula) de idioma deve **sempre** oferecer as quatro opções, nesta
ordem — **AUTO**, **PT**, **EN**, **ES** — com **AUTO pré-selecionado** por
padrão. `AUTO` segue o idioma do sistema/navegador; `PT`, `EN` e `ES` forçam o
idioma correspondente.

Esta regra vale para **todas** as pílulas/seletores de idioma do projeto:

- **Janela do plugin** (`LfsForm` / `_cboLanguage`): itens
  `Automático / Português / Inglês / Espanhol`, com `Automático` selecionado
  quando não há escolha persistida.
- **Landing page** (`index.html`): a pílula de idioma deve seguir a mesma ordem
  e default `AUTO`.

Ao acrescentar um novo idioma, ele entra na pílula mantendo `AUTO` na frente e
`AUTO` como pré-selecionado.
