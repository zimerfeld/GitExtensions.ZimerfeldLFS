---
tipo: procedimento
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [operacao, runbook, prod, release, nupkg, nuget]
---

# 🚀 Despliegue en Producción (Prod)

> 🇧🇷 Lee esta página en portugués → [[🚀 Deploy em Produção (Prod)]]

Cómo publicar una nueva versión del plugin: generar el paquete `.nupkg` de release y distribuirlo (NuGet + GitHub release), desde donde el Plugin Manager de GitExtensions lo descubre.

## ⚡ TL;DR — el comando único
```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1 -Force     # genera el .nupkg de release en la raíz del repositorio
```
El artefacto de release es `GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` en la raíz. Publíquelo en NuGet y adjúntelo a la GitHub release.

## ⚙️ Qué hace el script (en orden)
`build.ps1` produce el paquete publicable (ver [[🛠️ build.ps1 (ES)|🛠️ build.ps1]] y [[🔢 Versionamento (ES)|🔢 Versionado]]):
1. Incremento de versión (`major.minor.BUILD`) en el `.nuspec` y `.csproj`.
2. Graba versión + fecha en los READMEs (`README.md` / `.pt-BR` / `.en-US`) y en este cofre.
3. `dotnet build -c Release`.
4. `nuget pack .nuspec` → `GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg` en la raíz, **filtrando el aviso NU5101** (la DLL va en `lib\` raíz — grupo "any" — para que el Plugin Manager pueda extraerla).
5. Elimina los `.nupkg` de versiones anteriores.

## 📦 Publicación (pasos manuales tras el pack)
1. **NuGet:** `nuget push GitExtensions.ZimerfeldLFS.X.Y.Z.nupkg -Source nuget.org -ApiKey <clave>`.
2. **GitHub release:** crear la release/tag y adjuntar el `.nupkg` + notas de versión.
3. **Plugin Manager:** con el paquete en NuGet y la dependencia marcadora `GitExtensions.Extensibility`, el plugin aparece en la búsqueda de *ZimerfeldLFS*.

## 📏 Reglas que respeta (GitFlow)
> [!important] Orden correcto del GitFlow (regla de Renato)
> 1. Validar el entorno de **develop** en la branch de **release** (sin error).
> 2. **Finalizar la release** actualizando `develop` y luego `main`.
> 3. Generar el **tag** `YYYYMMddhhmm<nombre-de-la-fase>`.
> 4. **Solo entonces** publicar en producción (push NuGet + GitHub release).
> - **No** hacer deploy desde la release branch antes de finalizarla.
> - **No** commitear/pushear sin pedido explícito.

## ✅ Requisitos
- Todo lo de [[💻 Ambiente Local (Dev) (ES)|💻 Entorno Local (Dev)]] (.NET 9 SDK, `nuget`).
- Clave de API de NuGet y permiso de release en GitHub (owner `zimerfeld`).

## 🩺 Troubleshooting
- **NU5101 aparece como error** → debe ser solo aviso y filtrado; confirme que la DLL está en `lib\` raíz en el `.nuspec`. Ver [[🧱 Dependências (ES)|🧱 Dependencias]].
- **El plugin no aparece en el Plugin Manager** → verifique la dependencia `GitExtensions.Extensibility` en el `.nuspec` y si el paquete está indexado en NuGet.
- **La versión no se incrementó** → ejecute con `-Force` (la detección de cambios puede haber mantenido la versión).

## 🔗 Enlaces
- [[💻 Ambiente Local (Dev) (ES)|💻 Entorno Local (Dev)]]
- [[🔢 Versionamento (ES)|🔢 Versionado]]
- [[🛠️ build.ps1 (ES)|🛠️ build.ps1]]
- [[🧱 Dependências (ES)|🧱 Dependencias]]
