---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, csharp, gitextensions, mef, plugin]
---

# 🧩 Plugin MEF para GitExtensions

## Resumo
GitExtensions carrega plugins via **MEF** (Managed Extensibility Framework). O entry point é uma classe exportada que implementa `IGitPlugin` (normalmente herdando de `GitPluginBase`).

## Pontos-chave
- Exportar com `[Export(typeof(IGitPlugin))]` usando `System.ComponentModel.Composition`.
- Projeto compila como **`Library`** (DLL), `net9.0-windows`, WinForms habilitado.
- Referenciar os assemblies do host com **`<Private>false</Private>`** (não copiar para a saída — o host já os tem). No ZimerfeldLFS eles ficam **versionados em `refs\`** (build determinístico e offline):
  - `GitExtensions.Extensibility.dll`
  - `System.ComponentModel.Composition.dll`
- O `AssemblyName` precisa bater com o que `install.ps1` / nuspec esperam (`GitExtensions.Plugins.<Nome>`).
- Para aparecer no **Plugin Manager** interno, o pacote NuGet precisa **depender** de `GitExtensions.Extensibility` (dependência marcadora). Ver [[🧱 Dependências]].

## Ciclo de vida do plugin
- `Register(IGitUICommands)` — chamado ao carregar. Bom lugar para **capturar o `IGitUICommands`** (usado depois para abrir diálogos nativos) e, se desejado, assinar eventos (`PostRepositoryChanged`).
- `Unregister(IGitUICommands)` — desassinar eventos / limpar o commands capturado.
- `Execute(GitUIEventArgs)` — disparado pelo menu **Plugins → \<nome\>**. Acesso a `GitModule.WorkingDir`, `StartCommitDialog`, `StartPushDialog`, etc.

## Como o ZimerfeldLFS usa este modelo
- `: base(false)` no construtor → **sem** diálogo de settings (não implementa `GetSettings()`).
- `Execute` abre uma **janela não-modal singleton** (`LfsForm`) em vez de um diálogo modal, e retorna `false` (host não atualiza a própria UI). Ver [[🔌 ZimerfeldLfsPlugin]] e [[🪟 Janela dedicada não-modal]].
- **Desacoplado:** `Register` só **captura** o `IGitUICommands`; o plugin **não assina eventos do host**. O repositório-alvo vem do próprio dropdown da janela. Ver [[📁 Diretório de trabalho independente]].
- Os diálogos nativos de commit/push são abertos via `IGitUICommands.WithWorkingDirectory(dir)` → `StartCommitDialog` / `StartPushDialog`, no repo selecionado.

## 🔗 Relacionado
- [[📦 GitExtensions.ZimerfeldLFS]]
- [[🔌 ZimerfeldLfsPlugin]]
- [[GitExtensions.ZimerfeldTree]]
- [[GitExtensions.ZimerfeldCommitMsg]]
