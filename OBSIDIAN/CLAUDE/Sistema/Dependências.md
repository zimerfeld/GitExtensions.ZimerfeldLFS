---
tipo: sistema
tags: [dependências, assemblies, gitextensions, git-lfs, nuget]
atualizado: 2026-07-01
---

# Dependências

## Assemblies do GitExtensions (referências de compilação)

Ambas referenciadas com `<Private>false</Private>` — **não** copiadas para o output (o host já as fornece em runtime).

| Assembly | Caminho | Uso |
|---|---|---|
| `GitExtensions.Extensibility.dll` | `refs\` (versionado no repo) | `IGitPlugin`, `GitPluginBase`, `IGitUICommands`, `GitUIEventArgs`, `IGitModule` |
| `System.ComponentModel.Composition.dll` | `refs\` (versionado no repo) | MEF — `[Export(typeof(IGitPlugin))]` |

> **Build determinístico (qualquer máquina Windows):** os assemblies de referência ficam **versionados em `refs\`** (apontados por `$(GitExtensionsRefPath)` no `.csproj`), **não** baixados em prebuild. Garante compilação reprodutível e **offline**. O `.csproj` demove o aviso `MSB3277` (o `WindowsBase` do host é 8.0 enquanto o ref pack net9 traz 4.0 — o runtime resolve o correto ao carregar).

## Dependência do pacote NuGet (marcador do Plugin Manager)

```xml
<dependency id="GitExtensions.Extensibility" version="1.0.0.129" />
```

> [!important] Por que a dependência marcadora existe
> O Plugin Manager do GitExtensions filtra o feed do nuget.org por pacotes que **dependem** de
> `GitExtensions.Extensibility`. **Sem** essa dependência, o pacote é publicado mas **nunca aparece**
> no Plugin Manager interno. `1.0.0.129` é a maior versão publicada `<=` GitExtensions 6.0.5,
> mantendo compatibilidade com a linha 6.x.

## Empacotamento (nuspec)

- DLL em **`lib\` raiz** (grupo "any" que o Plugin Manager extrai) — gera o aviso **NU5101**, intencional e filtrado no `build.ps1`. Ver [[Versionamento]].
- Mesma DLL também em `tools\net9.0-windows\` para o install via **Package Manager Console** (`install.ps1`).
- `LICENSE.txt` (CC BY-NC-ND 4.0, `type="file"`), `README.md`/`README.pt-BR.md`/`README.en-US.md`, e `icon.png` (de `Resources\icon-128.png`) na raiz do pacote.

## Interfaces-chave usadas

### `IGitPlugin` (via `GitPluginBase`)
- `Register(IGitUICommands)` / `Unregister(IGitUICommands)` — captura/limpa o `_commands`
- `Execute(GitUIEventArgs)` — chamado via menu Plugins → ZimerfeldLFS

### `IGitUICommands`
- `WithWorkingDirectory(dir)` — obtém um `IGitUICommands` apontado para o repo do `cboRepo`
- `StartCommitDialog(owner, message, …)` / `StartPushDialog(owner, …, out pushCompleted)` — diálogos nativos
- `RepoChangedNotifier?.Notify()` — pede ao host para atualizar a própria UI após commit/checkout
- `Module.WorkingDir` — working dir do host (usado só como valor inicial do dropdown)

## Runtime (o que o usuário precisa ter)

| Requisito | Valor |
|---|---|
| GitExtensions | 6.x (.NET 9) |
| .NET | 9.0 (Windows) — fornecido pelo host |
| `git` | no `PATH` (o `LfsService` roda subprocessos) |
| `git-lfs` | no `PATH` (extensão Git LFS) |
| PowerShell | 5.1+ (scripts de build/deploy) |
| .NET 9 SDK + nuget | para compilar e empacotar |

## Relacionado

- [[Arquitetura]]
- [[Versionamento]]
- [[../Arquivos-Chave/LfsService]]
