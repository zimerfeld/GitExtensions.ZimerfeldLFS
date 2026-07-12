---
tipo: conhecimento
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
tags: [conhecimento, readme, instalacao, build, uso, git-lfs, i18n]
fonte: README.md
versao: 1.0.4
---

# 📖 README — Instalación, Uso y Build

> Espejo del `README.md` de la raíz del repositorio (bilingüe EN/PT), reconciliado con el código el 2026-07-01.
> Nota de proyecto: [[📦 GitExtensions.ZimerfeldLFS (ES)|📦 GitExtensions.ZimerfeldLFS]]. Conceptos en [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]].
> `build.ps1` graba versión + fecha en los READMEs **y** en este cofre en cada build (secciones 4b/4c) — ver [[🔢 Versionamento (ES)|🔢 Versionado]].

Plugin para **[GitExtensions](https://gitextensions.github.io/)** que gestiona el **Git Large File Storage (LFS)** en una **ventana dedicada y no modal**. Git LFS sustituye archivos grandes (audio, video, datasets) por **punteros de texto ligeros** en el repositorio; el contenido real queda en un servidor remoto separado, **acelerando el clonado y evitando el hinchamiento del repositorio**. La ventana conduce el flujo en **tres etapas** y usa un directorio de trabajo elegido **de forma independiente de GitExtensions**.

## ✨ Funcionalidades de alto nivel
- **Flujo guiado en tres etapas** — Instalación, Flujo básico (track/commit/push) y Clone & Pull, cada uno en su pestaña.
- **Directorio de trabajo independiente** — desplegable poblado con el historial de repositorios de GitExtensions; apunta la ventana a cualquier repo sin depender del host. Ver [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de Trabajo Independiente]].
- **Estado del LFS en vivo** — versión de `git lfs`, si está inicializado, patrones glob rastreados y archivos LFS, actualizados automáticamente.
- **Consola de salida** — cada botón muestra el comando `git`/`git lfs` exacto y su salida.
- **Localizado (Inglés / Portugués / Español)** con modo automático siguiendo el SO, más el **Mostrar Debug** que revela los IDs de los controles.

## 🧩 ¿Qué es Git LFS?
Extensión open-source de Git que cambia archivos grandes por punteros de texto ligeros; el contenido real queda en un servidor remoto, acelerando el clonado y evitando el hinchamiento. Detalles en [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]].

## 🔢 Las tres etapas

> **Guías técnicas de operación (nuevo en 2026-07-01):** el `README.md` raíz permanece como resumen bilingüe de alto nivel; los detalles de **cómo operar la ventana** se separaron en dos manuales por idioma — `README.en-US.md` (inglés) y `README.pt-BR.md` (portugués) — **enlazados desde la sección "The three steps / As três etapas"** del README raíz. Cada manual cubre: anatomía de la ventana, elección del directorio de trabajo, lectura de la consola de salida, cada botón/estado por pestaña y solución de problemas.

### 1 · Instalación
En Windows y macOS Git LFS normalmente ya viene incluido. Instalación manual vía **Homebrew** (`brew install git-lfs`), **Chocolatey** (`choco install git-lfs`) o los binarios oficiales en [git-lfs.com](https://git-lfs.com). Después, **`git lfs install`** inicializa el LFS para la cuenta (1× por máquina). **Verificar instalación** ejecuta `git lfs version`. Ver [[⚙️ Instalação (ES)|⚙️ Instalación]].

### 2 · Flujo básico — track / commit / push
Rastree tipos por **patrones glob** (`*.psd`, `*.mp4`, `*.zip`): escriba el patrón y haga clic en **Track** — el plugin ejecuta `git lfs track "<patrón>"` y **stagea el `.gitattributes`**. La ventana lista los patrones rastreados (con *Untrack*) y los archivos LFS (`git lfs ls-files`). **Commit** y **Push** abren los diálogos nativos de GitExtensions.
```bash
git lfs track "*.psd"
git add .gitattributes
git add my_large_design_file.psd
git commit -m "Add large PSD file"
git push origin main
```
Ver [[📤 Track Commit Push (ES)|📤 Track Commit Push]].

### 3 · Clone & Pull
Al clonar, Git LFS descarga los archivos pesados automáticamente en el checkout. Para buscar/restaurar después: `git lfs pull`, `git lfs fetch --all`, `git lfs checkout`, `git lfs status`. Ver [[⬇️ Clone e Pull (ES)|⬇️ Clone & Pull]].

## 📦 Instalación
**Vía Plugin Manager de GitExtensions:** busque *ZimerfeldLFS* (Plugins → Plugin Manager), instale, reinicie y abra **Plugins → ZimerfeldLFS**.

**Manual:** ejecute `build.ps1` (como Administrador para el deploy automático) o copie `GitExtensions.Plugins.ZimerfeldLFS.dll` a `C:\Program Files\GitExtensions\Plugins\`, o ejecute `tools\install.ps1` como Administrador.

## ✅ Requisitos
- GitExtensions 6.x (.NET 9)
- `git` y `git-lfs` en el `PATH`

## 🛠️ Build
```powershell
pwsh .\build.ps1          # incrementa versión, build Release, empaqueta el .nupkg
pwsh .\build.ps1 -Force   # siempre recompila/empaqueta
```
Ver [[🔢 Versionamento (ES)|🔢 Versionado]] y [[🛠️ build.ps1 (ES)|🛠️ build.ps1]].

## 💜 Apoya el proyecto
**GitHub Sponsors:** [github.com/sponsors/zimerfeld](https://github.com/sponsors/zimerfeld) · **Ko-fi:** [ko-fi.com/C0D621FCGD](https://ko-fi.com/C0D621FCGD). Badges en la parte superior del README y **banner clicable en la parte superior de la ventana** (`SponsorBanner.cs`).

## 🧩 Plugins integrados
Otros plugins de GitExtensions del mismo autor, referenciados en el pie de los READMEs:
- **[GitExtensions.ZimerfeldTree](https://github.com/zimerfeld/GitExtensions.ZimerfeldTree)** — ver [[GitExtensions.ZimerfeldTree]]
- **[GitExtensions.ZimerfeldCommitMsg](https://github.com/zimerfeld/GitExtensions.ZimerfeldCommitMsg)** — ver [[GitExtensions.ZimerfeldCommitMsg]]

## 📄 Licencia
Copyright © 2026 Zimerfeld — **CC BY-NC-ND 4.0** (`LICENSE.txt`).

## 🔗 Relacionado
- [[📦 GitExtensions.ZimerfeldLFS (ES)|📦 GitExtensions.ZimerfeldLFS]]
- [[🗃️ Git LFS - Conceitos (ES)|🗃️ Git LFS — Conceptos]]
- [[🔑 Fatos-Chave (ES)|🔑 Hechos Clave]]
