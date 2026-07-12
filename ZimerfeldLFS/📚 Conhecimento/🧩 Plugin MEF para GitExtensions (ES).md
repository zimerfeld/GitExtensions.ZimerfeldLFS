---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, csharp, gitextensions, mef, plugin]
---

# 🧩 Plugin MEF para GitExtensions

## Resumen
GitExtensions carga plugins vía **MEF** (Managed Extensibility Framework). El entry point es una clase exportada que implementa `IGitPlugin` (normalmente heredando de `GitPluginBase`).

## Puntos clave
- Exportar con `[Export(typeof(IGitPlugin))]` usando `System.ComponentModel.Composition`.
- El proyecto compila como **`Library`** (DLL), `net9.0-windows`, con WinForms habilitado.
- Referenciar los assemblies del host con **`<Private>false</Private>`** (no copiar a la salida — el host ya los tiene). En ZimerfeldLFS quedan **versionados en `refs\`** (build determinístico y offline):
  - `GitExtensions.Extensibility.dll`
  - `System.ComponentModel.Composition.dll`
- El `AssemblyName` debe coincidir con lo que `install.ps1` / el nuspec esperan (`GitExtensions.Plugins.<Nombre>`).
- Para aparecer en el **Plugin Manager** interno, el paquete NuGet debe **depender** de `GitExtensions.Extensibility` (dependencia marcadora). Ver [[🧱 Dependências (ES)|🧱 Dependencias]].

## Ciclo de vida del plugin
- `Register(IGitUICommands)` — llamado al cargar. Buen lugar para **capturar el `IGitUICommands`** (usado después para abrir diálogos nativos) y, si se desea, suscribirse a eventos (`PostRepositoryChanged`).
- `Unregister(IGitUICommands)` — desuscribir eventos / limpiar el commands capturado.
- `Execute(GitUIEventArgs)` — disparado por el menú **Plugins → \<nombre\>**. Acceso a `GitModule.WorkingDir`, `StartCommitDialog`, `StartPushDialog`, etc.

## Cómo ZimerfeldLFS usa este modelo
- `: base(false)` en el constructor → **sin** diálogo de settings (no implementa `GetSettings()`).
- `Execute` abre una **ventana no modal singleton** (`LfsForm`) en lugar de un diálogo modal, y devuelve `false` (el host no actualiza su propia UI). Ver [[🔌 ZimerfeldLfsPlugin (ES)|🔌 ZimerfeldLfsPlugin]] y [[🪟 Janela dedicada não-modal (ES)|🪟 Ventana dedicada no modal]].
- **Desacoplado:** `Register` solo **captura** el `IGitUICommands`; el plugin **no se suscribe a eventos del host**. El repositorio objetivo viene del propio desplegable de la ventana. Ver [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]].
- Los diálogos nativos de commit/push se abren vía `IGitUICommands.WithWorkingDirectory(dir)` → `StartCommitDialog` / `StartPushDialog`, en el repo seleccionado.

## 🔗 Relacionado
- [[📦 GitExtensions.ZimerfeldLFS (ES)|📦 GitExtensions.ZimerfeldLFS]]
- [[🔌 ZimerfeldLfsPlugin (ES)|🔌 ZimerfeldLfsPlugin]]
- [[GitExtensions.ZimerfeldTree]]
- [[GitExtensions.ZimerfeldCommitMsg]]
