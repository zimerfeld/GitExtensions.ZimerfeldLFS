---
tipo: decisao
projeto: GitExtensions.ZimerfeldLFS
lang: es-ES
atualizado: 2026-07-04
criado: 2026-07-01
status: aceita
tags: [decisao, adr, working-dir, desacoplamento, repositorio]
---

# 📁 ADR — Directorio de trabajo independiente del host

## 🧭 Contexto
Cada ventana de GitExtensions está vinculada a **un** repositorio activo. Atar la ventana de LFS a ese repositorio significaría que cambiar de repo en el host cambiaría (o rompería) el destino de la ventana — exactamente el tipo de bug de "dejó de funcionar al cambiar de repositorio" que el hermano CommitMsg tuvo que sortear.

## ✅ Decisión
Dar a la ventana su **propio dropdown de repositorio** (`cboRepo`), poblado a partir del **historial de repositorios de GitExtensions** (leído del archivo `GitExtensions.settings`), y usar el working dir del host **solo una vez** como valor preseleccionado. El plugin **no se suscribe a ningún evento** del host; el repositorio destino se elige exclusivamente mediante el dropdown.

## 🔀 Alternativas consideradas
- **Seguir el repositorio activo del host** (escuchar `PostRepositoryChanged`) — acoplamiento y fragilidad; el destino cambiaría bajo los pies del usuario.
- **Pedir la ruta manualmente** — sin descubrimiento; peor UX.
- **Dropdown del historial (elegida)** — descubre repositorios automáticamente, mantiene la ventana estable y reutilizable.

## 🛠️ Implementación
- `LfsService.GetRepositoriesFromSettings()` lee `%APPDATA%\GitExtensions\GitExtensions\GitExtensions.settings`, extrae el elemento `key="history"` (cuyo value es una cadena XML anidada) y recolecta cada `<Path>`.
- `LfsForm.LoadRepositories()` puebla el combo, inserta el working dir inicial en la parte superior si está ausente y lo preselecciona.
- `CboRepo_SelectedIndexChanged` cambia `_svc.WorkingDir` y vuelve a sondear el estado. Ver [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de trabajo independiente]].

## ⚖️ Consecuencias
**Positivas:**
- Ventana persistente que gestiona LFS para **cualquier** repo del historial.
- Desacoplamiento total → robustez entre versiones y ausencia de bugs de "repo equivocado".

**Negativas / trade-offs:**
- Depende del formato del XML de settings de GitExtensions (parsing best-effort, tolerante a fallos → lista vacía).
- Los repositorios fuera del historial no aparecen (solo el inicial se inserta manualmente).

## 🔗 Relacionado
- [[🪟 Janela dedicada não-modal (ES)|🪟 Ventana dedicada no modal]]
- [[📂 Diretório de Trabalho Independente (ES)|📂 Directorio de trabajo independiente]]
- [[⚡ LfsService (ES)|⚡ LfsService]]
