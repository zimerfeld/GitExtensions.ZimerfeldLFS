---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [arquivo, plugin, entry-point, mef, winforms]
arquivo: src/GitExtensions.ZimerfeldLFS/ZimerfeldLfsPlugin.cs
---

# 🔌 ZimerfeldLfsPlugin.cs

Punto de entrada del plugin. Exportado vía MEF a GitExtensions.

**Ruta:** `src/GitExtensions.ZimerfeldLFS/ZimerfeldLfsPlugin.cs`

---

## 📜 Declaración

```csharp
[Export(typeof(IGitPlugin))]
public sealed class ZimerfeldLfsPlugin : GitPluginBase
```

El atributo `[Export]` es el punto de descubrimiento por el MEF del host. Ver [[🧩 Plugin MEF para GitExtensions (ES)|🧩 Plugin MEF para GitExtensions]].

---

## 🏗️ Constructor — `: base(false)`

`base(false)` = el plugin **no tiene** settings configurables en el diálogo de GitExtensions (no aparece nodo en Configuración → Plugins). Define:
- `Name = "ZimerfeldLFS"`
- `Description` — texto bilingüe explicando LFS y los 3 pasos
- `Icon = PluginIcon.ForMenu()` — ícono 16×16 embebido

---

## 🧾 Campos de instancia

| Campo | Tipo | Propósito |
|---|---|---|
| `_form` | `LfsForm?` | Ventana **singleton** — una por sesión de GitExtensions |
| `_commands` | `IGitUICommands?` | Commands actual; actualizado por `Register`/`Unregister`. Se usa para abrir los diálogos nativos en el repo del `cboRepo` |

---

## ⚙️ Métodos (IGitPlugin)

### `Execute(GitUIEventArgs)` ← menú Plugins → ZimerfeldLFS
- Lee `args.GitModule?.WorkingDir` (solo como valor inicial).
- Si la ventana no existe (o fue descartada), **crea** `LfsForm(workDir, notifyChanged, openCommit, openPush)` y se suscribe a `FormClosed` para resetear `_form`. En caso contrario, llama a `UpdateWorkingDir(workDir)`.
- `Show()` + `BringToFront()`.
- **Devuelve `false`** — el host **no** debe actualizar su propia UI; la ventana gestiona su propio estado.

**Delegates pasados a la ventana:**
- `notifyChanged` = `() => args.GitUICommands?.RepoChangedNotifier?.Notify()` — pide al host actualizar tras commit/checkout.
- `openCommit(owner, workingDir)` → resuelve `_commands.WithWorkingDirectory(workingDir)` y llama a `StartCommitDialog(owner, "", false)`. Devuelve `bool?` (`null` = no disponible). Envuelto en `try/catch`.
- `openPush(owner, workingDir)` → `WithWorkingDirectory` + `StartPushDialog(owner, pushOnShow:true, forceWithLease:false, out pushCompleted)`. Devuelve `pushCompleted`. Envuelto en `try/catch`.

### `Register(IGitUICommands)`
- `base.Register(commands)` + captura `_commands = commands`.
- **No se suscribe a ningún evento del host.** El comentario en el código es explícito: el repositorio de la ventana viene **exclusivamente** del `cboRepo`; el working dir del host se usa solo como valor preseleccionado al abrir. Ver [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]].

### `Unregister(IGitUICommands)`
- `_commands = null` + `base.Unregister(commands)`.

---

## 🩺 Logging de diagnóstico

Cada evento de ciclo de vida (`Execute`/`Register`/`Unregister`) graba una línea con timestamp y un `_instanceId` incremental en `%APPDATA%\GitExtensions\ZimerfeldLFS.debug.log`. Best-effort, envuelto en `try/catch` — **nunca** tumba el plugin.

---

## 🛡️ Protección contra crash

Todos los delegates y el log están envueltos en `try/catch` — las excepciones en el plugin nunca tumban GitExtensions.

---

## 🔗 Relacionado

- [[🪟 LfsForm (ES)|🪟 LfsForm]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
- [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]]
- [[🪟 Janela dedicada não-modal (ES)|🪟 Ventana dedicada no modal]]
- [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]]
