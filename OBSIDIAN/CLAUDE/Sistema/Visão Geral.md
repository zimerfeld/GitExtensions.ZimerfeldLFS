---
tipo: sistema
tags: [sistema, overview, plugin, gitextensions, git-lfs, i18n]
atualizado: 2026-07-01
---

# Visão Geral

## O que é

Plugin para **GitExtensions** (Windows) que gerencia o **Git Large File Storage (LFS)** em uma **janela dedicada e não-modal**. Guia o usuário pelo fluxo padrão de LFS em **três etapas** (abas) e opera sobre um **repositório escolhido no próprio dropdown da janela**, independente do host. Ver [[../02 - Conhecimento/Git LFS - Conceitos]] e [[../Decisoes/Janela dedicada não-modal]].

## Stack

| Item | Valor |
|---|---|
| Linguagem | C# (.NET 9) |
| Target | `net9.0-windows` |
| UI Framework | Windows Forms (`UseWindowsForms`) |
| Tipo de saída | `Library` (DLL carregada pelo GitExtensions) |
| Assembly de saída | `GitExtensions.Plugins.ZimerfeldLFS.dll` |
| Namespace | `GitExtensions.ZimerfeldLFS` |
| Versão atual | `1.0.2` |
| Idiomas | Português-BR / Inglês (auto pelo SO + override) |
| Licença | CC BY-NC-ND 4.0 © 2026 Zimerfeld |
| Autor | Zimerfeld |

> O plugin exibe um **ícone** (PNG embutido em `Resources/ico.png`, 16×16) no menu Plugins e na barra de título da janela. Carregado por `PluginIcon` (cache lazy). Ver [[../Arquivos-Chave/Generate-LfsIcon]].

## As três etapas (abas)

### 1 · Instalação
Detecta o `git lfs` (`git lfs version`) e inicializa-o para a conta do usuário (`git lfs install`, roda uma vez por máquina). A ajuda menciona Homebrew, Chocolatey e os binários oficiais de [git-lfs.com](https://git-lfs.com). Ver [[../Fluxos/1 - Instalação]].

### 2 · Fluxo básico (track / commit / push)
Rastreia tipos de arquivo por **padrões glob** (`git lfs track "*.psd"`) e **stagea o `.gitattributes`**; lista os padrões rastreados (com *Untrack*) e os arquivos LFS (`git lfs ls-files`); **Commit** e **Push** abrem os diálogos **nativos** do GitExtensions para o repositório selecionado. Ver [[../Fluxos/2 - Track Commit Push]].

### 3 · Clone & Pull
Baixa/restaura conteúdo LFS: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`. Ver [[../Fluxos/3 - Clone e Pull]].

## Recursos transversais

| Recurso | Detalhe |
|---|---|
| Diretório independente | `cboRepo` populado pelo histórico do GitExtensions — ver [[../Fluxos/Diretório de Trabalho Independente]] |
| Console de saída | mostra o comando exato + stdout/stderr (fundo escuro, fonte Consolas) |
| Localização | JSON embutido por idioma; escolha persistida em `%APPDATA%\GitExtensions\ZimerfeldLFS.language.json` |
| Mostrar Debug | tooltip com o `Name` de cada controle; persistido em `ZimerfeldLFS.uisettings.json` |
| Threading | janela abre instantânea; primeira sondagem no `Shown` em thread de fundo |

## Requisitos de runtime

- **GitExtensions 6.x** (.NET 9)
- **`git`** e **`git-lfs`** no `PATH`

## Relacionado

- [[Arquitetura]]
- [[Versionamento]]
- [[Dependências]]
- [[../02 - Conhecimento/Git LFS - Conceitos]]
