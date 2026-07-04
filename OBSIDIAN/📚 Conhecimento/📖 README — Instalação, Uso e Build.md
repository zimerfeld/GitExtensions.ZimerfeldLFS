---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, readme, instalacao, build, uso, git-lfs, i18n]
fonte: README.md
versao: 1.0.3
---

# 📖 README — Instalação, Uso e Build

> Espelho do `README.md` da raiz do repositório (bilíngue EN/PT), reconciliado com o código em 2026-07-01.
> Nota de projeto: [[📦 GitExtensions.ZimerfeldLFS]]. Conceitos em [[🗃️ Git LFS - Conceitos]].
> O `build.ps1` carimba versão + data nos READMEs **e** neste cofre a cada build (seções 4b/4c) — ver [[🔢 Versionamento]].

Plugin para **[GitExtensions](https://gitextensions.github.io/)** que gerencia o **Git Large File Storage (LFS)** em uma **janela dedicada e não-modal**. O Git LFS substitui arquivos grandes (áudio, vídeo, datasets) por **ponteiros de texto leves** no repositório; o conteúdo real fica em um servidor remoto separado, **acelerando o clone e evitando o inchaço do repositório**. A janela conduz o fluxo em **três etapas** e usa um diretório de trabalho escolhido **de forma independente do GitExtensions**.

## ✨ Funcionalidades em alto nível
- **Fluxo guiado em três etapas** — Instalação, Fluxo básico (track/commit/push) e Clone & Pull, cada um em sua aba.
- **Diretório de trabalho independente** — dropdown preenchido com o histórico de repositórios do GitExtensions; aponta a janela para qualquer repo sem depender do host. Ver [[📂 Diretório de Trabalho Independente]].
- **Estado do LFS ao vivo** — versão do `git lfs`, se está inicializado, padrões glob rastreados e arquivos LFS, atualizados automaticamente.
- **Console de saída** — cada botão mostra o comando `git`/`git lfs` exato e sua saída.
- **Localizado (Inglês / Português)** com modo automático seguindo o SO, mais o **Mostrar Debug** que revela os IDs dos controles.

## 🧩 O que é o Git LFS?
Extensão open-source do Git que troca arquivos grandes por ponteiros de texto leves; o conteúdo real fica num servidor remoto, acelerando o clone e evitando o inchaço. Detalhes em [[🗃️ Git LFS - Conceitos]].

## 🔢 As três etapas

> **Guias técnicos de operação (novo em 2026-07-01):** o `README.md` raiz permanece como resumo bilíngue de alto nível; os detalhes de **como operar a janela** foram separados em dois manuais por idioma — `README.en-US.md` (inglês) e `README.pt-BR.md` (português) — **linkados a partir da seção "The three steps / As três etapas"** do README raiz. Cada manual cobre: anatomia da janela, escolha do diretório de trabalho, leitura do console de saída, cada botão/status por aba e solução de problemas.

### 1 · Instalação
No Windows e macOS o Git LFS normalmente já vem incluído. Instalação manual via **Homebrew** (`brew install git-lfs`), **Chocolatey** (`choco install git-lfs`) ou binários oficiais em [git-lfs.com](https://git-lfs.com). Depois, **`git lfs install`** inicializa o LFS para a conta (1× por máquina). **Verificar instalação** roda `git lfs version`. Ver [[⚙️ Instalação]].

### 2 · Fluxo básico — track / commit / push
Rastreie tipos por **padrões glob** (`*.psd`, `*.mp4`, `*.zip`): digite o padrão e clique **Track** — o plugin roda `git lfs track "<padrão>"` e **stagea o `.gitattributes`**. A janela lista os padrões rastreados (com *Untrack*) e os arquivos LFS (`git lfs ls-files`). **Commit** e **Push** abrem os diálogos nativos do GitExtensions.
```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```
Ver [[📤 Track Commit Push]].

### 3 · Clone & Pull
Ao clonar, o Git LFS baixa os arquivos pesados automaticamente no checkout. Para buscar/restaurar depois: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`. Ver [[⬇️ Clone e Pull]].

## 📦 Instalação
**Via Plugin Manager do GitExtensions:** procure por *ZimerfeldLFS* (Plugins → Plugin Manager), instale, reinicie e abra **Plugins → ZimerfeldLFS**.

**Manual:** rode `build.ps1` (como Administrador para deploy automático) ou copie `GitExtensions.Plugins.ZimerfeldLFS.dll` para `C:\Program Files\GitExtensions\Plugins\`, ou rode `tools\install.ps1` como Administrador.

## ✅ Requisitos
- GitExtensions 6.x (.NET 9)
- `git` e `git-lfs` no `PATH`

## 🛠️ Build
```powershell
pwsh .\build.ps1          # incrementa versão, build Release, empacota o .nupkg
pwsh .\build.ps1 -Force   # sempre recompila/empacota
```
Ver [[🔢 Versionamento]] e [[🛠️ build.ps1]].

## 💜 Apoie o projeto
**GitHub Sponsors:** [github.com/sponsors/zimerfeld](https://github.com/sponsors/zimerfeld) · **Ko-fi:** [ko-fi.com/C0D621FCGD](https://ko-fi.com/C0D621FCGD). Badges no topo do README e **banner clicável no topo da janela** (`SponsorBanner.cs`).

## 🧩 Plugins integrados
Outros plugins do GitExtensions do mesmo autor, referenciados no rodapé dos READMEs:
- **[GitExtensions.ZimerfeldTree](https://github.com/zimerfeld/GitExtensions.ZimerfeldTree)** — ver [[GitExtensions.ZimerfeldTree]]
- **[GitExtensions.ZimerfeldCommitMsg](https://github.com/zimerfeld/GitExtensions.ZimerfeldCommitMsg)** — ver [[GitExtensions.ZimerfeldCommitMsg]]

## 📄 Licença
Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (`LICENSE.txt`).

## 🔗 Relacionado
- [[📦 GitExtensions.ZimerfeldLFS]]
- [[🗃️ Git LFS - Conceitos]]
- [[🔑 Fatos-Chave]]
