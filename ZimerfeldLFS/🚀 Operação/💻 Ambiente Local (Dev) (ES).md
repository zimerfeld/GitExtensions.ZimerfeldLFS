---
tipo: procedimento
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [operacao, runbook, dev, build, gitextensions, powershell]
---

# 💻 Entorno Local (Dev)

> 🇧🇷 Lee esta página en portugués → [[💻 Ambiente Local (Dev)]]

Cómo compilar el plugin, instalarlo en el GitExtensions local e iterar durante el desarrollo.

## ⚡ TL;DR — el comando único
```powershell
cd C:\GitExtensions\GitExtensions.ZimerfeldLFS
.\build.ps1            # incrementa el build, compila Release, despliega en el GitExtensions local, genera el nupkg
```
Después, abra GitExtensions y vaya a **Plugins → ZimerfeldLFS**. Para forzar incluso sin cambios: `.\build.ps1 -Force`.

## ⚙️ Qué hace el script (en orden)
`build.ps1` (detallado en [[🛠️ build.ps1 (ES)|🛠️ build.ps1]]):
1. Lee la versión actual del `.nuspec` y calcula la siguiente (`build +1`).
2. **Detecta cambios** — sin `-Force` y sin fuentes/docs más recientes que el último `.nupkg`, mantiene la versión y sale.
3. **Cierra el GitExtensions** en ejecución (libera la DLL para el deploy).
4. Aplica el incremento de versión en el `.nuspec` y en el `.csproj`; graba versión + fecha en los READMEs y en este cofre.
5. `dotnet build -c Release` — falla si hay error; cuenta errores/avisos.
6. **Deploy (requiere Admin):** copia la DLL a `C:\Program Files\GitExtensions\Plugins\`.
7. Copia la DLL a `tools\net9.0-windows\` y empaqueta el `.nupkg` (filtrando el aviso **NU5101** intencional).

## 🧩 Instalación manual alternativa (tools/)
```powershell
tools\install.ps1      # requiere Admin — copia la DLL a la carpeta Plugins (también vía Package Manager Console)
tools\uninstall.ps1    # requiere Admin — elimina la DLL (no afecta al resto de GitExtensions)
```

## 🎛️ Parámetros / flags
- **`-Force`** — ignora la detección de cambios y siempre incrementa/recompila/empaqueta.

## 🔐 Requisitos
- **.NET 9 SDK** (`dotnet`) en el PATH.
- **Administrador** para el paso de deploy (si no, se omite con aviso).
- `nuget` (descargado automáticamente en `tools\nuget.exe` si falta).
- **Sin Admin** (p. ej.: vía la herramienta Bash): `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "build.ps1"` — compila y empaqueta; solo se omite el deploy.

## 📏 Reglas que respeta
- **GitFlow:** desarrollar en la feature branch actual; `build.ps1` solo compila/empaqueta, no hace commit ni push.
- **No commitear/pushear sin pedido explícito.**

## 🩺 Troubleshooting
- **"Git LFS is NOT installed"** en la ventana → `git` / `git-lfs` fuera del `PATH` (el `LfsService` se ejecuta como subproceso). Ver [[🧱 Dependências (ES)|🧱 Dependencias]].
- **Deploy omitido** → ejecute PowerShell como Administrador o use `tools\install.ps1`.
- **DLL bloqueada** → `build.ps1` cierra GitExtensions antes del deploy; si falla, ciérrelo manualmente.

## 🔗 Enlaces
- [[🛠️ build.ps1 (ES)|🛠️ build.ps1]]
- [[🔢 Versionamento (ES)|🔢 Versionado]]
- [[🧱 Dependências (ES)|🧱 Dependencias]]
- [[🚀 Deploy em Produção (Prod) (ES)|🚀 Despliegue en Producción (Prod)]]
