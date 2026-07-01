---
tipo: decisao
tags: [decisao, adr, ui, abas, fluxo, git-lfs]
status: aceita
criado: 2026-07-01
---

# ADR — Fluxo em 3 etapas (abas)

## Contexto
O Git LFS tem uma curva de aprendizado: instalar/inicializar, definir padrões de tracking (e lembrar de stagear o `.gitattributes`), commitar/pushar, e depois baixar/restaurar conteúdo em clones. Apresentar todos os botões juntos confundiria; esconder passos em menus dispersaria o fluxo.

## Decisão
Organizar a janela em um **`TabControl` de três abas** que espelham o fluxo padrão do Git LFS, na ordem em que o usuário o percorre:
1. **Instalação** — `git lfs version` + `git lfs install`.
2. **Fluxo básico** — track (glob) → stage `.gitattributes` → `ls-files` → Commit + Push.
3. **Clone & Pull** — `git lfs pull` / `fetch --all` / `checkout` / `status`.

## Alternativas consideradas
- **Uma tela única com todos os botões** — sobrecarga cognitiva; não comunica a ordem.
- **Assistente (wizard) linear** — rígido demais para uma ferramenta que também é usada de forma pontual.
- **Três abas (escolhida)** — agrupa por etapa, comunica a progressão e permite pular direto ao passo desejado.

## Consequências
**Positivas:**
- Ensina o fluxo de LFS pela própria estrutura da UI.
- Cada aba tem um texto de ajuda contextual (localizado).
- Console de saída compartilhado mostra o comando exato de qualquer botão.

**Negativas / trade-offs:**
- Alguns comandos poderiam pertencer a mais de uma etapa (ex.: `status`); a divisão é uma simplificação didática.

## 🔗 Relacionado
- [[Janela dedicada não-modal]]
- [[../Fluxos/1 - Instalação]] · [[../Fluxos/2 - Track Commit Push]] · [[../Fluxos/3 - Clone e Pull]]
- [[../02 - Conhecimento/Git LFS - Conceitos]]
