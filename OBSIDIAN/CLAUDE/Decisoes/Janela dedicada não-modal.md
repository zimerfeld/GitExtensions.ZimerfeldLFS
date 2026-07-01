---
tipo: decisao
tags: [decisao, adr, ui, janela, nao-modal]
status: aceita
criado: 2026-07-01
---

# ADR — Janela dedicada não-modal para o LFS

## Contexto
O Git LFS envolve várias operações distintas (detectar/instalar, rastrear padrões, listar arquivos, commit/push, pull/fetch/checkout) que o usuário executa em **momentos diferentes** do trabalho. Um diálogo modal bloquearia a janela principal do GitExtensions e forçaria abrir/fechar a cada operação.

## Decisão
Expor o plugin como uma **janela `Form` dedicada, não-modal e singleton** (`LfsForm`), aberta pelo menu Plugins → ZimerfeldLFS, que permanece disponível ao lado do GitExtensions enquanto o usuário trabalha. `Execute` retorna `false` (o host não atualiza a própria UI); a janela gerencia o próprio estado.

## Alternativas consideradas
- **Diálogo modal** — simples, mas bloqueia o host e não combina com operações intermitentes.
- **Integrar em telas existentes do host** (ex.: aba no commit) — acoplamento alto com a UI do GitExtensions, frágil entre versões.
- **Janela não-modal dedicada (escolhida)** — persistente, reutilizável, desacoplada; mesmo padrão do irmão [[GitExtensions.ZimerfeldTree]].

## Consequências
**Positivas:**
- Fluxo de trabalho fluido — a janela fica aberta enquanto se stagea/commita normalmente.
- Desacoplamento total do host (assina zero eventos) → robusta entre versões do GitExtensions.
- Permite o **diretório de trabalho independente** (ver [[Diretório de trabalho independente]]).

**Negativas / trade-offs:**
- A janela precisa sondar o estado sozinha (`RefreshStateAsync`) em vez de reagir a eventos do host.
- Commit/Push precisam de delegates (`WithWorkingDirectory`) para reaproveitar os diálogos nativos.

## 🔗 Relacionado
- [[Diretório de trabalho independente]]
- [[Fluxo em 3 etapas (abas)]]
- [[../Arquivos-Chave/LfsForm]]
- [[../Sistema/Arquitetura]]
