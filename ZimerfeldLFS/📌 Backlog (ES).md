---
tipo: backlog
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
tags: [backlog, retomada, roadmap]
---

# 📌 Backlog

> 🇧🇷 Leia em português → [[📌 Backlog]] · 🇺🇸 Read in English → [[📌 Backlog (EN)]]

> [!tip] Empieza por aquí al retomar
> Punto de retomada del proyecto **ZimerfeldLFS** en otra sesión. Lee también [[🏠 Home (ES)|🏠 Home]] y la nota madre [[📦 GitExtensions.ZimerfeldLFS]].

## ✅ Estado actual
- **Versión:** `1.0.2` (`major.minor.BUILD`, fuente de la verdad: `.nuspec` / `.csproj`).
- **Pruebas:** **36 pruebas unitarias (xUnit)** cubriendo el `LfsService`.
- **Build:** `net9.0-windows`, WinForms `Library`, empaquetado como `.nupkg` vía [[🛠️ build.ps1]].
- **Funcional:** flujo guiado en 3 etapas (Instalación / Flujo básico / Clone & Pull), directorio de trabajo independiente, i18n (Automático / EN-US / PT-BR / ES-ES), banner de patrocinio.
- **Cofre:** reestructurado al estándar "Cofre de Neuronas v2" el 2026-07-04 (pares bilingües, sortspec, custom-sort).

## 🔜 Próximos pasos (derivados de las notas)
- [ ] **Cubrir más casos de prueba** además del `LfsService` (p. ej.: parsing de `GetRepositoriesFromSettings`, i18n). Ver [[⚡ LfsService]].
- [ ] **Publicar/actualizar en NuGet** la versión actual siguiendo el runbook [[🚀 Deploy em Produção (Prod)|🚀 Despliegue en Producción (Prod)]] (GitFlow: release → tag → push).
- [ ] **Hacer seguimiento de la adopción** (clones + descargas de NuGet) del repositorio `zimerfeld/GitExtensions.ZimerfeldLFS`, según la rutina de portafolio.
- [ ] **Robustez del parsing de settings** — `GetRepositoriesFromSettings` depende del formato XML de GitExtensions (best-effort). Monitorear roturas entre versiones del host. Ver [[📁 Diretório de trabalho independente|📁 Directorio de trabajo independiente]].
- [ ] **Icono en 16×16** — muchos detalles compiten en el tamaño de menú; evaluar simplificación. Ver [[💣 Ícone 4 quadrantes + bomba|💣 Icono: 4 cuadrantes + bomba]].

## 💡 Ideas / a evaluar
- [ ] Documentar/expandir el manual de operación de la ventana (`README.en-US.md` / `README.pt-BR.md`) enlazado desde la sección "Las tres etapas".
- [ ] Reforzar el portafolio cohesionado con los hermanos [[GitExtensions.ZimerfeldTree]] y [[GitExtensions.ZimerfeldCommitMsg]].

## 🔗 Enlaces
- [[🏠 Home (ES)|🏠 Home]] · [[📦 GitExtensions.ZimerfeldLFS]] · [[🔢 Versionamento|🔢 Versionado]] · [[🚀 Deploy em Produção (Prod)|🚀 Despliegue en Producción (Prod)]]
