---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, icone, design, gdiplus]
---

# 💣 ADR — Ícone: 4 quadrantes + bomba

## Contexto
O plugin precisava de um ícone (para o menu Plugins, a janela e o pacote NuGet) que comunicasse de relance "**arquivos grandes de mídia**" — o domínio do Git LFS — e fosse gerado de forma **determinística e sem dependências** (parte da filosofia de build reprodutível do projeto).

## Decisão
Um ícone com a área dividida em **4 quadrantes iguais**, cada um um "cartão de arquivo" (file card) representando um tipo de arquivo grande, mais uma **bomba com pavio aceso** ao centro:
- superior-esquerdo — **joystick de arcade** (jogos), azul;
- superior-direito — **nota musical** com feixe duplo (áudio), roxo;
- inferior-esquerdo — **botão de play** (vídeo/filme), vermelho;
- inferior-direito — **cilindros de banco de dados** (datasets), verde-azulado;
- centro — **bomba com pavio** (estilo Super Mario Bros): a metáfora do "peso que explode" o repositório se versionado sem LFS.

Desenhado 100% em **GDI+ / System.Drawing** por [[🎨 Generate-LfsIcon]], produzindo `icon-128.png` (pacote/janela) e `ico.png` (16×16, menu).

## Iterações
Houve **duas iterações** de design:
1. inicial — "arquivos explodindo" (cartões de arquivo + explosão);
2. final — **4 quadrantes** com glifos por tipo + **bomba** central; nesta rodada, o joystick virou um **joystick de arcade completo** e o glifo de áudio virou uma **nota musical com feixe duplo**.

## Alternativas consideradas
- **Reaproveitar um ícone genérico de LFS** — sem identidade própria.
- **Um único símbolo** (ex.: só a bomba) — não comunica "vários tipos de mídia grande".
- **4 quadrantes + bomba (escolhida)** — comunica o domínio (jogos/áudio/vídeo/dados) **e** o problema que o LFS resolve.

## Consequências
**Positivas:**
- Determinístico, sem assets externos; regenerável a qualquer momento.
- Legível em 16×16 (renderizado em 64 e reduzido) e distinto no NuGet.

**Negativas / trade-offs:**
- Muitos detalhes competem em 16×16; a redução de alta qualidade mitiga, mas o ícone é mais rico em 128px.

## 🔗 Relacionado
- [[🎨 Generate-LfsIcon]]
- [[📦 GitExtensions.ZimerfeldLFS]]
