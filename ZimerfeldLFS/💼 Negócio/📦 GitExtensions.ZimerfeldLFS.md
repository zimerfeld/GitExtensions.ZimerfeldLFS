---
tipo: negocio
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
criado: 2026-07-01
tags: [projeto, negocio, csharp, gitextensions, plugin, winforms, git-lfs, i18n]
status: ativo
linguagem: C#
versao: 1.0.4
repo: C:\GitExtensions\GitExtensions.ZimerfeldLFS
---

# 📦 GitExtensions.ZimerfeldLFS

> 🇺🇸 Read this page in English → [[📦 GitExtensions.ZimerfeldLFS (EN)]]

## 🎯 Objetivo
Plugin para **[GitExtensions](https://gitextensions.github.io/)** que gerencia o **Git Large File Storage (LFS)** em uma **janela dedicada e não-modal**. O Git LFS substitui arquivos grandes (áudio, vídeo, datasets) por **ponteiros de texto leves** dentro do repositório; o conteúdo real fica em um servidor remoto separado — **acelerando o clone e evitando o inchaço do repositório**. A janela conduz o usuário pelo fluxo padrão de LFS em **três etapas** (abas) e aponta para um **diretório de trabalho escolhido de forma independente do GitExtensions**. Ver [[🗃️ Git LFS - Conceitos]].

## 💜 Financiamento / Patrocínio
Canais de doação configurados (badges no topo do README + **banner clicável no topo da janela**, ver [[🪟 LfsForm]] / `SponsorBanner.cs`):
- **GitHub Sponsors:** `@zimerfeld` → https://github.com/sponsors/zimerfeld
- **Ko-fi:** `C0D621FCGD` → https://ko-fi.com/C0D621FCGD
- **Prova social no README:** badges de versão e **downloads do NuGet** (`shields.io/nuget/v` e `/dt`).

## 📂 Estrutura do repositório
```
C:\GitExtensions\GitExtensions.ZimerfeldLFS\
├─ src\GitExtensions.ZimerfeldLFS\        # código do plugin (.csproj)
│   ├─ ZimerfeldLfsPlugin.cs             # entry point MEF (Execute/Register/Unregister)
│   ├─ LfsForm.cs                        # a janela: 3 abas + dropdown de repo + log + i18n
│   ├─ LfsService.cs                     # runner de git / git lfs (subprocessos)
│   ├─ Localization.cs                   # I18n + Translator (dicionários JSON embutidos)
│   ├─ SponsorBanner.cs                  # banner GitHub Sponsors + Ko-fi (topo da janela)
│   ├─ PluginIcon.cs                     # carrega o ícone embutido (ico.png)
│   ├─ Resources\                        # ico.png, icon-128.png, badges, ZimerfeldLFS.<culture>.json
│   ├─ *.csproj / *.nuspec               # build + manifesto NuGet
├─ refs\                                 # DLLs do host versionadas (build determinístico)
├─ tools\                                # install/uninstall .ps1, nuget.exe, icon-generator
│   ├─ icon-generator\Generate-LfsIcon.ps1  # gerador GDI+ do ícone
│   └─ net9.0-windows\                   # saída do build (DLL) usada pelo nupkg
├─ ZimerfeldLFS\                         # 🧠 este cofre de memória
├─ build.ps1                             # incrementa versão + build + deploy + nupkg
├─ README.md / README.pt-BR.md / README.en-US.md  # espelhado em [[📖 README — Instalação, Uso e Build]]
└─ GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg
```

## ⚙️ Stack técnica
- **Linguagem:** C# (`net9.0-windows`), `Nullable` + `ImplicitUsings` habilitados, `LangVersion=latest`
- **UI:** WinForms (`UseWindowsForms`) — janela própria (`LfsForm`), não usa telas do host além dos diálogos nativos de commit/push
- **Tipo de saída:** `Library` (DLL carregada pelo GitExtensions)
- **AssemblyName:** `GitExtensions.Plugins.ZimerfeldLFS`
- **Namespace raiz:** `GitExtensions.ZimerfeldLFS`
- **NeutralLanguage:** `pt-BR`
- **Plugin model:** MEF (`[Export(typeof(IGitPlugin))]`) — ver [[🧩 Plugin MEF para GitExtensions]]
- **Referências externas** (de `refs\`, `Private=false`): `GitExtensions.Extensibility.dll`, `System.ComponentModel.Composition.dll`

## ✨ Funcionalidades principais
- **Fluxo guiado em três etapas (abas):** ver [[⚙️ Instalação]], [[📤 Track Commit Push]], [[⬇️ Clone e Pull]].
  1. **Instalação** — `git lfs version` (detectar) + `git lfs install` (inicializar para o usuário); ajuda menciona Homebrew/Chocolatey/binários oficiais.
  2. **Fluxo básico** — rastrear padrões glob (`git lfs track "*.psd"`) e **stagear o `.gitattributes`**; listar padrões rastreados (+ untrack); listar arquivos LFS (`git lfs ls-files`); **Commit** e **Push** via diálogos nativos do host.
  3. **Clone & Pull** — `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`.
- **Diretório de trabalho independente:** dropdown `cboRepo` populado a partir do **histórico de repositórios do GitExtensions** (lido do arquivo de settings), sem depender da janela do host. Ver [[📂 Diretório de Trabalho Independente]].
- **Console de saída (log):** cada botão mostra o **comando `git`/`git lfs` exato** e sua saída num console escuro — nada fica escondido.
- **Estado do LFS ao vivo:** versão detectada, se está inicializado para o usuário, padrões rastreados e arquivos LFS — atualizados automaticamente após cada operação e ao trocar de repo.
- **Localização (PT-BR / EN-US):** modo automático seguindo o SO + override manual (dropdown Automático/Inglês/Português). Ver [[Localization.cs|I18n]].
- **Mostrar Debug:** alternância que exibe o `Name` de cada controle como tooltip (auxílio de desenvolvimento herdado do ZimerfeldTree).

## 🏗️ Arquitetura (Plugin → Form → Service)
Três classes, cada uma com uma responsabilidade única (ver [[🏛️ Arquitetura]]):
```
GitExtensions (host)
    │ MEF
    ▼
ZimerfeldLfsPlugin   ← [Export(IGitPlugin)], base(false) = sem diálogo de settings
    │ abre singleton, passa delegates openCommit/openPush (WithWorkingDirectory)
    ▼
LfsForm (a janela)  ── usa ──►  LfsService (subprocessos git / git lfs → GitResult)
```
O plugin **não assina nenhum evento do host** — é totalmente desacoplado. O working dir do host é usado **apenas uma vez**, como valor pré-selecionado do `cboRepo` ao abrir a janela.

## 🛠️ Build / instalação
```powershell
# Build: incrementa build, compila Release, faz deploy (Admin), gera nupkg
.\build.ps1
.\build.ps1 -Force   # sempre recompila/empacota

# Scripts auxiliares em tools\ (Admin p/ Program Files)
tools\install.ps1      # instala o plugin (também via Package Manager Console)
tools\uninstall.ps1    # remove (não afeta o resto do GitExtensions)
```
> Via **Plugin Manager do GitExtensions**: buscar por *ZimerfeldLFS* e instalar. Passo a passo em [[📖 README — Instalação, Uso e Build]] e [[🔢 Versionamento]].

## 🎨 Ícone
Área dividida em **4 quadrantes iguais**, cada um um "cartão de arquivo" (file card) de um tipo de arquivo grande: **joystick de arcade** (azul), **nota musical** com feixe duplo para áudio (roxo), **botão de play** de filme (vermelho) e **cilindros de banco de dados** (verde-azulado); ao centro, uma pequena **bomba com pavio aceso** (estilo Super Mario Bros). Gerado 100% via GDI+ em `Generate-LfsIcon.ps1` → `icon-128.png` (pacote/janela) e `ico.png` (16×16, menu). Ver [[🎨 Generate-LfsIcon]] e [[💣 Ícone 4 quadrantes + bomba]].

## 💰 Ângulo de investimento
- **Nicho subatendido:** o GitExtensions não tem uma tela de LFS guiada; usuários caem na linha de comando. Este plugin transforma um fluxo intimidante (`track` / `.gitattributes` / `ls-files` / `pull`/`fetch`/`checkout`) numa experiência de 3 cliques.
- **Público real:** times de jogos, design, áudio/vídeo, data science — exatamente quem sofre com repositórios inchados e é candidato a adotar LFS.
- **Custo marginal baixo:** compartilha a infraestrutura (build, i18n, banner, ícone GDI+, refs versionados) dos irmãos [[GitExtensions.ZimerfeldTree]] e [[GitExtensions.ZimerfeldCommitMsg]] — portfólio coeso reforça a marca **Zimerfeld** no ecossistema GitExtensions/NuGet.
- **Distribuição pronta:** publicado no NuGet e visível no Plugin Manager interno (dependência marcadora `GitExtensions.Extensibility`).

## 🐛 Armadilhas conhecidas
> [!warning] `git` e `git-lfs` precisam estar no PATH
> `LfsService` roda `git`/`git lfs` como subprocessos. Se não estiverem no `PATH`, os comandos retornam `GitResult` com `ExitCode -1` e a aba de Instalação mostra "Git LFS NÃO está instalado". Não é um crash — é surfaçado como resultado falho.

<!-- -->

> [!warning] DLL em `lib\` RAIZ no nuspec (aviso NU5101 intencional)
> O Plugin Manager do GitExtensions só extrai um grupo `lib` cujo target framework esteja na lista de monikers (inclui `any`). A DLL vai em `lib\` **raiz** (grupo "any"); uma subpasta `lib\net9.0-windows\` **não** seria extraída. Por isso o aviso **NU5101** do `nuget pack` é **filtrado de propósito** no `build.ps1`. Ver [[🔢 Versionamento]] e [[🧱 Dependências]].

## 🔢 Versionamento
- Versão atual: **1.0.4** (csproj + nuspec sincronizados pelo `build.ps1`)
- Esquema: `major.minor.BUILD`, BUILD auto-incrementado a cada build
- A cada build, o `build.ps1` carimba versão + data nos **READMEs** (seção 4b) **e** neste cofre Obsidian (seção 4c), mantendo tudo em sincronia

## 🔗 Relacionado
- [[📖 README — Instalação, Uso e Build]] — espelho do `README.md`
- [[🧩 Plugin MEF para GitExtensions]]
- [[🗃️ Git LFS - Conceitos]]
- [[🏛️ Arquitetura]] · [[🔭 Visão Geral]] · [[🔢 Versionamento]] · [[🧱 Dependências]]
- [[GitExtensions.ZimerfeldTree]] — irmão (árvore de branches)
- [[GitExtensions.ZimerfeldCommitMsg]] — irmão (mensagens de commit)
- [[🔑 Fatos-Chave]]
