---
tipo: negocio
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
tags: [projeto, negocio, csharp, gitextensions, plugin, winforms, git-lfs, i18n]
status: ativo
linguagem: C#
versao: 1.0.4
repo: C:\GitExtensions\GitExtensions.ZimerfeldLFS
---

# 📦 GitExtensions.ZimerfeldLFS

> 🇧🇷 Lee esta página en portugués → [[📦 GitExtensions.ZimerfeldLFS]]

## 🎯 Objetivo
Plugin para **[GitExtensions](https://gitextensions.github.io/)** que gestiona el **Git Large File Storage (LFS)** en una **ventana dedicada y no modal**. Git LFS sustituye los archivos grandes (audio, video, datasets) por **punteros de texto ligeros** dentro del repositorio; el contenido real queda en un servidor remoto separado — **acelerando el clonado y evitando el hinchamiento del repositorio**. La ventana guía al usuario por el flujo estándar de LFS en **tres etapas** (pestañas) y apunta a un **directorio de trabajo elegido de forma independiente de GitExtensions**. Ver [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]].

## 💜 Financiación / Patrocinio
Canales de donación configurados (badges en la parte superior del README + **banner clicable en la parte superior de la ventana**, ver [[🪟 LfsForm (ES)|🪟 LfsForm]] / `SponsorBanner.cs`):
- **GitHub Sponsors:** `@zimerfeld` → https://github.com/sponsors/zimerfeld
- **Ko-fi:** `C0D621FCGD` → https://ko-fi.com/C0D621FCGD
- **Botón nativo "Sponsor this project" en GitHub:** activado por `.github/FUNDING.yml` (`github: zimerfeld` + `ko_fi: C0D621FCGD`) — mismo patrón que `GitExtensions.ZimerfeldCommitMsg`. Muestra el botón en la parte superior de la página del repositorio.
- **Badges de patrocinio en todos los READMEs:** frase de invitación bilingüe + badges de GitHub Sponsor/Ko-fi presentes en `README.md`, `README.en-US.md` y `README.pt-BR.md`.
- **Prueba social en el README:** badges de versión y **descargas de NuGet** (`shields.io/nuget/v` y `/dt`).

## 📂 Estructura del repositorio
```
C:\GitExtensions\GitExtensions.ZimerfeldLFS\
├─ src\GitExtensions.ZimerfeldLFS\        # código del plugin (.csproj)
│   ├─ ZimerfeldLfsPlugin.cs             # entry point MEF (Execute/Register/Unregister)
│   ├─ LfsForm.cs                        # la ventana: 3 pestañas + dropdown de repo + log + i18n
│   ├─ LfsService.cs                     # runner de git / git lfs (subprocesos)
│   ├─ Localization.cs                   # I18n + Translator (diccionarios JSON embebidos)
│   ├─ SponsorBanner.cs                  # banner GitHub Sponsors + Ko-fi (parte superior de la ventana)
│   ├─ PluginIcon.cs                     # carga el ícono embebido (ico.png)
│   ├─ Resources\                        # ico.png, icon-128.png, badges, ZimerfeldLFS.<culture>.json
│   ├─ *.csproj / *.nuspec               # build + manifiesto NuGet
├─ refs\                                 # DLLs del host versionadas (build determinista)
├─ tools\                                # install/uninstall .ps1, nuget.exe, icon-generator
│   ├─ icon-generator\Generate-LfsIcon.ps1  # generador GDI+ del ícono
│   └─ net9.0-windows\                   # salida del build (DLL) usada por el nupkg
├─ ZimerfeldLFS\                         # 🧠 este cofre de memoria
├─ build.ps1                             # incrementa versión + build + deploy + nupkg
├─ README.md / README.pt-BR.md / README.en-US.md  # reflejado en [[📖 README — Instalação, Uso e Build (ES)|📖 README — Instalación, Uso y Build]]
└─ GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg
```

## ⚙️ Stack técnica
- **Lenguaje:** C# (`net9.0-windows`), `Nullable` + `ImplicitUsings` habilitados, `LangVersion=latest`
- **UI:** WinForms (`UseWindowsForms`) — ventana propia (`LfsForm`), no usa pantallas del host más allá de los diálogos nativos de commit/push
- **Tipo de salida:** `Library` (DLL cargada por GitExtensions)
- **AssemblyName:** `GitExtensions.Plugins.ZimerfeldLFS`
- **Namespace raíz:** `GitExtensions.ZimerfeldLFS`
- **NeutralLanguage:** `pt-BR`
- **Plugin model:** MEF (`[Export(typeof(IGitPlugin))]`) — ver [[🧩 Plugin MEF para GitExtensions (ES)|🧩 Plugin MEF para GitExtensions]]
- **Referencias externas** (de `refs\`, `Private=false`): `GitExtensions.Extensibility.dll`, `System.ComponentModel.Composition.dll`

## ✨ Funcionalidades principales
- **Flujo guiado en tres etapas (pestañas):** ver [[⚙️ Instalação (ES)|⚙️ Instalación]], [[📤 Track Commit Push (ES)|📤 Track Commit Push]], [[⬇️ Clone e Pull (ES)|⬇️ Clone y Pull]].
  1. **Instalación** — `git lfs version` (detectar) + `git lfs install` (inicializar para el usuario); la ayuda menciona Homebrew/Chocolatey/binarios oficiales.
  2. **Flujo básico** — rastrear patrones glob (`git lfs track "*.psd"`) y **stagear el `.gitattributes`**; listar patrones rastreados (+ untrack); listar archivos LFS (`git lfs ls-files`); **Commit** y **Push** vía los diálogos nativos del host.
  3. **Clone & Pull** — `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`.
- **Directorio de trabajo independiente:** dropdown `cboRepo` poblado a partir del **historial de repositorios de GitExtensions** (leído del archivo de configuración), sin depender de la ventana del host. Ver [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de trabajo independiente]].
- **Consola de salida (log):** cada botón muestra el **comando `git`/`git lfs` exacto** y su salida en una consola oscura — nada queda oculto.
- **Estado del LFS en vivo:** versión detectada, si está inicializado para el usuario, patrones rastreados y archivos LFS — actualizados automáticamente después de cada operación y al cambiar de repositorio.
- **Localización (PT-BR / EN-US / ES-ES):** modo automático siguiendo el SO + override manual (dropdown Automático/Portugués/Inglés/Español). Ver [[Localization.cs|I18n]].
- **Mostrar Debug:** alternancia que muestra el `Name` de cada control como tooltip (ayuda de desarrollo heredada de ZimerfeldTree).

## 🏗️ Arquitectura (Plugin → Form → Service)
Tres clases, cada una con una responsabilidad única (ver [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]]):
```
GitExtensions (host)
    │ MEF
    ▼
ZimerfeldLfsPlugin   ← [Export(IGitPlugin)], base(false) = sin diálogo de configuración
    │ abre singleton, pasa delegates openCommit/openPush (WithWorkingDirectory)
    ▼
LfsForm (la ventana)  ── usa ──►  LfsService (subprocesos git / git lfs → GitResult)
```
El plugin **no se suscribe a ningún evento del host** — está totalmente desacoplado. El directorio de trabajo del host se usa **solo una vez**, como valor preseleccionado de `cboRepo` al abrir la ventana.

## 🛠️ Build / instalación
```powershell
# Build: incrementa build, compila Release, hace deploy (Admin), genera nupkg
.\build.ps1
.\build.ps1 -Force   # siempre recompila/empaqueta

# Scripts auxiliares en tools\ (Admin para Program Files)
tools\install.ps1      # instala el plugin (también vía Package Manager Console)
tools\uninstall.ps1    # elimina (no afecta al resto de GitExtensions)
```
> Vía el **Plugin Manager de GitExtensions**: buscar *ZimerfeldLFS* e instalar. Paso a paso en [[📖 README — Instalação, Uso e Build (ES)|📖 README — Instalación, Uso y Build]] y [[🔢 Versionamento (ES)|🔢 Versionado]].

## 🎨 Ícono
Área dividida en **4 cuadrantes iguales**, cada uno una "tarjeta de archivo" (file card) de un tipo de archivo grande: **joystick de arcade** (azul), **nota musical** con doble barra para audio (morado), **botón de play** de película (rojo) y **cilindros de base de datos** (verde azulado); en el centro, una pequeña **bomba con mecha encendida** (estilo Super Mario Bros). Generado 100% vía GDI+ en `Generate-LfsIcon.ps1` → `icon-128.png` (paquete/ventana) e `ico.png` (16×16, menú). Ver [[🎨 Generate-LfsIcon (ES)|🎨 Generate-LfsIcon]] y [[💣 Ícone 4 quadrantes + bomba (ES)|💣 Ícono: 4 cuadrantes + bomba]].

## 💰 Ángulo de inversión
- **Nicho desatendido:** GitExtensions no tiene una pantalla de LFS guiada; los usuarios caen en la línea de comandos. Este plugin transforma un flujo intimidante (`track` / `.gitattributes` / `ls-files` / `pull`/`fetch`/`checkout`) en una experiencia de 3 clics.
- **Público real:** equipos de juegos, diseño, audio/video, data science — exactamente quienes sufren con repositorios hinchados y son candidatos a adoptar LFS.
- **Costo marginal bajo:** comparte la infraestructura (build, i18n, banner, ícono GDI+, refs versionados) con los hermanos [[GitExtensions.ZimerfeldTree]] y [[GitExtensions.ZimerfeldCommitMsg]] — un portafolio cohesivo refuerza la marca **Zimerfeld** en el ecosistema GitExtensions/NuGet.
- **Distribución lista:** publicado en NuGet y visible en el Plugin Manager interno (dependencia marcadora `GitExtensions.Extensibility`).

## 🐛 Trampas conocidas
> [!warning] `git` y `git-lfs` deben estar en el PATH
> `LfsService` ejecuta `git`/`git lfs` como subprocesos. Si no están en el `PATH`, los comandos devuelven un `GitResult` con `ExitCode -1` y la pestaña de Instalación muestra "Git LFS NO está instalado". No es un crash — se muestra como un resultado fallido.

<!-- -->

> [!warning] DLL en la RAÍZ de `lib\` en el nuspec (aviso NU5101 intencional)
> El Plugin Manager de GitExtensions solo extrae un grupo `lib` cuyo target framework esté en la lista de monikers (incluye `any`). La DLL va en la **raíz** de `lib\` (grupo "any"); una subcarpeta `lib\net9.0-windows\` **no** sería extraída. Por eso el aviso **NU5101** de `nuget pack` se **filtra a propósito** en `build.ps1`. Ver [[🔢 Versionamento (ES)|🔢 Versionado]] y [[🧱 Dependências (ES)|🧱 Dependencias]].

## 🔢 Versionado
- Versión actual: **1.0.4** (csproj + nuspec sincronizados por `build.ps1`)
- Esquema: `major.minor.BUILD`, BUILD autoincrementado en cada build
- En cada build, `build.ps1` sella versión + fecha en los **READMEs** (sección 4b) **y** en este cofre de Obsidian (sección 4c), manteniendo todo sincronizado

## 🔗 Relacionado
- [[📖 README — Instalação, Uso e Build (ES)|📖 README — Instalación, Uso y Build]] — espejo del `README.md`
- [[🧩 Plugin MEF para GitExtensions (ES)|🧩 Plugin MEF para GitExtensions]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
- [[🏛️ Arquitetura (ES)|🏛️ Arquitectura]] · [[🔭 Visão Geral (ES)|🔭 Visión general]] · [[🔢 Versionamento (ES)|🔢 Versionado]] · [[🧱 Dependências (ES)|🧱 Dependencias]]
- [[GitExtensions.ZimerfeldTree]] — hermano (árbol de branches)
- [[GitExtensions.ZimerfeldCommitMsg]] — hermano (mensajes de commit)
- [[🔑 Fatos-Chave (ES)|🔑 Datos clave]]
