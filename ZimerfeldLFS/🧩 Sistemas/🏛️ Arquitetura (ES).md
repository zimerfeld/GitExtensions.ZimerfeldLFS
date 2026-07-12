---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [arquitetura, classes, design, i18n, threading]
---

# 🏛️ Arquitectura

## 🗺️ Diagrama de clases

```
GitExtensions (host)
    │
    │  MEF (System.ComponentModel.Composition)
    ▼
ZimerfeldLfsPlugin        ← [Export(IGitPlugin)] : GitPluginBase, base(false)
    │  Execute()  → abre/trae al frente la ventana singleton
    │  captura _commands (Register/Unregister)
    │  pasa delegates: openCommit / openPush  (via WithWorkingDirectory)
    ▼
LfsForm (la ventana)      ← FixedSingle, no modal, 3 pestañas + log + i18n
    │  cboRepo (dropdown independiente)  →  WorkingDir
    │  RunAsync(title, op)  →  Task.Run
    ▼
LfsService                ← subprocesos git / git lfs
    │  RunGit(args) → GitResult(StdOut, StdErr, ExitCode)
    ▼
git / git lfs (PATH)
```

## 🧱 Las tres clases

### 🔌 `ZimerfeldLfsPlugin` — punto de entrada
Hereda de `GitPluginBase`, exportado vía MEF. Constructor `: base(false)` → **sin** diálogo de configuración en Configuración → Plugins. Ver [[🔌 ZimerfeldLfsPlugin (ES)|🔌 ZimerfeldLfsPlugin]].
- **`Execute`** — abre (o trae al frente) la **ventana singleton** `LfsForm`; pasa los delegates `openCommit`/`openPush` que abren los diálogos nativos del host **en el directorio de trabajo seleccionado en `cboRepo`** (`_commands.WithWorkingDirectory(dir)` → `StartCommitDialog`/`StartPushDialog`). Devuelve `false` (el host **no** debe actualizar su propia UI).
- **`Register`/`Unregister`** — capturan/limpian `_commands` (`IGitUICommands`).

### 🪟 `LfsForm` — la ventana
WinForms `FixedSingle`, no maximizable, `CenterScreen`, Segoe UI 9. De arriba a abajo: banner de patrocinio, dropdown de repositorio independiente + branch, `TabControl` de 3 pestañas, consola de log oscura, panel inferior (Debug + Idioma + Cerrar), barra de estado. Ver [[🪟 LfsForm (ES)|🪟 LfsForm]].

### ⚡ `LfsService` — ejecutor git/git-lfs
`RunGit(args)` ejecuta `git` en un `ProcessStartInfo` (redirigiendo stdout/stderr, UTF-8, sin ventana) y devuelve un `GitResult(StdOut, StdErr, ExitCode)` (record struct; `Ok` = exit 0; `Combined` = stdout+stderr recortado). Métodos de estado, de las 3 etapas, y el helper estático `GetRepositoriesFromSettings`. Ver [[⚡ LfsService (ES)|⚡ LfsService]].

## 🔗 Desacoplamiento del host

> [!important] El plugin no se suscribe a NINGÚN evento del host
> A diferencia de su hermano CommitMsg (que escucha `PostRepositoryChanged`), ZimerfeldLFS está **totalmente desacoplado**: la ventana elige el repositorio **exclusivamente** mediante su propio `cboRepo`. El directorio de trabajo del host se usa **una única vez**, como valor preseleccionado al abrir la ventana. `Register` solo guarda `_commands` para poder abrir los diálogos nativos; no hay revinculación ni `Application.Idle`. Ver [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]].

El único punto de contacto con el host es opcional: los delegates `openCommit`/`openPush`, que resuelven `IGitUICommands.WithWorkingDirectory(dir)` para abrir los diálogos nativos en el repositorio elegido. Si no están disponibles, el log registra el fallback (el Push cae en un `git push` puro).

## 🌐 Localización (i18n)

`I18n` (estático) resuelve el idioma (`Automatic` sigue el SO vía `CultureInfo.CurrentUICulture`; si no, `en-US`/`pt-BR`/`es-ES`) y carga el diccionario desde el **JSON embebido** `Resources\ZimerfeldLFS.<culture>.json` vía `GetManifestResourceStream`, con fallback a en-US → mapa vacío. Un `Translator` devuelve la cadena por clave (o la propia clave si falta) y tiene `F(key, args)` para `string.Format`. La elección se lee del disco **una vez** al iniciar y luego se maneja en memoria mediante el dropdown; se persiste en `%APPDATA%\GitExtensions\ZimerfeldLFS.language.json`. Ver [[🪟 LfsForm (ES)|🪟 LfsForm]] y `Localization.cs`.

## 🧵 Threading

> La ventana **se abre instantáneamente**: el constructor no realiza **ningún trabajo git** (solo completa el `cboRepo` leyendo el XML de configuración). El primer sondeo se ejecuta tras el evento `Shown`.

- **`RefreshStateAsync`** — sondea el repositorio en un `Task.Run` en segundo plano (versión de lfs, ¿disponible?, ¿inicializado?, branch, patrones, archivos → `StateSnapshot`) y aplica el resultado a la UI (`ApplyState`). Se llama en `Shown`, al cambiar de repositorio en `cboRepo`, y después de las operaciones.
- **`RunAsync(title, op, refreshAfter)`** — ejecuta una operación git/git-lfs en `Task.Run`, registra el comando y la salida, y (opcionalmente) llama a `RefreshStateAsync`. Las llamadas concurrentes se ignoran mientras `_busy`.
- **`Log`** realiza el marshalling al hilo de la UI con `BeginInvoke` cuando `InvokeRequired`.

## 🔗 Relacionado

- [[🔌 ZimerfeldLfsPlugin (ES)|🔌 ZimerfeldLfsPlugin]]
- [[🪟 LfsForm (ES)|🪟 LfsForm]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
- [[🪟 Janela dedicada não-modal (ES)|🪟 Ventana dedicada no modal]]
- [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]]
- [[🧱 Dependências (ES)|🧱 Dependencias]]
