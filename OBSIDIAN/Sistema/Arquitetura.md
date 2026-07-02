---
tipo: sistema
tags: [arquitetura, classes, design, i18n, threading]
atualizado: 2026-07-01
---

# Arquitetura

## Diagrama de classes

```
GitExtensions (host)
    │
    │  MEF (System.ComponentModel.Composition)
    ▼
ZimerfeldLfsPlugin        ← [Export(IGitPlugin)] : GitPluginBase, base(false)
    │  Execute()  → abre/traz-à-frente a janela singleton
    │  captura _commands (Register/Unregister)
    │  passa delegates: openCommit / openPush  (via WithWorkingDirectory)
    ▼
LfsForm (a janela)        ← FixedSingle, não-modal, 3 abas + log + i18n
    │  cboRepo (dropdown independente)  →  WorkingDir
    │  RunAsync(title, op)  →  Task.Run
    ▼
LfsService                ← subprocessos git / git lfs
    │  RunGit(args) → GitResult(StdOut, StdErr, ExitCode)
    ▼
git / git lfs (PATH)
```

## As três classes

### `ZimerfeldLfsPlugin` — ponto de entrada
Herda de `GitPluginBase`, exportado via MEF. Construtor `: base(false)` → **sem** diálogo de settings em Configurações → Plugins. Ver [[../Arquivos-Chave/ZimerfeldLfsPlugin]].
- **`Execute`** — abre (ou traz à frente) a **janela singleton** `LfsForm`; passa os delegates `openCommit`/`openPush` que abrem os diálogos nativos do host **no working dir selecionado no `cboRepo`** (`_commands.WithWorkingDirectory(dir)` → `StartCommitDialog`/`StartPushDialog`). Retorna `false` (host **não** deve atualizar a própria UI).
- **`Register`/`Unregister`** — capturam/limpam `_commands` (`IGitUICommands`).

### `LfsForm` — a janela
WinForms `FixedSingle`, não-maximizável, `CenterScreen`, Segoe UI 9. De cima para baixo: banner de patrocínio, dropdown de repo independente + branch, `TabControl` de 3 abas, console de log escuro, painel inferior (Debug + Idioma + Fechar), status strip. Ver [[../Arquivos-Chave/LfsForm]].

### `LfsService` — executor git/git-lfs
`RunGit(args)` roda `git` num `ProcessStartInfo` (redirecionando stdout/stderr, UTF-8, sem janela) e devolve um `GitResult(StdOut, StdErr, ExitCode)` (record struct; `Ok` = exit 0; `Combined` = stdout+stderr aparado). Métodos de estado, das 3 etapas, e o helper estático `GetRepositoriesFromSettings`. Ver [[../Arquivos-Chave/LfsService]].

## Desacoplamento do host

> [!important] O plugin assina **NENHUM** evento do host
> Ao contrário do irmão CommitMsg (que ouve `PostRepositoryChanged`), o ZimerfeldLFS é **totalmente desacoplado**: a janela escolhe o repositório **exclusivamente** pelo próprio `cboRepo`. O working dir do host é usado **uma única vez**, como valor pré-selecionado ao abrir a janela. `Register` só guarda o `_commands` para poder abrir os diálogos nativos; não há re-vinculação nem `Application.Idle`. Ver [[../Decisoes/Diretório de trabalho independente]].

O único ponto de contato com o host é opcional: os delegates `openCommit`/`openPush`, que resolvem `IGitUICommands.WithWorkingDirectory(dir)` para abrir os diálogos nativos no repo escolhido. Se indisponíveis, o log registra o fallback (o Push cai em `git push` puro).

## Localização (i18n)

`I18n` (estático) resolve o idioma (`Automatic` segue o SO via `CultureInfo.CurrentUICulture`; senão `en-US`/`pt-BR`) e carrega o dicionário do **JSON embutido** `Resources\ZimerfeldLFS.<culture>.json` via `GetManifestResourceStream`, com fallback en-US → mapa vazio. Um `Translator` devolve a string por chave (ou a própria chave se ausente) e tem `F(key, args)` para `string.Format`. A escolha é lida do disco **uma vez** no start e depois dirigida em memória pelo dropdown; persistida em `%APPDATA%\GitExtensions\ZimerfeldLFS.language.json`. Ver [[../Arquivos-Chave/LfsForm]] e `Localization.cs`.

## Threading

> A janela **abre instantânea**: o construtor faz **zero trabalho git** (só popula o `cboRepo` lendo o XML de settings). A primeira sondagem roda atrás do evento `Shown`.

- **`RefreshStateAsync`** — sonda o repositório numa `Task.Run` em thread de fundo (versão do lfs, disponível?, inicializado?, branch, padrões, arquivos → `StateSnapshot`) e aplica o resultado à UI (`ApplyState`). Chamada no `Shown`, ao trocar de repo no `cboRepo`, e após operações.
- **`RunAsync(title, op, refreshAfter)`** — roda uma operação git/git-lfs em `Task.Run`, loga o comando e a saída, e (opcionalmente) chama `RefreshStateAsync`. Chamadas concorrentes são ignoradas enquanto `_busy`.
- **`Log`** faz marshalling para a UI thread com `BeginInvoke` quando `InvokeRequired`.

## Relacionado

- [[../Arquivos-Chave/ZimerfeldLfsPlugin]]
- [[../Arquivos-Chave/LfsForm]]
- [[../Arquivos-Chave/LfsService]]
- [[../Decisoes/Janela dedicada não-modal]]
- [[../Decisoes/Diretório de trabalho independente]]
- [[Dependências]]
