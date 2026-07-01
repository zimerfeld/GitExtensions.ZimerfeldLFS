# GitExtensions.ZimerfeldLFS

![Ícone](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/src/GitExtensions.ZimerfeldLFS/Resources/icon-128.png)

**Versão:** 1.0.0 — **Atualizado em:** 2026-07-01

Plugin para o [GitExtensions](https://gitextensions.github.io/) que gerencia o **Git Large File Storage (LFS)** em uma janela dedicada e não-modal.

## O que é o Git LFS?

O Git Large File Storage (LFS) é uma extensão open-source do Git que substitui arquivos grandes (como áudio, vídeo ou datasets) por ponteiros de texto leves dentro do repositório. O conteúdo real do arquivo é hospedado em um servidor remoto separado, acelerando o clone e evitando o inchaço do repositório.

## As três etapas

A janela espelha o fluxo padrão do Git LFS em três abas.

### 1 · Instalação

- Windows e macOS: o Git LFS normalmente já vem incluído. Se precisar instalar manualmente, use o Homebrew (`brew install git-lfs`), o Chocolatey (`choco install git-lfs`) ou os binários oficiais em [git-lfs.com](https://git-lfs.com).
- **Verificar instalação** roda `git lfs version` e mostra o status detectado.
- **`git lfs install`** inicializa o LFS para a sua conta de usuário (executa uma vez por máquina).

### 2 · Fluxo básico — track / commit / push

Informe ao Git LFS quais tipos de arquivo gerenciar usando padrões glob:

```bash
git lfs track "*.psd"
git add .gitattributes
git add meu_arquivo_grande.psd
git commit -m "Adiciona arquivo PSD grande"
git push origin main
```

- Digite um padrão e clique em **Rastrear** — o plugin executa `git lfs track "<padrão>"` e adiciona o `.gitattributes` ao stage.
- A lista de **Padrões rastreados** tem um botão *Deixar de rastrear*; a lista de **Arquivos gerenciados pelo LFS** espelha `git lfs ls-files`.
- **Commit** e **Push** abrem os diálogos nativos do GitExtensions para o repositório selecionado.

### 3 · Clone & Pull

Quando colaboradores ou ferramentas de deploy clonam o repositório, o Git LFS baixa os arquivos pesados automaticamente ao fazer checkout da branch. Para buscar ou restaurar o conteúdo LFS depois:

- `git lfs pull` — baixa o conteúdo LFS do checkout atual
- `git lfs fetch --all` — pré-busca os objetos LFS de todas as refs
- `git lfs checkout` — popula os arquivos da árvore de trabalho a partir dos objetos baixados
- `git lfs status` — mostra o status dos objetos LFS

## Destaques

- **Diretório de trabalho independente** — um dropdown preenchido com o histórico de repositórios do GitExtensions.
- **Fluxo guiado em três etapas** — Instalação, Fluxo básico e Clone & Pull.
- **Estado do LFS ao vivo** — versão, inicialização, padrões rastreados e arquivos gerenciados pelo LFS, atualizados automaticamente.
- **Console de saída** — cada botão mostra o comando exato e sua saída.
- **Localizado (Inglês / Português)** com modo automático, além do **Mostrar Debug**.

## Instalação

- **Gerenciador de Plugins:** procure por *ZimerfeldLFS* (Plugins → Plugin Manager), instale, reinicie e abra **Plugins → ZimerfeldLFS**.
- **Manual:** execute o `build.ps1` como Administrador, ou copie `GitExtensions.Plugins.ZimerfeldLFS.dll` para `C:\Program Files\GitExtensions\Plugins\`, ou execute `tools\install.ps1` como Administrador.

## Requisitos

- GitExtensions 6.x (.NET 9)
- `git` e `git-lfs` no `PATH`

## Licença

Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (veja `LICENSE.txt`).
