---
tipo: fluxo
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [fluxo, working-dir, repositorio, dropdown, desacoplamento]
---

# 📂 Flujo: Directorio de Trabajo Independiente

Cómo se puebla el dropdown `cboRepo` y por qué la ventana permanece **independiente del repositorio activo del host**.

## 🌱 Origen de los elementos

```
LfsForm (constructor) → LoadRepositories()
        │
        ▼
LfsService.GetRepositoriesFromSettings()   (helper estático)
        │  lee:  %APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings
        │  encuentra el item key="history" → el value es un STRING XML anidado
        │  parsea ese string → recolecta cada <Path>
        ▼
lista de rutas distintas (case-insensitive)
        │
        ▼
cboRepo.Items  (+ el WorkingDir inicial insertado al inicio, si no está en la lista)
        │
        ▼
preselecciona el WorkingDir inicial (o el índice 0)
```

## 🔗 Por qué es independiente

> [!important] Ningún evento del host
> El plugin **no** se suscribe a `PostRepositoryChanged` ni a ningún otro evento de GitExtensions. El working dir del host (`args.GitModule.WorkingDir`) se usa **una única vez**, solo como valor preseleccionado del `cboRepo` cuando la ventana se abre. A partir de ahí, el repositorio se elige **exclusivamente** mediante el dropdown. Ver [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]].

## 🔄 Cambio de repositorio

- **`CboRepo_SelectedIndexChanged`** — al elegir otro elemento, establece `_svc.WorkingDir = dir` y dispara `RefreshStateAsync` (vuelve a probar la versión de lfs, la branch, los patrones y los archivos del nuevo repo).
- **`UpdateWorkingDir(newDir)`** — cuando la ventana ya está abierta y el usuario vuelve a activar el menú Plugins en un repositorio diferente, el plugin llama a esto: agrega el dir al combo si es necesario y lo selecciona.

## ✨ Ventaja

La ventana es **persistente y reutilizable**: se puede dejar abierta y gestionar LFS de **cualquier** repositorio del historial, sin necesidad de cambiar el repositorio activo en la ventana principal de GitExtensions.

## 🔗 Relacionado

- [[📁 Diretório de trabalho independente (ES)|📁 Directorio de trabajo independiente]]
- [[⚡ LfsService (ES)|⚡ LfsService]] — `GetRepositoriesFromSettings`
- [[🪟 LfsForm (ES)|🪟 LfsForm]] — `LoadRepositories` / `cboRepo`
