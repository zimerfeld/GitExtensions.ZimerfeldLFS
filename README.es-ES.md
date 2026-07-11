# GitExtensions.ZimerfeldLFS — Guía Técnica de Operación (Español)

![Icono](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/src/GitExtensions.ZimerfeldLFS/Resources/icon-128.png)

**Versión:** 1.0.4 — **Actualizado:** 2026-07-04

Este plugin se construye y mantiene en mi tiempo libre. Si te ahorra tiempo gestionando Git LFS, un patrocinio ayuda a mantenerlo actualizado para las nuevas versiones de GitExtensions. 💜

[![GitHub Sponsor](https://img.shields.io/badge/Sponsor-zimerfeld-EA4AAA?style=for-the-badge&logo=githubsponsors&logoColor=white)](https://github.com/sponsors/zimerfeld) &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [![Ko-fi](https://img.shields.io/badge/Ko--fi-Buy%20me%20a%20coffee-FF5E2B?style=for-the-badge&logo=ko-fi&logoColor=white)](https://ko-fi.com/C0D621FCGD)

> Este documento es el manual detallado, paso a paso, para **operar** la ventana del plugin.
> Para una visión general de alto nivel consulta el [README principal](README.md) · Versión en inglés: [README.en-US.md](README.en-US.md) · Versión en portugués: [README.pt-BR.md](README.pt-BR.md).

---

## ⚡ Resumen ejecutivo

- **Qué es:** un plugin de GitExtensions (MEF) que expone **Git LFS** en una **ventana dedicada y no modal**, guiándote a través de un **flujo de 3 pasos** — *Instalación* → *Track/Commit/Push* → *Clone/Pull*.
- **Problema que resuelve:** Git LFS es potente pero depende mucho de la línea de comandos y es fácil de configurar mal (`git lfs install`, `track`, `.gitattributes`). El plugin convierte ese flujo en clics, con un **log visible** de cada comando `git`/`git lfs` que ejecuta.
- **Diferenciadores:** ventana persistente (nunca interrumpe tu trabajo en el host); **directorio de trabajo independiente** del repositorio activo de GitExtensions; **i18n** (Automático / EN-US / PT-BR / ES-ES); icono personalizado; banner de patrocinio (GitHub Sponsors + Ko-fi).
- **Stack:** C# / WinForms `Library`, destino **net9.0-windows**, empaquetado como **nupkg**; build y versionado automatizados mediante `build.ps1`.
- **Estado actual:** versión **`1.0.2`** — funcional, con **36 pruebas unitarias (xUnit)** que cubren `LfsService`.
- **Público objetivo:** desarrolladores y equipos que versionan activos grandes (juegos, medios, datasets de ML) y ya usan GitExtensions en Windows.

---

## Anatomía de la ventana

Al abrir **Plugins → ZimerfeldLFS**, aparece una ventana no modal de tamaño fijo (720 px de ancho) con las siguientes regiones, de arriba hacia abajo:

| Región | Qué hace |
| --- | --- |
| **Banner de patrocinio** | Enlaces a GitHub Sponsors / Ko-fi y al diálogo *Acerca de*. |
| Menú desplegable **Working Directory** | Selecciona el repositorio contra el que se ejecuta cada comando — elegido de forma **independiente** del host de GitExtensions. |
| Etiqueta **Branch** | Muestra la rama actualmente activa del repositorio seleccionado (o *not a git repository*). |
| **Pestañas** | El flujo de trabajo en tres pasos: *Instalación*, *Flujo básico*, *Clonado y pull*. |
| Consola **Output** | Un log oscuro, de solo lectura y monoespaciado que refleja cada comando y su salida bruta. |
| **Barra inferior** | Interruptor *Show Debug*, selector de **idioma** y **Cerrar**. |
| **Barra de estado** | Retroalimentación *Ready* / *Running <comando>…* / *Refreshing…*. |

La ventana **no** realiza ninguna operación de git al abrirse, por lo que aparece de forma instantánea. La primera sondeo (versión de LFS, patrones rastreados, archivos, rama) se ejecuta en un hilo en segundo plano justo después de mostrarse la ventana.

## Elegir el directorio de trabajo

El menú desplegable **Working Directory** se rellena con el historial de repositorios de GitExtensions (leído de `GitExtensions.settings`), de modo que cualquier repositorio que hayas abierto en el host está disponible aquí — sin importar cuál repositorio muestre actualmente la ventana del host.

- Seleccionar una entrada distinta vuelve a sondear inmediatamente ese repositorio y actualiza todas las pestañas.
- Si el host cambia de repositorio mientras la ventana está abierta, el menú desplegable lo sigue.
- Si la ruta seleccionada no es un árbol de trabajo git, la etiqueta **Branch** muestra *not a git repository* y los botones del flujo de trabajo se deshabilitan.

## Leer la consola Output

Cada botón pasa por un único ejecutor que:

1. Refleja el comando con un prefijo `$ ` (por ejemplo, `$ git lfs pull`).
2. Imprime la salida combinada de stdout + stderr del proceso.
3. Termina con `✓ Done.` en caso de éxito (código de salida 0) o `✗ Failed (exit code N).` en caso de fallo.

Mientras se ejecuta un comando, el panel de directorio de trabajo y las pestañas se deshabilitan, el cursor muestra el cursor de espera y la barra de estado indica *Running <comando>…*. Los clics simultáneos se ignoran hasta que el comando actual finaliza. Usa **Clear** para vaciar el log.

---

## Paso 1 · Instalación

![ZimerfeldLFS — pestaña de Instalación](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotInstallation.png)

Esta pestaña confirma que Git LFS está presente e inicializado para tu cuenta de usuario.

**Línea de estado (arriba, coloreada):**

| Color | Significado |
| --- | --- |
| 🟢 Verde oscuro | LFS **listo** — detectado (por ejemplo, `git-lfs/3.7.1`) *e* inicializado para tu usuario. |
| 🟡 Ocre oscuro | LFS **disponible** pero aún no inicializado — haz clic en `git lfs install`. |
| 🔴 Rojo oscuro | LFS **ausente** — no se encuentra en el `PATH`. |

"Inicializado para tu usuario" se detecta comprobando que `filter.lfs.clean` exista en tu configuración global de git (lo que escribe `git lfs install`).

**Botones:**

- **Check installation** → ejecuta `git lfs version` y actualiza la línea de estado.
- **`git lfs install`** → ejecuta `git lfs install`, configurando los filtros de LFS para tu cuenta de usuario. Se ejecuta **una vez por máquina**; está deshabilitado cuando LFS falta.

**Instalar Git LFS manualmente** (solo si el estado está en rojo):

- **Windows / macOS** — normalmente ya viene incluido con Git.
- **macOS / Linux** — `brew install git-lfs`
- **Windows** — `choco install git-lfs`
- **Cualquier SO** — binarios oficiales en [git-lfs.com](https://git-lfs.com).

---

## Paso 2 · Flujo básico — track / commit / push

![ZimerfeldLFS — pestaña de Flujo Básico](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotBasicWorkflow.png)

Esta pestaña es el ciclo del día a día: indicarle a LFS qué archivos gestionar y luego hacer commit y push.

### Rastrear un patrón

Escribe un **patrón glob** en el cuadro de texto y pulsa **Track** (o presiona <kbd>Enter</kbd>). El plugin ejecuta:

```bash
git lfs track "<pattern>"
git add .gitattributes   # se agrega al stage automáticamente en caso de éxito
```

Patrones comunes: `*.psd`, `*.mp4`, `*.zip`, `*.bin`, `assets/**`. Un patrón vacío se rechaza con un aviso.

### Lista de patrones rastreados + Untrack

La lista **Tracked patterns** refleja `git lfs track` (con la anotación final `(.gitattributes)` eliminada). Selecciona una entrada y pulsa **Untrack** para ejecutar:

```bash
git lfs untrack "<pattern>"
git add .gitattributes   # se agrega al stage automáticamente en caso de éxito
```

Untrack sin ninguna selección se rechaza con un aviso.

### Archivos gestionados por LFS

La lista **LFS-managed files** refleja `git lfs ls-files` — cada línea es `<oid> <*|-> <path>` (donde `*` significa que el archivo está materializado localmente y `-` significa que es solo un puntero). La etiqueta muestra el recuento y se actualiza después de cada comando.

### Commit & Push

- **Commit…** abre el **diálogo nativo de commit de GitExtensions** en el mismo proceso (de modo que se cargan todos los plugins de commit) para el repositorio seleccionado. La consola registra si se hicieron commits, si el diálogo se cerró sin hacer commit, o si no estaba disponible.
- **Push…** abre el **diálogo nativo de push de GitExtensions**. Si no hay ningún diálogo de push del host disponible (o se cancela), el plugin recurre a un `git push` simple en la rama actual y registra el fallback.

El equivalente manual completo de este ciclo:

```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```

Todos los controles de *track / untrack / commit / push* están deshabilitados a menos que el directorio de trabajo sea un repositorio git **y** LFS esté disponible.

---

## Paso 3 · Clonado y pull

![ZimerfeldLFS — pestaña de Clonado y Pull](https://raw.githubusercontent.com/zimerfeld/GitExtensions.ZimerfeldLFS/main/screenshots/screenshotCloningPulling.png)

Cuando colaboradores o herramientas de despliegue clonan el repositorio, Git LFS descarga los archivos pesados automáticamente al hacer checkout de la rama. Para obtener o restaurar contenido LFS más adelante, cada botón corresponde exactamente a un comando, siempre ejecutado contra el repositorio del menú desplegable **Working Directory**:

| Botón | Comando | Propósito |
| --- | --- | --- |
| **git lfs pull** | `git lfs pull` | Descarga el contenido LFS del checkout actual. |
| **git lfs fetch --all** | `git lfs fetch --all` | Prefetch de los objetos LFS de **todas** las referencias (no toca el árbol de trabajo). |
| **git lfs checkout** | `git lfs checkout` | Rellena los archivos del árbol de trabajo a partir de los objetos ya descargados. |
| **git lfs status** | `git lfs status` | Muestra el estado de los objetos LFS (**no** dispara una actualización de estado). |

Una secuencia típica de restauración tras un clon nuevo que omitió los objetos LFS: **git lfs fetch --all** → **git lfs checkout**, o simplemente **git lfs pull**.

---

## Barra inferior

- **Show Debug** — revela el `Name` interno de cada control como tooltip (una ayuda de diagnóstico); la elección se persiste en `ZimerfeldLFS.uisettings.json`.
- **Language** — *Automático* (sigue el SO), *Inglés*, *Portugués* o *Español*. Cambiarlo vuelve a etiquetar toda la ventana de inmediato.
- **Close** — cierra la ventana (también es la acción de <kbd>Esc</kbd> / Cancelar). La ventana es no modal, así que GitExtensions permanece utilizable mientras está abierta.

## Solución de problemas

| Síntoma | Causa probable y solución |
| --- | --- |
| Línea de estado en rojo / *ausente* | Git LFS no está en el `PATH`. Instálalo (ver Paso 1), vuelve a abrir la pestaña, haz clic en **Check installation**. |
| Botones del flujo de trabajo en gris | La ruta seleccionada no es un repositorio git, o LFS no está disponible. Elige un repositorio válido en el menú desplegable. |
| **Branch** muestra *not a git repository* | El menú desplegable apunta a una carpeta que no es un árbol de trabajo git. |
| Commit indica *unavailable* | El diálogo nativo de commit de GitExtensions no pudo alojarse; haz el commit desde la ventana principal de GitExtensions. |
| El comando muestra `✗ Failed (exit code N)` | Lee la salida bruta encima de la línea de fallo — es el error literal de `git`/`git lfs`. |

## Plugins integrados

Otros plugins de GitExtensions del mismo autor que combinan bien con ZimerfeldLFS:

- **[GitExtensions.ZimerfeldTree](https://github.com/zimerfeld/GitExtensions.ZimerfeldTree)**
- **[GitExtensions.ZimerfeldCommitMsg](https://github.com/zimerfeld/GitExtensions.ZimerfeldCommitMsg)**

## Licencia

Copyright © 2026 Renato Zimerfeld — **CC BY-NC-ND 4.0** (ver [`LICENSE.txt`](LICENSE.txt)).
