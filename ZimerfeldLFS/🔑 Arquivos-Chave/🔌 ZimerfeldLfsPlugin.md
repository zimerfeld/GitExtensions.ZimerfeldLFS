---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: pt-BR
atualizado: 2026-07-04
tags: [arquivo, plugin, entry-point, mef, winforms]
arquivo: src/GitExtensions.ZimerfeldLFS/ZimerfeldLfsPlugin.cs
---

# 🔌 ZimerfeldLfsPlugin.cs

Ponto de entrada do plugin. Exportado via MEF para o GitExtensions.

**Caminho:** `src/GitExtensions.ZimerfeldLFS/ZimerfeldLfsPlugin.cs`

---

## 📜 Declaração

```csharp
[Export(typeof(IGitPlugin))]
public sealed class ZimerfeldLfsPlugin : GitPluginBase
```

O atributo `[Export]` é o ponto de descoberta pelo MEF do host. Ver [[🧩 Plugin MEF para GitExtensions]].

---

## 🏗️ Construtor — `: base(false)`

`base(false)` = o plugin **não tem** settings configuráveis no diálogo do GitExtensions (não aparece nó em Configurações → Plugins). Define:
- `Name = "ZimerfeldLFS"`
- `Description` — texto bilíngue explicando LFS e as 3 etapas
- `Icon = PluginIcon.ForMenu()` — ícone 16×16 embutido

---

## 🧾 Campos de instância

| Campo | Tipo | Propósito |
|---|---|---|
| `_form` | `LfsForm?` | Janela **singleton** — uma por sessão do GitExtensions |
| `_commands` | `IGitUICommands?` | Commands atual; atualizado por `Register`/`Unregister`. Usado para abrir os diálogos nativos no repo do `cboRepo` |

---

## ⚙️ Métodos (IGitPlugin)

### `Execute(GitUIEventArgs)` ← menu Plugins → ZimerfeldLFS
- Lê `args.GitModule?.WorkingDir` (só como valor inicial).
- Se a janela não existe (ou foi descartada), **cria** `LfsForm(workDir, notifyChanged, openCommit, openPush)` e assina `FormClosed` para zerar `_form`. Senão, chama `UpdateWorkingDir(workDir)`.
- `Show()` + `BringToFront()`.
- **Retorna `false`** — o host **não** deve atualizar a própria UI; a janela gerencia o próprio estado.

**Delegates passados à janela:**
- `notifyChanged` = `() => args.GitUICommands?.RepoChangedNotifier?.Notify()` — pede ao host para atualizar após commit/checkout.
- `openCommit(owner, workingDir)` → resolve `_commands.WithWorkingDirectory(workingDir)` e chama `StartCommitDialog(owner, "", false)`. Retorna `bool?` (`null` = indisponível). Em `try/catch`.
- `openPush(owner, workingDir)` → `WithWorkingDirectory` + `StartPushDialog(owner, pushOnShow:true, forceWithLease:false, out pushCompleted)`. Retorna `pushCompleted`. Em `try/catch`.

### `Register(IGitUICommands)`
- `base.Register(commands)` + captura `_commands = commands`.
- **Não assina nenhum evento do host.** O comentário no código é explícito: o repositório da janela vem **exclusivamente** do `cboRepo`; o working dir do host é usado só como valor pré-selecionado ao abrir. Ver [[📁 Diretório de trabalho independente]].

### `Unregister(IGitUICommands)`
- `_commands = null` + `base.Unregister(commands)`.

---

## 🩺 Logging diagnóstico

Cada evento de ciclo de vida (`Execute`/`Register`/`Unregister`) grava uma linha com timestamp e um `_instanceId` incremental em `%APPDATA%\GitExtensions\ZimerfeldLFS.debug.log`. Best-effort, envolto em `try/catch` — **nunca** derruba o plugin.

---

## 🛡️ Proteção contra crash

Todos os delegates e o log são envoltos em `try/catch` — exceções no plugin nunca derrubam o GitExtensions.

---

## 🔗 Relacionado

- [[🪟 LfsForm]]
- [[⚡ LfsService]]
- [[🏛️ Arquitetura]]
- [[🪟 Janela dedicada não-modal]]
- [[📁 Diretório de trabalho independente]]
