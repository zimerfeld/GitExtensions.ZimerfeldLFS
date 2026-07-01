---
tipo: conhecimento
<<<<<<< HEAD
criado: 2026-07-01
=======
criado: 2026-06-01
>>>>>>> d1cd405ab922f9de4a92773297bfec8df3e99866
tags: [conhecimento, csharp, gitextensions, mef, plugin]
---

# Plugin MEF para GitExtensions

## Resumo
GitExtensions carrega plugins via **MEF** (Managed Extensibility Framework). O entry point é uma classe exportada que implementa `IGitPlugin` (normalmente herdando de `GitPluginBase`).

## Pontos-chave
- Exportar com `[Export(typeof(IGitPlugin))]` usando `System.ComponentModel.Composition`.
- Projeto compila como **`Library`** (DLL), `net9.0-windows`, WinForms habilitado.
<<<<<<< HEAD
- Referenciar os assemblies do host com **`<Private>false</Private>`** (não copiar para a saída — o host já os tem). No ZimerfeldLFS eles ficam **versionados em `refs\`** (build determinístico e offline):
  - `GitExtensions.Extensibility.dll`
  - `System.ComponentModel.Composition.dll`
- O `AssemblyName` precisa bater com o que `install.ps1` / nuspec esperam (`GitExtensions.Plugins.<Nome>`).
- Para aparecer no **Plugin Manager** interno, o pacote NuGet precisa **depender** de `GitExtensions.Extensibility` (dependência marcadora). Ver [[../Sistema/Dependências]].

## Ciclo de vida do plugin
- `Register(IGitUICommands)` — chamado ao carregar. Bom lugar para **capturar o `IGitUICommands`** (usado depois para abrir diálogos nativos) e, se desejado, assinar eventos (`PostRepositoryChanged`).
- `Unregister(IGitUICommands)` — desassinar eventos / limpar o commands capturado.
- `Execute(GitUIEventArgs)` — disparado pelo menu **Plugins → \<nome\>**. Acesso a `GitModule.WorkingDir`, `StartCommitDialog`, `StartPushDialog`, etc.

## Como o ZimerfeldLFS usa este modelo
- `: base(false)` no construtor → **sem** diálogo de settings (não implementa `GetSettings()`).
- `Execute` abre uma **janela não-modal singleton** (`LfsForm`) em vez de um diálogo modal, e retorna `false` (host não atualiza a própria UI). Ver [[../Arquivos-Chave/ZimerfeldLfsPlugin]] e [[../Decisoes/Janela dedicada não-modal]].
- **Desacoplado:** `Register` só **captura** o `IGitUICommands`; o plugin **não assina eventos do host**. O repositório-alvo vem do próprio dropdown da janela. Ver [[../Decisoes/Diretório de trabalho independente]].
- Os diálogos nativos de commit/push são abertos via `IGitUICommands.WithWorkingDirectory(dir)` → `StartCommitDialog` / `StartPushDialog`, no repo selecionado.

## 🔗 Relacionado
- [[GitExtensions.ZimerfeldLFS]]
- [[../Arquivos-Chave/ZimerfeldLfsPlugin]]
- [[GitExtensions.ZimerfeldTree]]
- [[GitExtensions.ZimerfeldCommitMsg]]
=======
- Referenciar os assemblies do GitExtensions de `C:\Program Files\GitExtensions\` com **`<Private>false</Private>`** (não copiar para a saída — o host já os tem):
  - `GitExtensions.Extensibility.dll`
  - `GitUIPluginInterfaces.dll`
  - `System.ComponentModel.Composition.dll`
- O `AssemblyName` precisa bater com o que `install.ps1` / nuspec esperam (`GitExtensions.Plugins.<Nome>`).

## Ciclo de vida do plugin
- `Register(IGitUICommands)` — chamado na **UI thread**. Bom lugar para capturar `SynchronizationContext.Current`, registrar templates de commit (`AddCommitTemplate`) e assinar eventos (`PostRepositoryChanged`).
- `Unregister(IGitUICommands)` — remover templates (`RemoveCommitTemplate`) e desassinar eventos.
- `Execute(GitUIEventArgs)` — disparado pelo menu **Plugins → \<nome\>**. Acesso a `GitModule.WorkingDir`, `StartCommitDialog`, etc.

## Descobrindo a API (truque do `inspector`)
Um console separado com `MetadataLoadContext` + reflection lista os tipos/membros públicos das DLLs do host sem carregá-las no runtime — forma rápida de achar a assinatura certa (`ICommitMessageManager`, `GitUIEventArgs`, …) ao evoluir o plugin. Ver `inspector\Program.cs` em [[GitExtensions.ZimerfeldCommitMsg]].

## 🔗 Relacionado
- [[GitExtensions.ZimerfeldCommitMsg]]
- [[GitExtensions.ZimerfeldTree]]
>>>>>>> d1cd405ab922f9de4a92773297bfec8df3e99866
