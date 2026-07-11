---
tipo: sistema
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [build, versão, nupkg, deploy]
---

# 🔢 Versionado y Build

## 🔢 Esquema de versión

`major.minor.build` — solo el `build` se incrementa automáticamente mediante `build.ps1`. Major y minor se cambian manualmente.

**Versión actual:** `1.0.4` *(fuente de la verdad: `.nuspec` / `.csproj`)*

> [!note] Diccionarios de idioma embebidos (sin satellite assemblies)
> Los textos de la UI viven en `Resources/ZimerfeldLFS.en-US.json`, `Resources/ZimerfeldLFS.pt-BR.json` y `Resources/ZimerfeldLFS.es-ES.json`,
> embebidos en el **assembly principal** con `WithCulture="false"` + `LogicalName` explícito — así
> MSBuild **no** interpreta el infijo `.en-US`/`.pt-BR`/`.es-ES` como cultura (lo que los desviaría a
> satellite assemblies), preservando el deploy de **DLL única**. Leídos en tiempo de ejecución por `I18n`.

## 🔄 Ciclo build.ps1

```
build.ps1  [-Force]
  │
  ├─ 1. Lee la versión actual desde el .nuspec
  ├─ 2. Calcula newVersion (build +1)
  ├─ 2b. Detecta cambios (fuentes/docs más recientes que el último .nupkg); sin -Force y sin
  │      cambios → mantiene la versión y sale (se omiten build/pack)
  ├─ 2c. Cierra GitExtensions si está en ejecución
  ├─ 3. Bump en el .nuspec  ← <version>
  ├─ 4. Bump en el .csproj  ← <Version>
  ├─ 4b. Sella versión + fecha en la parte superior de los READMEs (README.md / .pt-BR / .en-US / .es-ES)
  ├─ 4c. Sella versión + fecha en este cofre de Obsidian (notas que reflejan la versión)
  ├─ 5. dotnet build -c Release
  ├─ 6. Copia la DLL → C:\Program Files\GitExtensions\Plugins\  (requiere Admin)
  ├─ 6b. Copia la DLL → tools\net9.0-windows\  (para el nupkg)
  ├─ 7. nuget pack .nuspec → .nupkg en la raíz (filtra el aviso NU5101)
  └─ 7b. Elimina el .nupkg de versiones anteriores
```

> [!warning] El aviso **NU5101** es intencional
> La DLL se empaqueta en `lib\` **raíz** (grupo "any" que el Plugin Manager extrae), no en
> `lib\net9.0-windows\`. Esto genera el aviso NU5101, que `build.ps1` **filtra a propósito** en
> `nuget pack`. Detalles en [[🧱 Dependências (ES)|🧱 Dependencias]] y en el `.nuspec`.

<!-- -->

> Requiere el **.NET 9 SDK** (`dotnet`) y, para el deploy, permisos de **Administrador**. Sin Admin, el paso de deploy se omite con un aviso; `nuget` se descarga a `tools\nuget.exe` si no está en el PATH.

## 📄 Archivos versionados

| Archivo | Campo actualizado |
|---|---|
| `GitExtensions.ZimerfeldLFS.nuspec` | `<version>` |
| `GitExtensions.ZimerfeldLFS.csproj` | `<Version>` (y `**Versão atual**` + enlace de NuGet en README.md) |
| `README.md` / `README.pt-BR.md` / `README.en-US.md` / `README.es-ES.md` | `**Version/Versão:**` y `**Updated/Atualizado em:**` |

> **Nota:** `build.ps1` sella versión/fecha en los **READMEs** (sección 4b) **y** en este cofre de Obsidian (sección 4c) — las notas que reflejan la versión actual (Proyecto, README espejo, Visión general, Versionado, HOME) permanecen sincronizadas automáticamente, igual que el hermano CommitMsg.

## 🔧 Instalación / desinstalación manual

```powershell
tools\install.ps1        # requiere Admin — copia la DLL a la carpeta Plugins (también vía PMC)
tools\uninstall.ps1      # requiere Admin — elimina la DLL (no afecta al resto de GitExtensions)
```

## 🔗 Relacionado

- [[🛠️ build.ps1 (ES)|🛠️ build.ps1]]
- [[🧱 Dependências (ES)|🧱 Dependencias]]
- [[💻 Ambiente Local (Dev) (ES)|💻 Entorno local (Dev)]]
- [[🚀 Deploy em Produção (Prod) (ES)|🚀 Deploy en producción (Prod)]]
