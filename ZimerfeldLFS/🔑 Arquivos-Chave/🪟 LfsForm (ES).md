---
tipo: arquivo-chave
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [arquivo, winforms, ui, janela, abas, i18n, threading]
arquivo: src/GitExtensions.ZimerfeldLFS/LfsForm.cs
---

# 🪟 LfsForm.cs

La ventana principal — no modal — que conduce el flujo de LFS en tres etapas.

**Ruta:** `src/GitExtensions.ZimerfeldLFS/LfsForm.cs`

---

## 📜 Declaración y propiedades de la ventana

```csharp
public sealed class LfsForm : Form
```

| Propiedad | Valor |
|---|---|
| `FormBorderStyle` | `FixedSingle` (no redimensionable) |
| `MaximizeBox` | `false` (con `MinimizeBox = true`) |
| `StartPosition` | `CenterScreen` |
| `Font` | Segoe UI 9 |
| `Icon` | `PluginIcon.ForForm()` |
| `Size` | `720 × (720 + SponsorBanner.PanelHeight)` |

**Constructor** `LfsForm(workingDir, notifyRepoChanged?, openCommitDialog?, openPushDialog?)`: crea el `LfsService`, guarda los delegates, arma la UI (`InitializeComponent`) y puebla el `cboRepo` (`LoadRepositories`, solo lectura de XML). **Ningún trabajo de git en el constructor** → ventana instantánea; el primer sondeo se ejecuta en `Shown` vía `RefreshStateAsync` en un hilo de fondo.

---

## 🖼️ Diseño (arriba → abajo)

1. **SponsorBanner** (`DockStyle.Top`, en primer plano) — badges de GitHub Sponsors + Ko-fi clicables + enlace "Acerca de" a la derecha (`SponsorBanner.cs`).
2. **Panel superior** — etiqueta "Directorio de Trabajo", **`cboRepo`** (dropdown de repositorio independiente) y etiqueta de branch.
3. **TabControl** (`DockStyle.Fill`) — 3 pestañas: `tabInstall`, `tabWorkflow`, `tabClone`.
4. **Panel de log** (`DockStyle.Bottom`, altura 150) — cabecera ("Salida:" + botón Limpiar) y `txtLog` (multilínea, solo lectura, fondo `#1E1E1E`, fuente Consolas).
5. **Panel inferior** — checkbox **Mostrar Debug**, etiqueta + dropdown **Idioma**, botón **Cerrar** (también `CancelButton`).
6. **StatusStrip** — etiqueta de estado (`NoGripRenderer` elimina el grip de redimensionamiento).

---

## 🗂️ Las 3 pestañas

### `tabInstall` (Etapa 1 · Instalación)
- `lblInstallStatus` (estado con color: rojo = ausente, dorado = disponible, verde = listo).
- Botones: **Verificar instalación** → `RunAsync(..., () => _svc.GetLfsVersion())`; **`git lfs install`** → `RunAsync("git lfs install", () => _svc.LfsInstall())`.
- `lblInstallHelp` con el texto de ayuda (Homebrew/Chocolatey/binarios). Ver [[⚙️ Instalação (ES)|⚙️ Instalación]].

### `tabWorkflow` (Etapa 2 · Flujo básico)
- `txtPattern` (con placeholder) + botón **Track** (`DoTrack`; Enter también lo dispara).
- `lstPatterns` (patrones rastreados) + botón **Untrack** (`DoUntrack`).
- `lstLfsFiles` (archivos LFS, `ls-files`) con scroll horizontal.
- `commitRow` (base): **Commit…** (`DoCommit`) y **Push…** (`DoPush`). Ver [[📤 Track Commit Push (ES)|📤 Track Commit Push]].

### `tabClone` (Etapa 3 · Clone & Pull)
- `lblCloneHelp` + 4 botones: **`git lfs pull`**, **`git lfs fetch --all`**, **`git lfs checkout`**, **`git lfs status`** (`status` se ejecuta con `refreshAfter:false`). Ver [[⬇️ Clone e Pull (ES)|⬇️ Clone y Pull]].

---

## 📂 Directorio de trabajo independiente

`LoadRepositories()` llama a `LfsService.GetRepositoriesFromSettings()` (lee el historial de GitExtensions del XML de settings), inserta el working dir inicial en la parte superior si no está en la lista, y lo preselecciona. `CboRepo_SelectedIndexChanged` cambia `_svc.WorkingDir` y dispara `RefreshStateAsync`. `UpdateWorkingDir(newDir)` (llamado por el plugin cuando la ventana ya existe) agrega/selecciona el dir. Ver [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de Trabajo Independiente]].

---

## 🪟 Acciones que abren diálogos nativos

- **`DoCommit`** — llama a `_openCommitDialog?.Invoke(this, dir)`; registra "completado / cerrado / no disponible" según el `bool?`; llama a `notifyRepoChanged` y `RefreshStateAsync`.
- **`DoPush`** — llama a `_openPushDialog`; si `true`, registra y refresca el estado; en caso contrario cae al **fallback** `git push` puro vía `RunAsync`.

`DoTrack`/`DoUntrack` ejecutan `git lfs track/untrack "<glob>"` y, si tienen éxito, **stagean el `.gitattributes`** (`_svc.Add(".gitattributes")`).

---

## 🧵 Runner y refresh (threading)

### `RunAsync(title, op, refreshAfter = true)`
Ignora si `_busy` o si `!EnsureRepoSelected()`. Marca busy, registra `$ <title>`, ejecuta `await Task.Run(op)`, registra `Combined` + ✓/✗ (con `ExitCode`), desmarca busy y (opcionalmente) `RefreshStateAsync`.

### `RefreshStateAsync()`
Sondea en un `Task.Run`: `IsGitRepo`, branch, versión de lfs, disponible, inicializado, patrones, archivos → `StateSnapshot` → `ApplyState` (actualiza etiquetas, listas y habilita/deshabilita botones). El estado pasa a "Actualizando…" durante el sondeo.

### `Log(message)`
Añade al `txtLog` con auto-scroll; hace `BeginInvoke` si `InvokeRequired`.

---

## 🌐 Localización y Debug

- **Idioma:** dropdown Automático/Inglés/Portugués/Español → `I18n.SetLanguage` → `ApplyLanguage()` recarga el `Translator` y reescribe todos los textos. `_suppressLangEvent` evita la reentrancia al repoblar el combo.
- **Mostrar Debug:** `ApplyControlTooltips(show)` recorre el árbol y establece el tooltip = `Name` de cada control. Persistido en `%APPDATA%\GitExtensions\ZimerfeldLFS.uisettings.json` (`{"showControlIds":…}`).
- **Acerca de:** `LinkLabel` en el banner → `MessageBox` con el texto localizado (`aboutText`).

### 💾 Archivos de settings persistidos
| Archivo (`%APPDATA%\GitExtensions\`) | Contenido |
|---|---|
| `ZimerfeldLFS.language.json` | idioma elegido (`I18n`) |
| `ZimerfeldLFS.uisettings.json` | `showControlIds` (checkbox Debug) |
| `ZimerfeldLFS.debug.log` | log de diagnóstico del plugin |

---

## 🔗 Relacionado

- [[🔌 ZimerfeldLfsPlugin (ES)|🔌 ZimerfeldLfsPlugin]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
- [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de Trabajo Independiente]]
- [[🗂️ Fluxo em 3 etapas (abas) (ES)|🗂️ Flujo en 3 etapas (pestañas)]]
- [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]]
