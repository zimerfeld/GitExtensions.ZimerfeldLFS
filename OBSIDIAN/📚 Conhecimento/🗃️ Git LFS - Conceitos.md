---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, git, git-lfs, ponteiros, gitattributes]
---

# 🗃️ Git LFS — Conceitos

## Resumo
**Git Large File Storage (LFS)** é uma extensão open-source do Git que substitui **arquivos grandes** (áudio, vídeo, datasets, PSDs, binários) por **ponteiros de texto leves** dentro do repositório. O conteúdo real dos arquivos fica hospedado em um **servidor remoto separado**, o que **acelera o clone** e **evita o inchaço** do histórico do repositório.

## Ponteiro vs. conteúdo real
No commit, o Git armazena apenas um pequeno arquivo-ponteiro (algumas linhas: versão, oid SHA-256, tamanho) no lugar do arquivo pesado. O blob binário vai para o **LFS store** do remoto. Ao dar checkout, o Git LFS troca o ponteiro pelo conteúdo real (smudge); ao commitar, troca o conteúdo pelo ponteiro (clean) — os **filtros `filter.lfs.clean`/`smudge`** são instalados por `git lfs install`.

## `.gitattributes` e tracking
Os padrões rastreados ficam no arquivo **`.gitattributes`** (versionado). Rastrear um tipo:
```bash
git lfs track "*.psd"     # grava "*.psd filter=lfs diff=lfs merge=lfs -text" no .gitattributes
git add .gitattributes    # o .gitattributes PRECISA ser commitado
```
> [!important] Lembre de commitar o `.gitattributes`
> Sem o `.gitattributes` versionado, colaboradores não sabem que aqueles arquivos são LFS. Por isso a janela **stagea o `.gitattributes` automaticamente** após track/untrack — ver [[📤 Track Commit Push]].

## Comandos por etapa

| Etapa | Comando | Efeito |
|---|---|---|
| Instalação | `git lfs install` | grava os filtros LFS no config global (1× por máquina) |
| | `git lfs version` | verifica se o `git-lfs` está disponível |
| Tracking | `git lfs track "<glob>"` | adiciona o padrão ao `.gitattributes` |
| | `git lfs untrack "<glob>"` | remove o padrão |
| | `git lfs track` | lista os padrões rastreados |
| | `git lfs ls-files` | lista os arquivos gerenciados pelo LFS |
| Clone/Pull | `git lfs pull` | baixa o conteúdo LFS do checkout atual |
| | `git lfs fetch --all` | pré-busca objetos LFS de todas as refs |
| | `git lfs checkout` | popula a árvore de trabalho a partir dos objetos baixados |
| | `git lfs status` | mostra o status dos objetos LFS |

## Storage e clone
O `git clone` de um repo com LFS baixa os arquivos pesados **sob demanda** ao fazer checkout da branch (via smudge). `fetch --all` pré-baixa tudo (útil antes de ficar offline). Como o histórico guarda só ponteiros, clonar não arrasta todas as versões antigas dos binários — daí o ganho de velocidade e de tamanho.

## Onde isto aparece no plugin
Cada método do [[⚡ LfsService]] mapeia diretamente para um destes comandos; as três abas da [[🪟 LfsForm]] agrupam-nos por etapa. Ver [[🗂️ Fluxo em 3 etapas (abas)]].

## 🔗 Relacionado
- [[📦 GitExtensions.ZimerfeldLFS]]
- [[⚡ LfsService]]
- [[📤 Track Commit Push]]
