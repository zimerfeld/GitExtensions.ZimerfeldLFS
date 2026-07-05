---
tipo: meta
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
criado: 2026-06-01
tags: [meta, protocolo]
---

# 🧭 Como usar este cofre (protocolo do Claude)

> 🇺🇸 Read this page in English → [[🧭 Como usar este cofre (EN)]]

> [!important] Protocolo de memória
> No **início** de cada sessão, leia: [[🏠 Home]] (mapa por prioridade), [[📌 Backlog]] (ponto de retomada), [[🔑 Fatos-Chave]] e a nota-mãe [[📦 GitExtensions.ZimerfeldLFS]].
> No **fim** de cada sessão, atualize [[📌 Backlog]] e as notas afetadas (conteúdo + `atualizado:` do frontmatter).

## ✍️ Quando gravar memória
| Situação | Onde gravar |
|----------|-------------|
| Arquivo-fonte de alto impacto | `🔑 Arquivos-Chave/` |
| Subsistema/serviço reutilizado | `🧩 Sistemas/` |
| Mapeei um fluxo de uso | `🔀 Fluxos/` |
| Como rodar/publicar (runbook) | `🚀 Operação/` |
| Tomamos uma decisão de arquitetura | `⚖️ Decisões/` |
| Conceito ou padrão reutilizável | `📚 Conhecimento/` |
| Objetivo, stack, monetização, adoção | `💼 Negócio/` |
| Fatos-chave, contexto do Renato, ferramentas, inbox | `🧭 Meta/` |
| Ponto de retomada / próximos passos | `📌 Backlog.md` (raiz) |

## 🔗 Regras de escrita
1. **Frontmatter padrão** (`tipo`, `projeto`, `lang`, `atualizado`) em toda nota — ver [[🏠 Home]].
2. **Par bilíngue:** cada nota PT `<emoji> Nome.md` tem seu par `<emoji> Nome (EN).md` (tradução integral).
3. **Emoji + espaço** no começo de toda pasta e todo arquivo, e nos títulos `#`/`##`/`###`.
4. **Interligue** com `[[wikilinks]]` — notas PT linkam nomes PT; notas EN linkam os pares `(EN)`.
5. **Atomicidade**: uma ideia por nota quando possível. **Datas em ISO** `AAAA-MM-DD`.
6. Use **callouts** (`> [!note]`, `> [!warning]`) para destaques.

> [!note] Ordenação por prioridade
> A ordem das pastas no explorador é definida por [[sortspec]] (plugin **custom-sort**): impacto → reutilização → uso → operação. Não apagar.

## 🧩 Plugins recomendados (opcionais)
- **Custom File Explorer sorting** (`custom-sort`) — **já instalado**; aplica a ordem de [[sortspec]].
- **Dataview** — listas dinâmicas (opcional).
- **Templater** — templates avançados (opcional).
Sem os opcionais, o cofre funciona normalmente.

## 🔗 Relacionado
- [[🏠 Home]] · [[📌 Backlog]] · [[🔑 Fatos-Chave]]
